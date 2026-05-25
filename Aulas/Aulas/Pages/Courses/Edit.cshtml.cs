using Aulas.Data;
using Aulas.Data.Model;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using System.Security.Claims;

namespace Aulas.Pages.Courses;

[Authorize(Roles = "Professor")]
public class EditModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public EditModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Course Course { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        // to protect from not allowed professor to change the couurse
        // I need to include the professors list to check if the current user
        // is in the list of professors that can change the course
        var course = await _context.Courses
                                   .Include(c => c.ProfessorsList)
                                   .FirstOrDefaultAsync(m => m.Id == id);
        if (course is null)
        {
            return NotFound();
        }

        // if the authenticated professor authorized to change the course?
        //var userId= User.Claims.FirstOrDefault(c => c.Type == "UserID")?.Value;
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if(!course.ProfessorsList.Any(p=>p.UserID == userId)) {
         // the user change the URL on browser
         // I will redirect to Index page
         return RedirectToPage("./Index");
      }

        // if I am here, the user is authorized to change the course
        Course = course;
        return Page();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Attach(Course).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CourseExists(Course.Id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToPage("./Index");
    }

    private bool CourseExists(int id)
    {
        return _context.Courses.Any(e => e.Id == id);
    }
}
