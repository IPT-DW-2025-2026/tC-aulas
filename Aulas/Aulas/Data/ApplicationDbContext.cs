using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Aulas.Data {
   public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):IdentityDbContext(options) {
   }
}
