using Aulas.Data.Model;

using Microsoft.AspNetCore.Identity;

namespace Aulas.Data.Seed {

   internal class DbInitializer {

      internal static async void Initialize(ApplicationDbContext dbContext) {

         /*
          * https://stackoverflow.com/questions/70581816/how-to-seed-data-in-net-core-6-with-entity-framework
          * 
          * https://learn.microsoft.com/en-us/aspnet/core/data/ef-mvc/intro?view=aspnetcore-6.0#initialize-db-with-test-data
          * https://github.com/dotnet/AspNetCore.Docs/blob/main/aspnetcore/data/ef-mvc/intro/samples/5cu/Program.cs
          * https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/style-rules/ide0300
          */


         ArgumentNullException.ThrowIfNull(dbContext, nameof(dbContext));
         dbContext.Database.EnsureCreated();

         // var auxiliar
         bool haAdicao = false;



         // Se não houver Cursos, cria-os
         var cursos = Array.Empty<Degree>();
         if(!dbContext.Degrees.Any()) {
            cursos = [
               new Degree{ Name="Lic. Eng. Informática", Logotype="noImage.jpg" },
               new Degree{ Name="Lic. Eng. Eletrotécnica e de Computadores", Logotype="noImage.jpg" }
            //adicionar outros cursos
            ];
            await dbContext.Degrees.AddRangeAsync(cursos);
            haAdicao = true;
         }

         // se não houver 'roles' cria-as
         if(dbContext.Roles.Count() == 0) {
            await dbContext.Roles.AddRangeAsync(
                 new IdentityRole { Id = "prof", Name = "Professor", NormalizedName = "PROFESSOR" },
                 new IdentityRole { Id = "adm", Name = "Administrativo", NormalizedName = "ADMINISTRATIVO" }
              );
            haAdicao = true;
         }

         // Se não houver Utilizadores Identity, cria-os
         var users = Array.Empty<IdentityUser>();
         //a hasher to hash the password before seeding the user to the db
         var hasher = new PasswordHasher<IdentityUser>();

         if(!dbContext.Users.Any()) {
            var user1 = new IdentityUser {
               UserName = "joao.mendes@ipt.pt",
               NormalizedUserName = "JOAO.MENDES@IPT.PT",
               Email = "joao.mendes@ipt.pt",
               NormalizedEmail = "JOAO.MENDES@IPT.PT",
               EmailConfirmed = true,
               SecurityStamp = "5ZPZEF6SBW7IU4M344XNLT4NN5RO4GRU",
               ConcurrencyStamp = "c86d8254-dd50-44be-8561-d2d44d4bbb2f"
            };
            user1.PasswordHash = hasher.HashPassword(user1, "Aa0_aa");

            var user2 = new IdentityUser {
               UserName = "maria.sousa@ipt.pt",
               NormalizedUserName = "MARIA.SOUSA@IPT.PT",
               Email = "maria.sousa@ipt.pt",
               NormalizedEmail = "MARIA.SOUSA@IPT.PT",
               EmailConfirmed = true,
               SecurityStamp = "TW49PF6SBW7IU4M344XNLT4NN5RO4GRU",
               ConcurrencyStamp = "d8254c86-dd50-44be-8561-d2d44d4bbb2f"
            };
            user2.PasswordHash = hasher.HashPassword(user2, "Aa0_aa");

            var user3 = new IdentityUser {
               UserName = "aluno00001@ipt.pt",
               NormalizedUserName = "ALUNO00001@IPT.PT",
               Email = "aluno00001@ipt.pt",
               NormalizedEmail = "ALUNO00001@IPT.PT",
               EmailConfirmed = true,
               SecurityStamp = "TW49PF6SBW7IU4M344XNLT4NN5RO4GRU",
               ConcurrencyStamp = "d8254c86-dd50-44be-8561-d2d44d4bbb2f"
            };
            user3.PasswordHash = hasher.HashPassword(user3, "Aa0_aa");

            users = new[] { user1, user2, user3 };
            await dbContext.Users.AddRangeAsync(users);


            // associar os 'professores' à role 'Professor'
            await dbContext.UserRoles.AddRangeAsync(
                 new IdentityUserRole<string> { UserId = users[0].Id, RoleId = "prof" },
                 new IdentityUserRole<string> { UserId = users[1].Id, RoleId = "prof" }
              );

            haAdicao = true;
         }


         // Se não houver Alunos, cria-os
         var alunos = Array.Empty<Student>();
         if(!dbContext.Students.Any()) {
            alunos = [
               new Student{ Name="Mário Lopes", BirthDate=DateOnly.Parse("2000-12-15"),CellPhone="" ,
                       Degree= cursos[0], RegistrationDate=DateTime.Parse("2024-02-15"), StudentNumber=1,
                       TuitionFee=1000.00m, UserID=users[2].Id},
               new Student{ Name="Joana Gomes", BirthDate=DateOnly.Parse("2000-12-16"),CellPhone="913456789" ,
                       Degree= cursos[0], RegistrationDate=DateTime.Parse("2024-12-15"), StudentNumber=2,
                       TuitionFee=1000.00m},
               new Student{ Name="João Silva", BirthDate=DateOnly.Parse("1999-12-31"),CellPhone="92345687" ,
                       Degree= cursos[0], RegistrationDate=DateTime.Parse("2024-12-15"), StudentNumber=3,
                       TuitionFee=1000.00m },

               new Student{ Name="Maria Santos", BirthDate=DateOnly.Parse("2000-12-15"),CellPhone="9612347" ,
                       Degree= cursos[1], RegistrationDate=DateTime.Parse("2026-12-15"), StudentNumber=4,
                       TuitionFee=1100.00m  },
               new Student{ Name="Ana Costa", BirthDate=DateOnly.Parse("2000-12-15"),CellPhone="" ,
                       Degree= cursos[1], RegistrationDate=DateTime.Parse("2026-12-15"), StudentNumber=5,
                       TuitionFee=1100.00m },
           //add other users
        ];
            await dbContext.Students.AddRangeAsync(alunos);
            haAdicao = true;
         }



         // Se não houver Professores, cria-os
         var profs = Array.Empty<Professor>();
         if(!dbContext.Professors.Any()) {
            profs = [
               new Professor { Name="João Mendes", BirthDate=DateOnly.Parse("1970-04-10"), CellPhone="919876543" , UserID=users[0].Id },
               new Professor { Name="Maria Sousa", BirthDate=DateOnly.Parse("1988-09-12"), CellPhone="918076543" , UserID=users[1].Id }
              ];
            await dbContext.Professors.AddRangeAsync(profs);
            haAdicao = true;
         }



         // Se não houver UCs, cria-as
         var ucs = Array.Empty<Course>();
         if(!dbContext.Courses.Any()) {
            ucs = [
               new Course{Name="Desenvolvimento Web", CurricularYear=1, Semester=1, Degree=cursos[0], ProfessorsList=[profs[0]]},
               new Course{Name="Interfaces Web", CurricularYear=1, Semester=1, Degree=cursos[0], ProfessorsList=[profs[0],profs[1]]},
               new Course{Name="Análise de Circuitos", CurricularYear=1, Semester=1, Degree=cursos[1], ProfessorsList=[profs[1]]},
               new Course{Name="Sistemas Digitais", CurricularYear=1, Semester=2, Degree=cursos[1], ProfessorsList=[profs[1]]}
            ];
            await dbContext.Courses.AddRangeAsync(ucs);
            haAdicao = true;
         }


         try {
            if(haAdicao) {
               // tornar persistentes os dados
               dbContext.SaveChanges();
            }
         }
         catch(Exception) {

            throw;
         }
      }
   }



}
