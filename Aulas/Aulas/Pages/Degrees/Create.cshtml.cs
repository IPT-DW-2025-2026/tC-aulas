using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Aulas.Data;
using Aulas.Data.Model;

namespace Aulas.Pages.Degrees
{
    public class CreateModel : PageModel
    {
        private readonly Aulas.Data.ApplicationDbContext _context;

        public CreateModel(Aulas.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Degree Degree { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Degrees.Add(Degree);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
