namespace BibliotecaPersonal.ConsoleApp.Moduls
{
    public class Author
    {
            public int AuthorId { get; set; }
            public string Name { get; set; } = string.Empty;

            // Navigation
            public List<Book> Books { get; set; } = new();
    }
}
