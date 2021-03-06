using Book_TestAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Book_TestAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ILogger _loggerfile;
        DataContext db;

        public BooksController(DataContext dbcontext, ILogger<BooksController> logger, ILoggerFactory loggerFactory )
        {
            loggerFactory.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logger_bookcontroller.txt"));
            _loggerfile = loggerFactory.CreateLogger("FileLogger");

            _logger = logger;
            db = dbcontext;
        }

        // Получение списка всех книг с полным именем автора.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> Get()
        {
            //_loggerfile.LogInformation(" ==== context.Response.StatusCode {0}", HttpContext.Response.StatusCode);

            return await db.Books.ToListAsync();
        }

        // Получение книг по названию(при вводе неполного имени также должен находиться результат)
        [HttpGet("{name}")]
        public async Task<ActionResult<IEnumerable<Book>>> Get(string name)
        {
            return await db.Books.Where(x => x.Name.Contains(name)).ToListAsync();
        }

        // Поиск книг по названию или Имени, или Фамилии или Отчеству автора
        [HttpGet("{search}/{name}")]
        public async Task<ActionResult<IEnumerable<Book>>> Search(string name)
        {
            return await db.Books 
                        .Where(x => x.Name.Contains(name) || x.Author.FullName.Contains(name) )
                        .ToListAsync();
        }

        // Создание книги (название книги и автор обязательные поля)
        [HttpPost]
        public async Task<ActionResult<Book>> Post(Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Author author = db.Authors.Find(book.AuthorId);
            book.Author = author;

            db.Books.Add(book);

            await db.SaveChangesAsync();

            return Ok(book);
        }

        // Метод изменения статуса книги с любого значения на "в наличии" по идентификатору
        // книги(если книга не найдена выводит ошибку). При смене статуса в поле логин
        // пользователя должен быть записан логин пользователя, отправившего запрос.
        [HttpPut("{id}")]
        public async Task<ActionResult<Book>> Put(int id)
        {
            Book book = db.Books.Find(id);

            if (book == null)
            {
                return BadRequest();
            }

            if (!db.Books.Any(x => x.Id == book.Id))
            {
                return NotFound();
            }

            book.BookStatusId = 2;
            book.UserLogin = HttpContext.User.Identity.Name;

            await db.SaveChangesAsync();
            return Ok(book);
        }


        //Метод изменения статуса книги с "в наличии" на "продана" по идентификатору
        //книги(если книга не найдена выводит ошибку).  При смене статуса в поле
        //логин пользователя должен быть записан логин пользователя, отправившего запрос.
        [HttpPut("{sold}/{id}")]
        public async Task<ActionResult<Book>> Sold(int id)
        {
            Book book = db.Books.Find(id);

            if (book == null)
            {
                return BadRequest();
            }

            if (!db.Books.Any(x => x.Id == book.Id))
            {
                return NotFound();
            }

            book.BookStatusId = book.BookStatusId == 2 ? 1 : book.BookStatusId;
            book.UserLogin = HttpContext.User.Identity.Name;

            await db.SaveChangesAsync();
            return Ok(book);
        }


        // Удаления книги (Если не найдена книга, выводить текст ошибки "не найдена книга")
        [HttpDelete("{id}")]
        public async Task<ActionResult<Book>> Delete(int id)
        {
            Book book = db.Books.Find(id);

            if (book == null)
            {
                return NotFound("Не найдена книга");
            }

            db.Books.Remove(book);
            await db.SaveChangesAsync();
            return Ok(book);
        }

    }
}
