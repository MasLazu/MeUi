using MediatR;
using MeUi.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace MeUi.Application.Common.Behaviors;

public class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ICurrentUser _currentUser;
    private readonly IAuthorizationService _authorizationService;
    private readonly ILogger<AuthorizationBehavior<TRequest, TResponse>> _logger;

    public AuthorizationBehavior(
        ICurrentUser currentUser,
        IAuthorizationService authorizationService,
        ILogger<AuthorizationBehavior<TRequest, TResponse>> logger)
    {
        _currentUser = currentUser;
        _authorizationService = authorizationService;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var requestName = typeof(TRequest).Name;

        // Check if the request has authorization attributes
        var authorizeAttributes = typeof(TRequest).GetCustomAttributes<RequirePermissionAttribute>().ToList();

        if (!authorizeAttributes.Any())
        {
            return await next();
        }

        if (!_currentUser.IsAuthenticated)
        {
            _logger.LogWarning("Unauthorized access attempt to {RequestName}", requestName);
            throw new UnauthorizedAccessException($"Authentication required for {requestName}");
        }

        foreach (var attribute in authorizeAttributes)
        {
            var isAuthorized = await CheckPermissionAsync(attribute, ct);

            if (!isAuthorized)
            {
                _logger.LogWarning("Access denied for user {UserId} to {RequestName}. Required permission: {Permission}",
                    _currentUser.UserId, requestName, attribute.Permission);
                throw new UnauthorizedAccessException($"Access denied for {requestName}. Required permission: {attribute.Permission}");
            }
        }

        _logger.LogDebug("Authorization successful for user {UserId} to {RequestName}",
            _currentUser.UserId, requestName);

        return await next();
    }

    private async Task<bool> CheckPermissionAsync(RequirePermissionAttribute attribute, CancellationToken ct)
    {
        // Single permission check
        if (!string.IsNullOrEmpty(attribute.Permission))
        {
            return await _authorizationService.HasPermissionAsync(attribute.Permission, ct);
        }

        // Multiple permissions with AND logic (all required)
        if (attribute.AllPermissions?.Any() == true)
        {
            return await _authorizationService.HasAllPermissionsAsync(attribute.AllPermissions, ct);
        }

        // Multiple permissions with OR logic (any required)
        if (attribute.AnyPermissions?.Any() == true)
        {
            return await _authorizationService.HasAnyPermissionAsync(attribute.AnyPermissions, ct);
        }

        // If no permissions specified, just check if authenticated
        return true;
    }
}

/// <summary>
/// Attribute to specify permission requirements for commands/queries.
/// Examples: 
/// - [RequirePermission("read:user")]
/// - [RequirePermission(AnyPermissions = new[] { "read:user", "read:admin" })]
/// - [RequirePermission(AllPermissions = new[] { "read:user", "write:user" })]
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class RequirePermissionAttribute : Attribute
{
    /// <summary>
    /// Single permission required (e.g., "read:user", "write:order")
    /// </summary>
    public string? Permission { get; set; }

    /// <summary>
    /// User must have ANY of these permissions (OR logic)
    /// </summary>
    public string[]? AnyPermissions { get; set; }

    /// <summary>
    /// User must have ALL of these permissions (AND logic)
    /// </summary>
    public string[]? AllPermissions { get; set; }

    /// <summary>
    /// Constructor for single permission
    /// </summary>
    /// <param name="permission">Permission in format "action:resource" (e.g., "read:user")</param>
    public RequirePermissionAttribute(string permission)
    {
        Permission = permission;
    }

    /// <summary>
    /// Parameterless constructor for use with named parameters
    /// </summary>
    public RequirePermissionAttribute()
    {
    }
}