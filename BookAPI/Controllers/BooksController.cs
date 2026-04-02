using Microsoft.AspNetCore.Mvc;
using BookAPI.Data;

namespace BookAPI.Controllers
{
    //Marks as an API controller 
    [ApiController]
    //route to /Books
    [Route("[controller]")]
    public class BooksController : ControllerBase
    {
        //inject the database context
        private BookContext _context;

        public BooksController(BookContext context)
        {
            _context = context;
        }

        //retrieve books
        [HttpGet]
        public IActionResult GetBooks(int pageSize = 5, int pageNum = 1, string sortOrder = "asc")
        {
            //convert table into a queryable object
            var query = _context.Books.AsQueryable();

            //sorting
            if (sortOrder != null && sortOrder.ToLower() == "desc")
            {
                query = query.OrderByDescending(b => b.Title);
            }
            else
            {
                query = query.OrderBy(b => b.Title);
            }

            //pagination
            var books = query
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(new
            {
                Books = books,
                TotalBooks = _context.Books.Count()
            });
        }
    }
}