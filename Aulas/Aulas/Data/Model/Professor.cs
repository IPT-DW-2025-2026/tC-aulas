namespace Aulas.Data.Model {

   /// <summary>
   /// Professor que leciona Disciplinas
   /// </summary>
   public class Professor:MyUser {

      // ############################################################
      // Relacionamento
      // ############################################################
      /// <summary>
      /// Lista de disciplinas lecionadas pelo professor
      /// </summary>
      public ICollection<Course> CoursesList { get; set; } = [];

   }
}
