using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aulas.Data.Model {

   /// <summary>
   /// classe que herda todas as características do MyUser, 
   /// ou seja, tem Id, Nome, Email, etc. 
   /// E, pode ter outras características específicas de um estudante, 
   /// como por exemplo, a matrícula, o curso que está fazendo, etc.
   /// </summary>
   public class Student:MyUser {


      /// <summary>
      /// Número atribuído a cada estudante, para o identificar de forma única
      /// </summary>
      public int StudentNumber { get; set; }

      /// <summary>
      /// Atributo auxiliar para recolher os dados da propina,
      /// para depois ser convertido para decimal e 
      /// guardado no atributo 'TuitionFee' do Student,
      /// </summary>
      [Display(Name = "Propina")]
      [Required(ErrorMessage = "O {0} é de preenchimento obrigatório.")]
      [NotMapped] // informa a EF para não criar um atributo na BD para este atributo
      [RegularExpression("[0-9]{1,7}([.,][0-9]{1,2})?", 
             ErrorMessage = "O {0} deve ser um número decimal com até 2 casas decimais.")]
      [StringLength(10)]
      public string TuitionFeeAux { get; set; }="";


      /// <summary>
      /// Propina paga pelo Student aquando da matrícula no Degree
      /// </summary>
      [Precision(9, 2)] // informa a EF para criar o atributo com 9 dígitoe e 2, como parte decimal
      public decimal TuitionFee { get; set; }

      /// <summary>
      /// Data de matrícula do aluno
      /// </summary>
      [Display(Name = "Data Matrícula")]
      [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
      [DataType(DataType.Date)]
      public DateTime RegistrationDate { get; set; } = DateTime.Now;


      /* ****************************************
      * Construção dos Relacionamentos
      * *************************************** */

      // relacionamento 1-N


      /// <summary>
      /// FK para o Degree
      /// </summary>
      [ForeignKey(nameof(Degree))] // esta anotação informa a EF que o atributo 'DegreeFK' é uma FK em conjunto com o atributo 'Degree'
      [Display(Name = "Curso")]
      public int DegreeFK { get; set; } // FK para o Degree
      [ValidateNever] // informa a EF para não validar este atributo
      public Degree Degree { get; set; } = null!; // FK para o Degree



      // relacionamento N-M, com atributos no relacionamento
      /// <summary>
      /// Lista de UCs em que o aluno está inscrito
      /// </summary>
      public ICollection<Registration> RegistrationsList { get; set; } = [];
   }
}
