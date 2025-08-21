namespace BibliotecaPersonal.ConsoleApp.Moduls
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;

        // Navigation
        public List<Book> Books { get; set; } = new();
    }
}
