using Book_TestAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Book_TestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        DataContext db;
        private readonly ILogger _loggerfile;

        public AuthorsController(DataContext context, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logger_bookcontroller.txt"));
            _loggerfile = loggerFactory.CreateLogger("FileLogger");

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
            //_loggerfile.LogInformation(" ==== AuthorsController context.Response.StatusCode {0}", HttpContext.Response.StatusCode);

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
