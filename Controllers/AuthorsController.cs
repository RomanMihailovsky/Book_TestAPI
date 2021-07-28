using Book_TestAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Book_TestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        DataContext db;
        public AuthorsController(DataContext context)
        {
            db = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Author>>> Get()
        {
            return await db.Authors.ToListAsync();
        }

        // Получение авторов по его имени (при вводе неполного имени также должен находиться результат).
        [HttpGet("{fullname}")]
        public async Task<ActionResult<IEnumerable<Author>>> Get(string fullname)
        {
            return await db.Authors.Where(x => x.FullName.Contains(fullname)).ToListAsync();
        }

        // Создание автора (Фамилия имя и отчество обязательные поля)
        [HttpPost]
        public async Task<ActionResult<Author>> Post(Author author)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Authors.Add(author);
            await db.SaveChangesAsync();
            return Ok(author);
        }

        // Удаление автора и всех его книг (Если не найден автор, выводить текст ошибки "не найден автор")
        [HttpDelete("{id}")]
        public async Task<ActionResult<Author>> Delete(int id)
        {
            Author author = db.Authors.Find(id);

            if (author == null)
            {
                return NotFound("Не найден автор");
            }

            foreach (var book in db.Books.Where(p => p.AuthorId == id))
            {
                db.Books.Remove(book);
            }

            db.Authors.Remove(author);
            await db.SaveChangesAsync();
            return Ok(author);
        }

    }
}
