using LibraryAPI.Data;
using LibraryAPI.DTOs;
using LibraryAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookLoansController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public BookLoansController(LibraryDbContext context)
        {
            _context = context;
        }

        // GET: api/BookLoans
        [HttpGet]
        [Authorize(Roles = "Librarian")]
        public async Task<ActionResult<IEnumerable<BookLoanDto>>> GetBookLoans()
        {
            var bookLoans = await _context.BookLoans
                .Include(bl => bl.Book)
                .Include(bl => bl.Client)
                .Select(bl => new BookLoanDto
                {
                    Id = bl.Id,
                    BookId = bl.BookId,
                    BookTitle = bl.Book.Title,
                    ClientId = bl.ClientId,
                    ClientName = $"{bl.Client.FirstName} {bl.Client.LastName}",
                    LoanDate = bl.LoanDate,
                    DueDate = bl.DueDate,
                    ReturnDate = bl.ReturnDate,
                    Status = bl.Status,
                    LateFee = bl.LateFee,
                    Notes = bl.Notes
                })
                .ToListAsync();

            return Ok(bookLoans);
        }

        // GET: api/BookLoans/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookLoanDto>> GetBookLoan(int id)
        {
            var bookLoan = await _context.BookLoans
                .Include(bl => bl.Book)
                .Include(bl => bl.Client)
                .Where(bl => bl.Id == id)
                .Select(bl => new BookLoanDto
                {
                    Id = bl.Id,
                    BookId = bl.BookId,
                    BookTitle = bl.Book.Title,
                    ClientId = bl.ClientId,
                    ClientName = $"{bl.Client.FirstName} {bl.Client.LastName}",
                    LoanDate = bl.LoanDate,
                    DueDate = bl.DueDate,
                    ReturnDate = bl.ReturnDate,
                    Status = bl.Status,
                    LateFee = bl.LateFee,
                    Notes = bl.Notes
                })
                .FirstOrDefaultAsync();

            if (bookLoan == null)
            {
                return NotFound(new { message = "Book loan not found" });
            }

            return Ok(bookLoan);
        }

        // GET: api/BookLoans/client/5
        [HttpGet("client/{clientId}")]
        public async Task<ActionResult<IEnumerable<BookLoanDto>>> GetClientLoans(int clientId)
        {
            var bookLoans = await _context.BookLoans
                .Include(bl => bl.Book)
                .Include(bl => bl.Client)
                .Where(bl => bl.ClientId == clientId)
                .Select(bl => new BookLoanDto
                {
                    Id = bl.Id,
                    BookId = bl.BookId,
                    BookTitle = bl.Book.Title,
                    ClientId = bl.ClientId,
                    ClientName = $"{bl.Client.FirstName} {bl.Client.LastName}",
                    LoanDate = bl.LoanDate,
                    DueDate = bl.DueDate,
                    ReturnDate = bl.ReturnDate,
                    Status = bl.Status,
                    LateFee = bl.LateFee,
                    Notes = bl.Notes
                })
                .ToListAsync();

            return Ok(bookLoans);
        }

        // POST: api/BookLoans
        [HttpPost]
        [Authorize(Roles = "Librarian")]
        public async Task<ActionResult<BookLoanDto>> CreateBookLoan([FromBody] CreateBookLoanDto createBookLoanDto)
        {
            // Check if book exists and has available copies
            var book = await _context.Books.FindAsync(createBookLoanDto.BookId);
            if (book == null)
            {
                return NotFound(new { message = "Book not found" });
            }

            if (book.AvailableCopies <= 0)
            {
                return BadRequest(new { message = "No available copies of this book" });
            }

            // Check if client exists and is active
            var client = await _context.Clients.FindAsync(createBookLoanDto.ClientId);
            if (client == null)
            {
                return NotFound(new { message = "Client not found" });
            }

            if (!client.IsActive)
            {
                return BadRequest(new { message = "Client account is not active" });
            }

            // Check if client already has this book on loan
            var existingLoan = await _context.BookLoans
                .AnyAsync(bl => bl.BookId == createBookLoanDto.BookId
                    && bl.ClientId == createBookLoanDto.ClientId
                    && bl.Status == "Active");

            if (existingLoan)
            {
                return BadRequest(new { message = "Client already has this book on loan" });
            }

            var bookLoan = new BookLoan
            {
                BookId = createBookLoanDto.BookId,
                ClientId = createBookLoanDto.ClientId,
                LoanDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(createBookLoanDto.LoanDurationDays),
                Status = "Active",
                Notes = createBookLoanDto.Notes
            };

            // Decrease available copies
            book.AvailableCopies--;

            _context.BookLoans.Add(bookLoan);
            await _context.SaveChangesAsync();

            var bookLoanDto = new BookLoanDto
            {
                Id = bookLoan.Id,
                BookId = bookLoan.BookId,
                BookTitle = book.Title,
                ClientId = bookLoan.ClientId,
                ClientName = $"{client.FirstName} {client.LastName}",
                LoanDate = bookLoan.LoanDate,
                DueDate = bookLoan.DueDate,
                ReturnDate = bookLoan.ReturnDate,
                Status = bookLoan.Status,
                LateFee = bookLoan.LateFee,
                Notes = bookLoan.Notes
            };

            return CreatedAtAction(nameof(GetBookLoan), new { id = bookLoan.Id }, bookLoanDto);
        }

        // POST: api/BookLoans/return
        [HttpPost("return")]
        [Authorize(Roles = "Librarian")]
        public async Task<IActionResult> ReturnBook([FromBody] ReturnBookDto returnBookDto)
        {
            var bookLoan = await _context.BookLoans
                .Include(bl => bl.Book)
                .FirstOrDefaultAsync(bl => bl.Id == returnBookDto.BookLoanId);

            if (bookLoan == null)
            {
                return NotFound(new { message = "Book loan not found" });
            }

            if (bookLoan.Status == "Returned")
            {
                return BadRequest(new { message = "Book already returned" });
            }

            bookLoan.ReturnDate = DateTime.UtcNow;
            bookLoan.Status = "Returned";

            // Calculate late fee if overdue
            if (bookLoan.ReturnDate > bookLoan.DueDate)
            {
                var daysLate = (bookLoan.ReturnDate.Value - bookLoan.DueDate).Days;
                bookLoan.LateFee = daysLate * 1.0m; // $1 per day late fee
            }

            if (!string.IsNullOrEmpty(returnBookDto.Notes))
            {
                bookLoan.Notes = bookLoan.Notes + "\nReturn notes: " + returnBookDto.Notes;
            }

            // Increase available copies
            bookLoan.Book.AvailableCopies++;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Book returned successfully",
                lateFee = bookLoan.LateFee
            });
        }

        // DELETE: api/BookLoans/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Librarian")]
        public async Task<IActionResult> DeleteBookLoan(int id)
        {
            var bookLoan = await _context.BookLoans
                .Include(bl => bl.Book)
                .FirstOrDefaultAsync(bl => bl.Id == id);

            if (bookLoan == null)
            {
                return NotFound(new { message = "Book loan not found" });
            }

            // If loan was active, restore available copies
            if (bookLoan.Status == "Active")
            {
                bookLoan.Book.AvailableCopies++;
            }

            _context.BookLoans.Remove(bookLoan);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
