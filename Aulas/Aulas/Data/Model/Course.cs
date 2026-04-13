using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aulas.Data.Model {

   /// <summary>
   /// Disciplina que pertence a um curso, e é lecionada por um ou mais professores, e em que os alunos se podem inscrever
   /// </summary>
   public class Course {


      // Vamos usar a Entity Framework para a construção do Model
      // https://learn.microsoft.com/en-us/ef/


      [Key] // PK
      public int Id { get; set; }

      /// <summary>
      /// Nome da Disciplina
      /// </summary>
      [Required(ErrorMessage = "o {0} é de preenchimento obrigatório")]
      [StringLength(30)]
      [Display(Name = "Nome da Disciplina")]
      public string Name { get; set; } = "";

      /// <summary>
      /// Ano curricular da disciplina
      /// </summary>
      public int CurricularYear { get; set; }

      /// <summary>
      /// Semestre da disciplina
      /// </summary>
      public int Semester { get; set; }

      /* ****************************************
       * Construção dos Relacionamentos
       * *************************************** */

      // relacionamento 1-N

      /// <summary>
      /// FK para o curso a que a disciplina pertence
      /// </summary>
      [ForeignKey(nameof(Degree))]
      [Display(Name = "Curso")]
      public int DegreeFK { get; set; } // FK para o Degree
      /// <summary>
      /// FK para o curso a que a disciplina pertence
      /// </summary>
      [ValidateNever]
      public Degree Degree { get; set; } = null!; // FK para o Degree


      // relacionamento M-N, SEM atributos no relacionamento
      /// <summary>
      /// Lista de professores que lecionam a disciplina
      /// </summary>
      public ICollection<Professor> ProfessorsList { get; set; } = [];


      // relacionamento N-M, COM atributos no relacionamento
      /// <summary>
      /// Lista de alunos inscritos na disciplina
      /// </summary>
      public ICollection<Registration> RegistrationsList { get; set; } = [];

   }
}
