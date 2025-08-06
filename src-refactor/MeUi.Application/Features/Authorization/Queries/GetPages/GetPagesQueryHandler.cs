using MediatR;
using MeUi.Application.Common.Interfaces;
using MeUi.Application.Features.Authorization.Models;
using MeUi.Domain.Entities;

namespace MeUi.Application.Features.Authorization.Queries.GetPages;

public class GetPagesQueryHandler : IRequestHandler<GetPagesQuery, IEnumerable<PageDto>>
{
    private readonly IRepository<Page> _pageRepository;

    public GetPagesQueryHandler(IRepository<Page> pageRepository)
    {
        _pageRepository = pageRepository;
    }

    public async Task<IEnumerable<PageDto>> Handle(GetPagesQuery request, CancellationToken cancellationToken)
    {
        var pages = await _pageRepository.GetAllAsync(cancellationToken);

        return pages
            .OrderBy(p => p.Name)
            .Select(p => new PageDto
            {
                Id = p.Id,
                ParentId = p.ParentId,
                PageGroupId = p.PageGroupId,
                Code = p.Code,
                Name = p.Name,
                Path = p.Path,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            });
    }
}