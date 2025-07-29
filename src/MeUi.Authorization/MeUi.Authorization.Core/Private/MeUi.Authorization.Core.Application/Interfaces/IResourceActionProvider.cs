namespace MeUi.Authorization.Core.Application.Interfaces;

public interface IResourceActionProvider
{
    static abstract string Resource();
    static abstract string Action();
}
