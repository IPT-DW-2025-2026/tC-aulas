using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

using Aulas.Data;
using Aulas.Data.Model;

namespace Aulas.Pages.Students {

   public class CreateModel:PageModel {

      private readonly ApplicationDbContext _context;

      public CreateModel(ApplicationDbContext context) {
         _context = context;
      }



      public IActionResult OnGet() {

         // list of options to appear in the dropdown list for the Degree property
         ViewData["DegreeFK"] = new SelectList(_context.Degrees.OrderBy(d => d.Name), "Id", "Name");
     
         return Page();
      }


      [BindProperty]
      public Student Student { get; set; } = default!;



      // For more information, see https://aka.ms/RazorPagesCRUD.
      public async Task<IActionResult> OnPostAsync() {
       
         if(!ModelState.IsValid) {
            // list of options to appear in the dropdown list for the Degree property
            ViewData["DegreeFK"] = new SelectList(_context.Degrees.OrderBy(d => d.Name), "Id", "Name");

            return Page();
         }

         _context.Students.Add(Student);
         await _context.SaveChangesAsync();

         return RedirectToPage("./Index");
      }
   }
}
