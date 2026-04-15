using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Infrastructure.Data.Configurations;

namespace Infrastructure.Data
{
    public partial class ChinookManagerContext(DbContextOptions<ChinookManagerContext> options) : IdentityDbContext(options)
    {

    }
}
