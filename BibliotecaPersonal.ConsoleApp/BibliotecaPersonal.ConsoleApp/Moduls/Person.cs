using System.ComponentModel.DataAnnotations;

namespace BibliotecaPersonal.ConsoleApp.Moduls
{
    public class Person
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public ICollection<Loan> Loans { get; set; } = new List<Loan>();
    }
}
