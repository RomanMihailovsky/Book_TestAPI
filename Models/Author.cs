using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Book_TestAPI.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }

        [Required(ErrorMessage = "Укажите полное имя")]
        public string FullName { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}
