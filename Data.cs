using Book_TestAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Book_TestAPI
{
    public static class Data
    {
        public static void Initialize(DataContext context)
        {
            Author mbulgakov = new Author { Name = "Михаил", SurName = "Булгаков", FullName = "Михаил Афанасьевич Булгаков", BirthDate = new DateTime(1891, 5, 15) };
            Author apushkin = new Author { Name = "Александр", SurName = "Пушкин", FullName = "Александр Сергеевич Пушкин", BirthDate = new DateTime(1799, 6, 6) };

            if (!context.Authors.Any())
            {
                context.Authors.Add(mbulgakov);
                context.Authors.Add(apushkin);
                context.SaveChanges();
            }

            if (!context.Books.Any())
            {
                context.Books.Add(new Book { Name = "Мастер и маргарита", Author = mbulgakov, ReleaseDate = new DateTime(2000, 10, 30), CreateDate = DateTime.Now });
                context.Books.Add(new Book { Name = "Сказка о рыбаке и рыбке", Author = apushkin, ReleaseDate = new DateTime(2000, 10, 30), CreateDate = DateTime.Now });
                context.SaveChanges();
            }

            if (!context.BookStatuses.Any())
            {
                context.BookStatuses.Add(new BookStatus { Name = "Продана" });
                context.BookStatuses.Add(new BookStatus { Name = "В наличии" });
                context.BookStatuses.Add(new BookStatus { Name = "Неизвестно" });
                context.SaveChanges();
            }
        }
    }
}
