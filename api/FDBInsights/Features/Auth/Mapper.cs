using FastEndpoints;
using FDBInsights.Models;

namespace FDBInsights.Features.Auth;

public class UserMapper : Mapper<AuthRequest, AuthResponse, User>
{
    public override User ToEntity(AuthRequest r)
    {
        return new User
        {
            FullName = r.username,
            UserName = r.username
        };
    }

    public override AuthResponse FromEntity(User e)
    {
        return new AuthResponse("token", "refreshtoken");
    }
}