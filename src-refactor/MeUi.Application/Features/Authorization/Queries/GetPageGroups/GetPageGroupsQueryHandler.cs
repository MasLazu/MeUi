using MediatR;
using MeUi.Application.Common.Interfaces;
using MeUi.Application.Features.Authorization.Models;
using MeUi.Domain.Entities;

namespace MeUi.Application.Features.Authorization.Queries.GetPageGroups;

public class GetPageGroupsQueryHandler : IRequestHandler<GetPageGroupsQuery, IEnumerable<PageGroupDto>>
{
    private readonly IRepository<PageGroup> _pageGroupRepository;
    private readonly IRepository<Page> _pageRepository;

    public GetPageGroupsQueryHandler(
        IRepository<PageGroup> pageGroupRepository,
        IRepository<Page> pageRepository)
    {
        _pageGroupRepository = pageGroupRepository;
        _pageRepository = pageRepository;
    }

    public async Task<IEnumerable<PageGroupDto>> Handle(GetPageGroupsQuery request, CancellationToken cancellationToken)
    {
        var pageGroups = await _pageGroupRepository.GetAllAsync(cancellationToken);
        var pages = await _pageRepository.GetAllAsync(cancellationToken);

        return pageGroups
            .OrderBy(pg => pg.Name)
            .Select(pg => new PageGroupDto
            {
                Id = pg.Id,
                Code = pg.Code,
                Name = pg.Name,
                Icon = pg.Icon,
                CreatedAt = pg.CreatedAt,
                UpdatedAt = pg.UpdatedAt,
                Pages = pages
                    .Where(p => p.PageGroupId == pg.Id)
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
                    }).ToList()
            });
    }
}