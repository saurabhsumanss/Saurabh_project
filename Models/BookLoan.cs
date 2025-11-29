namespace LibraryAPI.Models
{
    public class BookLoan
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int ClientId { get; set; }
        public DateTime LoanDate { get; set; } = DateTime.UtcNow;
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string Status { get; set; } = "Active"; // Active, Returned, Overdue
        public decimal? LateFee { get; set; }
        public string? Notes { get; set; }

        // Navigation properties
        public Book Book { get; set; } = null!;
        public Client Client { get; set; } = null!;
    }
}
