using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Aulas.Data;
using Aulas.Data.Model;

namespace Aulas.Pages.Degrees
{
    public class EditModel : PageModel
    {
        private readonly Aulas.Data.ApplicationDbContext _context;

        public EditModel(Aulas.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Degree Degree { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var degree =  await _context.Degrees
                                        .FirstOrDefaultAsync(m => m.Id == id);
            if (degree == null)
            {
                return NotFound();
            }
            Degree = degree;

            // use of cookies to save the data you send to the browser
            HttpContext.Session.SetInt32("DegreeId", Degree.Id);
            
            // if you are using MVC,
            // you can store also the name of your action
            HttpContext.Session.SetString("ActionName", "Degree/Edit");

         return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {

         // retrieve the data from the cookie
         var degreeId = HttpContext.Session.GetInt32("DegreeId");
         var actionName = HttpContext.Session.GetString("ActionName");

         // verify if you do not have spend to much time 
         // and the cookie is not expired
         if(degreeId == null || actionName == null)
         {
            ModelState.AddModelError(string.Empty, "Session expired. Please restart the action.");
            return Page();
         }

         // you have data on your cookie, but that data is ok?
         if(degreeId != Degree.Id || actionName != "Degree/Edit") {
            // if you enter here, it means that the user has change the data
            // and tries to make harm to your system
            return RedirectToPage("./Index");
         }

         // if you enter here, it means that the user has change ONLY
         // the data that he should change

         if(!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Degree).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DegreeExists(Degree.Id))
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

        private bool DegreeExists(int id)
        {
            return _context.Degrees.Any(e => e.Id == id);
        }
    }
}
