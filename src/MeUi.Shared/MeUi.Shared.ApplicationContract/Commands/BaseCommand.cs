using System.Data.Common;
using System.Text.Json.Serialization;
using FastEndpoints;
using System.ComponentModel;

namespace MeUi.Shared.ApplicationContract.Commands;

public abstract class BaseCommand<T> : ICommand<T>
{
    [JsonIgnore]
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public DbTransaction? Transaction { get; set; }
}