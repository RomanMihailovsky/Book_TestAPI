using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Book_TestAPI.Models
{
    public class Book
    {
        public int Id { get; set; }
       
        public virtual Author Author { get; set; }

        [Required(ErrorMessage = "Укажите автора книги")]
        public int AuthorId { get; set; }

        [Required(ErrorMessage = "Укажите название книги")]
        public string Name { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public virtual BookStatus BookStatus { get; set; }
        public int? BookStatusId { get; set; }
        public string UserLogin { get; set; }
        public DateTime? CreateDate { get; set; }



    }
}
