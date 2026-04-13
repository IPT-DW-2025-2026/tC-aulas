using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aulas.Data.Model {


   [PrimaryKey(nameof(StudentFK), nameof(CourseFK))]  // PK in a M-N relationship
   public class Registration {



      public DateTime RegistrationDate { get; set; }



      [ForeignKey(nameof(Student))]
     // [Key]
      public int StudentFK { get; set; }
      public Student Student { get; set; } = null!;


      [ForeignKey(nameof(Course))]
     // [Key]
      public int CourseFK { get; set; }
      public Course Course { get; set; } = null!;


   }
}
