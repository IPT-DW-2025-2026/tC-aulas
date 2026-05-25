using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using Aulas.Data.Model;
using Aulas.Data;

using System.Security.Claims;

namespace Aulas.Pages.Courses;

public class IndexModel:PageModel {
   private readonly ApplicationDbContext _context;

   public IndexModel(ApplicationDbContext context) {
      _context = context;
   }

   public IList<Course> Courses { get; set; } = default!;

   /// <summary>
   /// list of courses' IDs that the user is professor of, 
   /// to be used in the Index.cshtml to show the 'Edit' and 'Delete' buttons only 
   /// for those courses
   /// </summary>
   public List<int> MyCoursesIds { get; set; } =   [];


   public async Task OnGetAsync() {

      // Search for users' Username
      string? username = User.Identity?.Name; // the username is the email address
      //                                        that he uses to 'enter' on system                                       

      // search for the user's ID
      var userId=User.FindFirstValue(ClaimTypes.NameIdentifier); // retrieves the user's ID


      // shearch for 'courses' that the user is professor
      // in SQL
      // SELECT Id
      // FROM Courses c INNER JOIN Professors p ON c.ProfessorFK = p.Id
      // WHERE p.UserId = 'userId'
      //
      // in Linq
      var myCoursesIds= await _context.Courses
                                      .Include(c=>c.ProfessorsList)
                                      .Where(c=>c.ProfessorsList.Any(p=>p.UserID==userId))
                                      .Select(c=>c.Id)
                                      .ToListAsync();

      MyCoursesIds = myCoursesIds;


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
