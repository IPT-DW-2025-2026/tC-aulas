using System.ComponentModel.DataAnnotations;

namespace Aulas.Data.Model {

   /// <summary>
   /// Curso
   /// </summary>
   public class Degree {

      [Key] // PK
      public int Id { get; set; }

      /// <summary>
      /// nome do Curso
      /// </summary>
      [Required(ErrorMessage = "O {0} é de preenchimento obrigatório!")]
      [StringLength(100)]
      [Display(Name = "Curso")] // define o nome a aparecer no ecrã
      public string Name { get; set; } = "";

      /// <summary>
      /// nome do ficheiro que contém o logótipo do Degree
      /// </summary>
      [Display(Name = "Logótipo")] // define o nome a aparecer no ecrã
      [StringLength(50)]
      public string? Logotype { get; set; } // o '?' vai tornar o atributo em preenchimento facultativo


      /* ****************************************
       * Construção dos Relacionamentos
       * *************************************** */

      // relacionamento 1-N

      /// <summary>
      /// Lista das disciplinas do curso
      /// </summary>
      public ICollection<Course> CoursesList { get; set; } = [];

      /// <summary>
      /// lista dos alunos matriculados no curso
      /// </summary>
      public ICollection<Student> StudentsList { get; set; } = [];

   }
}
