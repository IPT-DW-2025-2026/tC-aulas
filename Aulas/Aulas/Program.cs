using Aulas.Data;
using Aulas.Data.Seed;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// configurar o Identity
// https://learn.microsoft.com/en-us/aspnet/core/security/authorization/roles?view=aspnetcore-10.0
// the use of 'AddRoles' is to be able to use the 'RoleManager' and 'UserManager' services,
// which are needed for seeding the database with initial roles and users
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
   .AddRoles<IdentityRole>()
   .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages();


// configurar o de uso de 'cookies'
builder.Services.AddSession(options => {
   options.IdleTimeout = TimeSpan.FromSeconds(120);
   options.Cookie.HttpOnly = true;
   options.Cookie.IsEssential = true;
});
builder.Services.AddDistributedMemoryCache();



var app = builder.Build();

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment()) {
   app.UseMigrationsEndPoint();
   // Invocar o seed da BD
   app.UseItToSeedSqlServer();
}
else {
   app.UseExceptionHandler("/Error");
   // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
   app.UseHsts();
}

app.UseHttpsRedirection();


// na segunda secção, adicionar para
// começar a usar, realmente, os 'cookies'
app.UseSession();



app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
