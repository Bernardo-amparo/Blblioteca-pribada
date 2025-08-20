using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca_personal.Model
{
    internal class Book
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
