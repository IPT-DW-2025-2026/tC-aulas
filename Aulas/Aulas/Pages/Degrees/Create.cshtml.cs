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
using System.ComponentModel.DataAnnotations;

namespace Aulas.Pages.Degrees {

   public class CreateModel:PageModel {

      /// <summary>
      /// represents the database context of the application, 
      /// allowing access to the database and its operations.
      /// </summary>
      private readonly ApplicationDbContext _context;

      /// <summary>
      /// this object represents the hosting environment of the web application,
      /// and it is used to access information about the environment 
      /// in which the application is running,
      /// specifically to determine the path where the uploaded
      /// image file should be saved on the server.
      /// </summary>
      private readonly IWebHostEnvironment _webHostEnvironment;

      public CreateModel(
         ApplicationDbContext context,
         IWebHostEnvironment environment) {
         _context = context;
         _webHostEnvironment = environment;
      }


      /// <summary>
      /// when the interaction between the user and the page is 
      /// a GET request, this method is called to handle the 
      /// request and return the appropriate response.
      /// </summary>
      /// <returns></returns>
      public IActionResult OnGet() {
         return Page();
      }



      /// <summary>
      /// this property represents the data related to a degree,
      /// and it is decorated with the [BindProperty] attribute, which
      /// allows the data to be bound to the page's form inputs.
      /// </summary>
      [BindProperty]
      public Degree Degree { get; set; } = default!;

      [BindProperty]
      [Required(ErrorMessage = "Por favor, selecione uma imagem (PNG ou JPG).")]
      public IFormFile? ImageLogo { get; set; }



      /// <summary>
      /// when the interaction between the user and the page is a POST request,
      /// this method is called to handle the request and 
      /// add the data related with a new degree.
      /// </summary>
      /// <returns></returns>
      // For more information, see https://aka.ms/RazorPagesCRUD.
      public async Task<IActionResult> OnPostAsync() {
         /* Algorithm to save the image file to the server 
          * and set the ImageLogo property of the Degree object
          * 
          * if we have a file to upload
          *    if yes, we need to see it it is an image
          *       if yes, we need to specify the image name
          *               assign the image name to the Degree object
          *               define where to save the file
          *               save the file to the server
          *    otherwise, 
          *       send an error message to the user 
          *       indicating that the file is not an image
          */

         // ensure that you have a file
         if(ImageLogo == null || ImageLogo.Length == 0) {
            ModelState.AddModelError("ImageLogo", "Please upload an image file.");
            return Page();
         }

         // ensure that the file is an image
         if(!(ImageLogo.ContentType == "image/jpeg" ||
              ImageLogo.ContentType == "image/png")) {
            // !(A && B) = !A || !B
            ModelState.AddModelError("ImageLogo", "Only JPEG and PNG image files are allowed.");
            return Page();
         }

         // process your image file
         // define the image name
         string imageName = Guid.NewGuid().ToString() + Path.GetExtension(ImageLogo.FileName).ToLowerInvariant();
         // assign the image name to the Degree object   
         Degree.Logotype = imageName;


         if(!ModelState.IsValid) {
            return Page();
         }


         try {
            _context.Degrees.Add(Degree);
            await _context.SaveChangesAsync();

            // save the image file to the server
            string imagePath= _webHostEnvironment.WebRootPath;
            imagePath = Path.Combine(imagePath, "images");
            if(!Directory.Exists(imagePath)) {
               Directory.CreateDirectory(imagePath);
            }
            // you specify the path where to save the file
            // now, it is possible to save the file to the server
            imagePath = Path.Combine(imagePath, imageName);
            using (var stream = new FileStream(imagePath, FileMode.Create)) {
               await ImageLogo.CopyToAsync(stream);
            }

            return RedirectToPage("./Index");
         }
         catch(Exception) {
            // throw;

            // in production, you should log the exception
            // and show a user-friendly message

            ModelState.AddModelError(string.Empty, "An error occurred while creating the degree. Please try again.");
            return Page();
         }

      }
   }
}
