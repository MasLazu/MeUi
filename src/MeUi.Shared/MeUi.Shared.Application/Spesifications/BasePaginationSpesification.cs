using System.Linq.Expressions;
using System.Reflection;
using Ardalis.Specification;
using MeUi.Shared.ApplicationContract.Queries;
using MeUi.Shared.Domain.Entities;

namespace MeUi.Shared.Application.Spesifications;

public abstract class BasePaginationSpecification<TEntity> : Specification<TEntity> where TEntity : BaseEntity
{

    protected void ApplyPagination(BasePaginationQuery<TEntity> query)
    {
        ApplyFiltering(query);
        ApplySorting(query.Sorts);

        Query
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize);

        Query
            .AsNoTracking()
            .AsSplitQuery();
    }

    protected void ApplyFiltering(BasePaginationQuery<TEntity> query)
    {
        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            ApplySearch(query.Search);
        }

        ApplyFilters(query.Filters);
    }

    protected virtual void ApplySearch(string searchTerm)
    {
        List<PropertyInfo> searchableProperties = GetSearchableProperties();

        if (!searchableProperties.Any())
        {
            return;
        }

        ParameterExpression parameter = Expression.Parameter(typeof(TEntity), "x");
        Expression? searchExpression = null;

        foreach (PropertyInfo property in searchableProperties)
        {
            MemberExpression propertyAccess = Expression.Property(parameter, property);
            ConstantExpression searchValue = Expression.Constant(searchTerm);

            MethodInfo containsMethod = typeof(string).GetMethod("Contains", [typeof(string)])!;
            MethodCallExpression containsCall = Expression.Call(propertyAccess, containsMethod, searchValue);

            searchExpression = searchExpression == null ? containsCall : Expression.OrElse(searchExpression, containsCall);
        }

        if (searchExpression != null)
        {
            var lambda = Expression.Lambda<Func<TEntity, bool>>(searchExpression, parameter);
            Query.Where(lambda);
        }
    }

    private static List<PropertyInfo> GetSearchableProperties()
    {
        return typeof(TEntity)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.PropertyType == typeof(string) &&
                       p.CanRead &&
                       IsSearchableProperty(p.Name))
            .ToList();
    }

    private static bool IsSearchableProperty(string propertyName)
    {
        string[] searchablePatterns =
        [
            "Name", "Title", "Description", "Code", "Text", "Content",
            "Subject", "Summary", "Label", "Caption", "Comment", "Note"
        ];

        return searchablePatterns.Any(pattern =>
            propertyName.Contains(pattern, StringComparison.OrdinalIgnoreCase));
    }

    protected virtual void ApplyFilters(ICollection<FilterPaginationQuery> filters)
    {
        foreach (FilterPaginationQuery filter in filters)
        {
            ApplyFilter(filter);
        }
    }

    protected virtual void ApplyFilter(FilterPaginationQuery filter)
    {
        ParameterExpression parameter = Expression.Parameter(typeof(TEntity), "x");
        MemberExpression? property = GetPropertyExpression(parameter, filter.Field);

        if (property == null)
        {
            return;
        }

        Expression? filterExpression = CreateFilterExpression(parameter, property, filter);

        if (filterExpression != null)
        {
            var lambda = Expression.Lambda<Func<TEntity, bool>>(filterExpression, parameter);
            Query.Where(lambda);
        }
    }

    protected virtual void ApplySorting(ICollection<SortPaginationQuery> sorts)
    {
        if (!sorts.Any())
        {
            return;
        }

        SortPaginationQuery firstSort = sorts.First();
        ParameterExpression parameter = Expression.Parameter(typeof(TEntity), "x");
        MemberExpression? property = GetPropertyExpression(parameter, firstSort.Field);

        if (property != null)
        {
            var lambda = Expression.Lambda<Func<TEntity, object?>>(
                Expression.Convert(property, typeof(object)), parameter);

            if (string.Equals(firstSort.Direction, "desc", StringComparison.OrdinalIgnoreCase))
            {
                Query.OrderByDescending(lambda);
            }
            else
            {
                Query.OrderBy(lambda);
            }

            foreach (SortPaginationQuery sort in sorts.Skip(1))
            {
                MemberExpression? additionalProperty = GetPropertyExpression(parameter, sort.Field);
                if (additionalProperty != null)
                {
                    var additionalLambda = Expression.Lambda<Func<TEntity, object?>>(
                        Expression.Convert(additionalProperty, typeof(object)), parameter);

                    if (string.Equals(sort.Direction, "desc", StringComparison.OrdinalIgnoreCase))
                    {
                        Query.OrderByDescending(additionalLambda);
                    }
                    else
                    {
                        Query.OrderBy(additionalLambda);
                    }
                }
            }
        }
    }

    protected static MemberExpression? GetPropertyExpression(ParameterExpression parameter, string propertyName)
    {
        try
        {
            PropertyInfo? property = typeof(TEntity).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            return property != null ? Expression.Property(parameter, property) : null;
        }
        catch
        {
            return null;
        }
    }

    private static Expression? CreateFilterExpression(ParameterExpression parameter, MemberExpression property, FilterPaginationQuery filter)
    {
        if (filter.Value == null && filter.Op != "null" && filter.Op != "notnull")
        {
            return null;
        }

        try
        {
            Type propertyType = property.Type;
            Type? nullableType = Nullable.GetUnderlyingType(propertyType);
            Type actualType = nullableType ?? propertyType;

            if (filter.Op == "null")
            {
                return Expression.Equal(property, Expression.Constant(null));
            }

            if (filter.Op == "notnull")
            {
                return Expression.NotEqual(property, Expression.Constant(null));
            }

            object? convertedValue = ConvertValue(filter.Value, actualType);
            Expression valueExpression = Expression.Constant(convertedValue, propertyType);

            return filter.Op.ToLower() switch
            {
                "eq" => Expression.Equal(property, valueExpression),
                "ne" => Expression.NotEqual(property, valueExpression),
                "gt" => Expression.GreaterThan(property, valueExpression),
                "gte" => Expression.GreaterThanOrEqual(property, valueExpression),
                "lt" => Expression.LessThan(property, valueExpression),
                "lte" => Expression.LessThanOrEqual(property, valueExpression),
                "contains" when actualType == typeof(string) =>
                    Expression.Call(property, typeof(string).GetMethod("Contains", [typeof(string)])!, valueExpression),
                "startswith" when actualType == typeof(string) =>
                    Expression.Call(property, typeof(string).GetMethod("StartsWith", [typeof(string)])!, valueExpression),
                "endswith" when actualType == typeof(string) =>
                    Expression.Call(property, typeof(string).GetMethod("EndsWith", [typeof(string)])!, valueExpression),
                _ => null
            };
        }
        catch
        {
            return null;
        }
    }

    private static object? ConvertValue(object? value, Type targetType)
    {
        if (value == null)
        {
            return null;
        }

        try
        {
            if (targetType == typeof(string))
            {
                return value.ToString();
            }

            string? stringValue = value.ToString();
            if (string.IsNullOrEmpty(stringValue))
            {
                return null;
            }

            if (targetType == typeof(DateTime))
            {
                if (DateTime.TryParse(stringValue, out DateTime dateTime))
                {
                    return dateTime;
                }
                if (DateTimeOffset.TryParse(stringValue, out DateTimeOffset dateTimeOffset))
                {
                    return dateTimeOffset.DateTime;
                }
                return null;
            }

            if (targetType == typeof(DateTimeOffset))
            {
                if (DateTimeOffset.TryParse(stringValue, out DateTimeOffset dateTimeOffset))
                {
                    return dateTimeOffset;
                }
                return null;
            }

            if (targetType == typeof(TimeSpan))
            {
                if (TimeSpan.TryParse(stringValue, out TimeSpan timeSpan))
                {
                    return timeSpan;
                }
                return null;
            }

            if (targetType == typeof(Guid))
            {
                if (Guid.TryParse(stringValue, out Guid guid))
                {
                    return guid;
                }
                return null;
            }

            if (targetType.IsEnum)
            {
                if (Enum.TryParse(targetType, stringValue, true, out object? enumValue))
                {
                    return enumValue;
                }
                if (int.TryParse(stringValue, out int intValue) && Enum.IsDefined(targetType, intValue))
                {
                    return Enum.ToObject(targetType, intValue);
                }
                return null;
            }

            if (targetType == typeof(int))
            {
                if (value is int intVal)
                {
                    return intVal;
                }
                if (value is double doubleVal)
                {
                    return (int)doubleVal;
                }
                if (value is decimal decimalVal)
                {
                    return (int)decimalVal;
                }
                if (int.TryParse(stringValue, out int parsedInt))
                {
                    return parsedInt;
                }
                return null;
            }

            if (targetType == typeof(long))
            {
                if (value is long longVal)
                {
                    return longVal;
                }
                if (value is int intVal)
                {
                    return (long)intVal;
                }
                if (value is double doubleVal)
                {
                    return (long)doubleVal;
                }
                if (long.TryParse(stringValue, out long parsedLong))
                {
                    return parsedLong;
                }
                return null;
            }

            if (targetType == typeof(decimal))
            {
                if (value is decimal decimalVal)
                {
                    return decimalVal;
                }
                if (value is double doubleVal)
                {
                    return (decimal)doubleVal;
                }
                if (value is int intVal)
                {
                    return (decimal)intVal;
                }
                if (decimal.TryParse(stringValue, out decimal parsedDecimal))
                {
                    return parsedDecimal;
                }
                return null;
            }

            if (targetType == typeof(double))
            {
                if (value is double doubleVal)
                {
                    return doubleVal;
                }
                if (value is decimal decimalVal)
                {
                    return (double)decimalVal;
                }
                if (value is int intVal)
                {
                    return (double)intVal;
                }
                if (double.TryParse(stringValue, out double parsedDouble))
                {
                    return parsedDouble;
                }
                return null;
            }

            if (targetType == typeof(float))
            {
                if (value is float floatVal)
                {
                    return floatVal;
                }
                if (value is double doubleVal)
                {
                    return (float)doubleVal;
                }
                if (value is int intVal)
                {
                    return (float)intVal;
                }
                if (float.TryParse(stringValue, out float parsedFloat))
                {
                    return parsedFloat;
                }
                return null;
            }

            if (targetType == typeof(bool))
            {
                if (value is bool boolVal)
                {
                    return boolVal;
                }
                if (bool.TryParse(stringValue, out bool parsedBool))
                {
                    return parsedBool;
                }
                string lowerValue = stringValue.ToLower();
                if (lowerValue == "1" || lowerValue == "yes" || lowerValue == "y")
                {
                    return true;
                }
                if (lowerValue == "0" || lowerValue == "no" || lowerValue == "n")
                {
                    return false;
                }
                return null;
            }

            return Convert.ChangeType(value, targetType);
        }
        catch
        {
            return null;
        }
    }
}