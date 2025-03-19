using FDBInsights.Data;
using FDBInsights.Models;

namespace FDBInsights.Repositories.Implementation;

public class RoleRepository(ApplicationDbContext dbContext, ApplicationDbContext context)
    : GenericRepository<UserRole>(dbContext), IRoleRepository
{
    private readonly ApplicationDbContext _context = context;
}