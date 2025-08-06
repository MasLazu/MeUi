using Mapster;
using System.Reflection;

namespace MeUi.Application.Common.Mappings;

public static class MappingConfig
{
    public static void Configure()
    {
        // Configure global settings
        TypeAdapterConfig.GlobalSettings.Default.PreserveReference(true);
        TypeAdapterConfig.GlobalSettings.Default.NameMatchingStrategy(NameMatchingStrategy.Flexible);

        // Scan and apply all mapping configurations from the assembly
        var assembly = Assembly.GetExecutingAssembly();
        TypeAdapterConfig.GlobalSettings.Scan(assembly);
    }
}

public interface IMapFrom<T>
{
    void Mapping(TypeAdapterConfig config) => config.NewConfig(typeof(T), GetType());
}

public interface IMapTo<T>
{
    void Mapping(TypeAdapterConfig config) => config.NewConfig(GetType(), typeof(T));
}