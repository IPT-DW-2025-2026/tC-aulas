// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Aulas.Data;
using Aulas.Data.Model;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;

namespace Aulas.Areas.Identity.Pages.Account {
   public class RegisterModel:PageModel {
      private readonly SignInManager<IdentityUser> _signInManager;
      private readonly UserManager<IdentityUser> _userManager;
      private readonly IUserStore<IdentityUser> _userStore;
      private readonly IUserEmailStore<IdentityUser> _emailStore;
      private readonly ILogger<RegisterModel> _logger;
      private readonly IEmailSender _emailSender;

      private readonly ApplicationDbContext _context;

      public RegisterModel(
          UserManager<IdentityUser> userManager,
          IUserStore<IdentityUser> userStore,
          SignInManager<IdentityUser> signInManager,
          ILogger<RegisterModel> logger,
          IEmailSender emailSender,
          ApplicationDbContext context) {
         _userManager = userManager;
         _userStore = userStore;
         _emailStore = GetEmailStore();
         _signInManager = signInManager;
         _logger = logger;
         _emailSender = emailSender;
         _context = context;
      }

      /// <summary>
      /// object to colect data from the form and save into the database
      /// </summary>
      [BindProperty]
      public InputModel Input { get; set; }

      /// <summary>
      ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
      ///     directly from your code. This API may change or be removed in future releases.
      /// </summary>
      public string ReturnUrl { get; set; }

      /// <summary>
      ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
      ///     directly from your code. This API may change or be removed in future releases.
      /// </summary>
      public IList<AuthenticationScheme> ExternalLogins { get; set; }

      /// <summary>
      /// class that defines the data you want to collect from the form 
      /// and save into the database
      /// </summary>
      public class InputModel {
         /// <summary>
         ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
         ///     directly from your code. This API may change or be removed in future releases.
         /// </summary>
         [Required(ErrorMessage = "o {0} é obrigatório")]
         [EmailAddress(ErrorMessage = "Por favor, escreva um {0} válido")]
         [Display(Name = "Email")]
         [RegularExpression(@"[a-zA-Z0-9._-]+@ipt\.pt",
                ErrorMessage = "O email deve ser do domínio IPT")]
         public string Email { get; set; }

         /// <summary>
         ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
         ///     directly from your code. This API may change or be removed in future releases.
         /// </summary>
         [Required(ErrorMessage = "o {0} é obrigatório")]
         [StringLength(100, ErrorMessage = "A {0} deve ter  entre {1} e {2} caracteres.", MinimumLength = 6)]
         [DataType(DataType.Password)]
         [Display(Name = "Password")]
         public string Password { get; set; }

         /// <summary>
         ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
         ///     directly from your code. This API may change or be removed in future releases.
         /// </summary>
         [DataType(DataType.Password)]
         [Display(Name = "Confirmar password")]
         [Compare("Password", ErrorMessage = "A password e a sua confirmação não coincidem.")]
         public string ConfirmPassword { get; set; }

         // ++++++++++++++++++++++++++++++++++++++++++++++++
         // the adition of Student attributo offers you the possibility
         // to save data related with Students
         // ++++++++++++++++++++++++++++++++++++++++++++++++

         public Student Student { get; set; }

         // ++++++++++++++++++++++++++++++++++++++++++++++++

      } // end of InputModel class




      public async Task OnGetAsync(string returnUrl = null) {
         ReturnUrl = returnUrl;

         // list of options to appear in the dropdown list for the Degree property
         ViewData["DegreeFK"] = new SelectList(_context.Degrees.OrderBy(d => d.Name), "Id", "Name");

      }




      public async Task<IActionResult> OnPostAsync(string returnUrl = null) {
         returnUrl ??= Url.Content("~/");

         if(ModelState.IsValid) {
            var user = CreateUser();

            await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
            await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
            // save the users' data on the database
            var result = await _userManager.CreateAsync(user, Input.Password);

            if(result.Succeeded) {
               _logger.LogInformation("User created a new account with password.");

               // you have success in creating the user,
               // but you also want to save the data related with the Student

               // Convert the TuitionFeeAux string to a decimal
               // and assign it to the TuitionFee property
               Input.Student.TuitionFee = Convert.ToDecimal(Input.Student.TuitionFeeAux.Replace('.', ','),
                                                      new CultureInfo("pt-PT"));

               // we need to decide how to assign the StudentNumber to this new user

               // add the user's ID to the new 'student'
               var userId = await _userManager.GetUserIdAsync(user);
               Input.Student.UserID = userId;

              // save the student data on the database
               try {
                  _context.Students.Add(Input.Student);
                  await _context.SaveChangesAsync();
               }
               catch(Exception) {
                  // do not forget that YOU MUST handle the exception
                  // in a way that is appropriate for your application
                  // DO NOT USE a 'throw' in a production application,
                  // because it will present to many details about the exception to the user,
                  // which IS a security risk.

                  // if you arrive here, it means that the user was created successfully,
                  // but there was an error when you tried to save the student data on the database.
                  // What we need to do?

                  /*
                   * 1. You must delete the user that was created, 
                   *    because it is not useful without the student data.
                   * 2. You must show an error message to the user, 
                   *    saying that there was an error when saving the student data, 
                   *    and that they should try to register again.
                   * 3. You must log the error, so you can analyze it later and fix it.
                   */

                  // throw;
               }






               // send notification message to user
               var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
               code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
               var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

               await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                   $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

               if(_userManager.Options.SignIn.RequireConfirmedAccount) {
                  return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
               }
               else {
                  await _signInManager.SignInAsync(user, isPersistent: false);
                  return LocalRedirect(returnUrl);
               }
            }
            foreach(var error in result.Errors) {
               ModelState.AddModelError(string.Empty, error.Description);
            }
         }

         // select the list of degrees to appear
         // in the dropdown list for the Degree property
         ViewData["DegreeFK"] = new SelectList(_context.Degrees.OrderBy(d => d.Name), "Id", "Name");

         // If we got this far, something failed, redisplay form
         return Page();
      }

      private IdentityUser CreateUser() {
         try {
            return Activator.CreateInstance<IdentityUser>();
         }
         catch {
            throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
         }
      }

      private IUserEmailStore<IdentityUser> GetEmailStore() {
         if(!_userManager.SupportsUserEmail) {
            throw new NotSupportedException("The default UI requires a user store with email support.");
         }
         return (IUserEmailStore<IdentityUser>)_userStore;
      }
   }
}
