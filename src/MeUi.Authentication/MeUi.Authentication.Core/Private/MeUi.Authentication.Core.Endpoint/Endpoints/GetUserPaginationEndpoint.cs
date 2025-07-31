using MeUi.Authentication.Core.ApplicationContract.Dtos;
using MeUi.Authentication.Core.ApplicationContract.Queries;
using MeUi.Shared.Endpoint.Responses;
using Microsoft.AspNetCore.Http;
using FastEndpoints;
using System.Net;
using MeUi.Authorization.Core.Shared.Endpoint.Endpoints;
using MeUi.Authorization.Core.Application.Interfaces;
using MeUi.Shared.ApplicationContract.Queries;

namespace MeUi.Authentication.Core.Endpoint.Endpoints;

public class GetUserPaginationEndpoint : EndointWithAuthorization<GetUserPaginationEndpoint, GetUserPaginationQuery, SuccessResponse<BasePaginationQueryResult<UserDto>>>, IResourceActionProvider
{
    public override void Configure()
    {
        base.Configure();
        Post("/v1/users/Pagination");
        Description(x => x.WithTags("User"));
        Summary(s =>
        {
            s.Summary = "Retrieve paginated list of users";
            s.Description = "Returns a paginated list of users based on optional filters, sorting, and search criteria.";
        });
    }

    public static string Action() => "READ";
    public static string Resource() => "USER";

    public override async Task HandleAsync(GetUserPaginationQuery req, CancellationToken ct)
    {
        BasePaginationQueryResult<UserDto> result = await req.ExecuteAsync(ct);

        var response = new SuccessResponse<BasePaginationQueryResult<UserDto>>(
            result,
            "User list retrieved successfully",
            HttpStatusCode.OK
        );

        await Send.ResponseAsync(response, (int)HttpStatusCode.OK, ct);
    }
}