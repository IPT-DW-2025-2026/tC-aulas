using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

using Aulas.Data;
using Aulas.Data.Model;
using System.Globalization;

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

         // Convert the TuitionFeeAux string to a decimal
         // and assign it to the TuitionFee property
         Student.TuitionFee = Convert.ToDecimal(Student.TuitionFeeAux.Replace('.', ','),
                                                new CultureInfo("pt-PT"));

         try {
            _context.Students.Add(Student);
            await _context.SaveChangesAsync();
         }
         catch(Exception) {
            // do not forget that YOU MUST handle the exception
            // in a way that is appropriate for your application
            // DO NOT USE a 'throw' in a production application,
            // because it will present to many details about the exception to the user,
            // which IS a security risk.
            throw;
         }


         return RedirectToPage("./Index");
      }
   }
}
