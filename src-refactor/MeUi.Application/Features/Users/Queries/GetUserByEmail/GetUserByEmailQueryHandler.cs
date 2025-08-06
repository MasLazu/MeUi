using MapsterMapper;
using MediatR;
using MeUi.Application.Common.Interfaces;
using MeUi.Application.Features.Users.Models;
using MeUi.Domain.Entities;

namespace MeUi.Application.Features.Users.Queries.GetUserByEmail;

public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, UserDto?>
{
    private readonly IRepository<User> _userRepository;
    private readonly IMapper _mapper;

    public GetUserByEmailQueryHandler(
        IRepository<User> userRepository,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<UserDto?> Handle(GetUserByEmailQuery request, CancellationToken ct)
    {
        var user = await _userRepository.FirstOrDefaultAsync(
            u => u.Email == request.Email, ct);

        return user == null ? null : _mapper.Map<UserDto>(user);
    }
}