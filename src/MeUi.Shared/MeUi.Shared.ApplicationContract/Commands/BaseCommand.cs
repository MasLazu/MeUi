using System.Data.Common;
using System.Text.Json.Serialization;
using FastEndpoints;

namespace MeUi.Shared.ApplicationContract.Commands;

public abstract class BaseCommand<T> : ICommand<T>
{
    [JsonIgnore]
    public DbTransaction? Transaction { get; set; }
}