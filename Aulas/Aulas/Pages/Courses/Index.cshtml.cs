using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using Aulas.Data.Model;
using Aulas.Data;

namespace Aulas.Pages.Courses;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public IList<Course> Course { get; set; } = default!;

    public async Task OnGetAsync()
    {
        Course = await _context.Courses.ToListAsync();
    }
}
