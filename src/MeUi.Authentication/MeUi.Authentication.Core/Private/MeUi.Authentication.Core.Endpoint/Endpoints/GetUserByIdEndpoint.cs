using MeUi.Authentication.Core.ApplicationContract.Dtos;
using MeUi.Authentication.Core.ApplicationContract.Queries;
using MeUi.Shared.Endpoint.Responses;
using Microsoft.AspNetCore.Http;
using FastEndpoints;
using System.Net;
using MeUi.Shared.Application.Exceptions;
using MeUi.Authorization.Core.Shared.Endpoint.Endpoints;
using MeUi.Authorization.Core.Application.Interfaces;

namespace MeUi.Authentication.Core.Endpoint.Endpoints;

public class GetUserByIdEndpoint : EndointWithAuthorization<GetUserByIdEndpoint, GetUserByIdQuery, SuccessResponse<UserDto>>, IResourceActionProvider
{
    public override void Configure()
    {
        base.Configure();
        Get("/v1/users/{id}");
        AllowAnonymous();
        Description(x => x.WithTags("User"));
        Summary(s =>
        {
            s.Summary = "Retrieve a user by ID";
            s.Description = "Returns detailed information about a user identified by their unique ID.";
        });
    }

    public static string Action() => "READ";
    public static string Resource() => "USER";

    public override async Task HandleAsync(GetUserByIdQuery req, CancellationToken ct)
    {
        UserDto? user = await req.ExecuteAsync(ct) ?? throw new NotFoundException("User not found");

        var response = new SuccessResponse<UserDto>(
            user,
            "User retrieved successfully",
            HttpStatusCode.OK
        );

        await Send.ResponseAsync(response, (int)HttpStatusCode.OK, ct);
    }
}