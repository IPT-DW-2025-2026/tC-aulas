using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace Aulas.Data.Model {

   /// <summary>
   /// Classe do relacionamento entre alunos e disciplinas
   /// </summary>
   [PrimaryKey(nameof(StudentFK), nameof(CourseFK))]  // PK composta (EF Core >= 7)
   public class Registration {

      /// <summary>
      /// Data em que o aluno se inscreve na disciplina
      /// </summary>
      [Display(Name = "Data Inscrição")]
      [DataType(DataType.Date)]
      public DateTime RegistrationDate { get; set; } = DateTime.Now.Date;

      // FK para Student
      //   [Key, Column(Order = 1)] ----> válido para EF <=6
      /// <summary>
      /// FK para o aluno que se inscreve na disciplina
      /// </summary>
      [ForeignKey(nameof(Student))] // esta anotação informa a EF que o atributo 'StudentFK' é uma FK em conjunto com o atributo 'Student'
      [Display(Name = "Estudante")]
      public int StudentFK { get; set; }
      /// <summary>
      /// FK para o aluno que se inscreve na disciplina
      /// </summary>
      public Student Student { get; set; } = null!;


      // FK para Course
      //   [Key, Column(Order = 2)] ----> válido para EF <=6
      /// <summary>
      /// FK para a disciplina em que o aluno se inscreve
      /// </summary>
      [ForeignKey(nameof(Course))] // esta anotação informa a EF que o atributo 'CourseFK' é uma FK em conjunto com o atributo 'Course'
      [Display(Name = "Disciplina")]
      public int CourseFK { get; set; }
      /// <summary>
      /// FK para a disciplina em que o aluno se inscreve
      /// </summary>
      public Course Course { get; set; } = null!;



   }
}
