using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca_personal.Model
{
    internal class Author
    {
        public int AuthorId { get; set; }
        public string Name { get; set; } = string.Empty;

        // Navigation
        public List<Book> Books { get; set; } = new();
    }
}
