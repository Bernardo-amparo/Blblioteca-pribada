using BibliotecaPersonal.ConsoleApp.Moduls;
using Microsoft.EntityFrameworkCore;
using BibliotecaPersonal.ConsoleApp.Data;
using System;
using System.Linq;
using static BibliotecaPersonal.ConsoleApp.Data.PersonalLibrary;

using var db = new LibraryContext ();

bool exit = false;
while (!exit)
{
    Console.WriteLine("\n📚 Sistema de Gestión de Biblioteca Personal");
    Console.WriteLine("1. Agregar libro");
    Console.WriteLine("2. Listar libros");
    Console.WriteLine("3. Buscar libro");
    Console.WriteLine("4. Prestar libro");
    Console.WriteLine("5. Devolver libro");
    Console.WriteLine("6. Ver historial de préstamos");
    Console.WriteLine("0. Salir");
    Console.Write("Opción: ");

    switch (Console.ReadLine())
    {
        case "1": AddBook(db); break;
        case "2": ListBooks(db); break;
        case "3": SearchBooks(db); break;
        case "4": LendBook(db); break;
        case "5": ReturnBook(db); break;
        case "6": ViewLoanHistory(db); break;
        case "0": exit = true; break;
        default: Console.WriteLine("Opción no válida."); break;
    }
}

// ==================== METHODS ====================

static void AddBook(LibraryContext db)
{
    Console.Write("Título: ");
    string title = Console.ReadLine() ?? string.Empty;

    Console.Write("Autor: ");
    string authorName = Console.ReadLine() ?? string.Empty;

    Console.Write("Categoría: ");
    string categoryName = Console.ReadLine() ?? string.Empty;

    var author = db.Authors.FirstOrDefault(a => a.Name == authorName);
    if (author is null)
    {
        author = new Author { Name = authorName };
        db.Authors.Add(author);
    }

    var category = db.Categories.FirstOrDefault(c => c.Name == categoryName);
    if (category is null)
    {
        category = new Category { Name = categoryName };
        db.Categories.Add(category);
    }

    var book = new Book
    {
        Title = title,
        Author = author,
        Category = category
    };

    db.Books.Add(book);
    db.SaveChanges();

    Console.WriteLine("Libro agregado correctamente.");
}

static void ListBooks(LibraryContext db)
{
    var books = db.Books
        .Include(b => b.Author)
        .Include(b => b.Category)
        .OrderBy(b => b.Title)
        .ToList();

    if (!books.Any())
    {
        Console.WriteLine("No hay libros registrados.");
        return;
    }

    foreach (var b in books)
    {
        Console.WriteLine($"{b.BookId}. {b.Title} - {b.Author?.Name} ({b.Category?.Name}) {(b.IsLent ? "[Prestado]" : "")}");
    }
}

static void SearchBooks(LibraryContext db)
{
    Console.Write("Buscar por título: ");
    string query = (Console.ReadLine() ?? string.Empty).Trim();

    if (string.IsNullOrWhiteSpace(query))
    {
        Console.WriteLine("Búsqueda vacía.");
        return;
    }

    var books = db.Books
        .Include(b => b.Author)
        .Include(b => b.Category)
        .Where(b => EF.Functions.Like(b.Title, $"%{query}%"))
        .OrderBy(b => b.Title)
        .ToList();

    if (!books.Any())
    {
        Console.WriteLine("No se encontraron resultados.");
        return;
    }

    foreach (var b in books)
    {
        Console.WriteLine($"{b.BookId}. {b.Title} - {b.Author?.Name} ({b.Category?.Name}) {(b.IsLent ? "[Prestado]" : "")}");
    }
}

static void LendBook(LibraryContext db)
{
    Console.Write("ID del libro a prestar: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine("ID inválido.");
        return;
    }

    var book = db.Books.Include(b => b.Loans).FirstOrDefault(b => b.BookId == id);
    if (book is null)
    {
        Console.WriteLine("Libro no encontrado.");
        return;
    }

    if (book.IsLent)
    {
        Console.WriteLine("El libro ya está prestado.");
        return;
    }

    book.IsLent = true;
    db.Loans.Add(new Loan
    {
        Book = book,
        LoanDate = DateTime.Now
    });

    db.SaveChanges();
    Console.WriteLine("Libro prestado.");
}

static void ReturnBook(LibraryContext db)
{
    Console.Write("ID del libro a devolver: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine("ID inválido.");
        return;
    }

    var book = db.Books.FirstOrDefault(b => b.BookId == id);
    if (book is null)
    {
        Console.WriteLine("Libro no encontrado.");
        return;
    }

    if (!book.IsLent)
    {
        Console.WriteLine("El libro no está prestado.");
        return;
    }

    book.IsLent = false;

    var loan = db.Loans
        .Where(l => l.BookId == id && l.ReturnDate == null)
        .OrderByDescending(l => l.LoanDate)
        .FirstOrDefault();

    if (loan is not null)
    {
        loan.ReturnDate = DateTime.Now;
    }

    db.SaveChanges();
    Console.WriteLine("Libro devuelto.");
}

static void ViewLoanHistory(LibraryContext db)
{
    var history = db.Loans
        .Include(l => l.Book)
        .OrderByDescending(l => l.LoanDate)
        .ToList();

    if (!history.Any())
    {
        Console.WriteLine("No hay préstamos registrados.");
        return;
    }

    foreach (var h in history)
    {
        var title = h.Book?.Title ?? "(desconocido)";
        var loanDate = h.LoanDate.ToString("yyyy-MM-dd HH:mm");
        var returnDate = h.ReturnDate?.ToString("yyyy-MM-dd HH:mm") ?? "No devuelto";
        Console.WriteLine($"{h.LoanId}. {title} - Prestado: {loanDate} - Devuelto: {returnDate}");
    }
}
