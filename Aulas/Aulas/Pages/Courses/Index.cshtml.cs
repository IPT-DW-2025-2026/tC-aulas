using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using Aulas.Data.Model;
using Aulas.Data;

namespace Aulas.Pages.Courses;

public class IndexModel:PageModel {
   private readonly ApplicationDbContext _context;

   public IndexModel(ApplicationDbContext context) {
      _context = context;
   }

   public IList<Course> Courses { get; set; } = default!;

   public async Task OnGetAsync() {
      /*
       * SELECT * 
       * FROM Courses c INNER JOIN Professors p ON c.ProfessorFK = p.Id
       *                INNER JOIN Degrees d ON c.DegreeFK = d.Id
       * ORDER BY c.Name
       */
      Courses = await _context.Courses
                              .Include(c=>c.ProfessorsList)
                              .Include(c=>c.Degree)
                              .OrderBy(c => c.Name)
                              .ToListAsync();
      // LINQ: Language Integrated Query
   }
}
