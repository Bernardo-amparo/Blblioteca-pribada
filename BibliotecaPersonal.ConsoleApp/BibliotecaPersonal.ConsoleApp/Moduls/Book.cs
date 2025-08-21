namespace BibliotecaPersonal.ConsoleApp.Moduls
{
    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; } = string.Empty;

        // Relations
        public int AuthorId { get; set; }
        public Author? Author { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        // State
        public bool IsLent { get; set; } = false;

        // History
        public List<Loan> Loans { get; set; } = new();
    }
}
