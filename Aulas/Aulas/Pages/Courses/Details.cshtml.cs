using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using Aulas.Data.Model;
using Aulas.Data;

namespace Aulas.Pages.Courses;

public class DetailsModel : PageModel
{
    private readonly ApplicationDbContext _context;
    public DetailsModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public Course Course { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var course = await _context.Courses.FirstOrDefaultAsync(m => m.Id == id);
        if (course is null)
        {
            return NotFound();
        }
        else
        {
            Course = course;
        }

        return Page();
    }
}
