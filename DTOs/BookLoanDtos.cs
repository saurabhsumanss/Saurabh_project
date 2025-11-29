namespace LibraryAPI.DTOs
{
    public class BookLoanDto
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string BookTitle { get; set; } = string.Empty;
        public int ClientId { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public DateTime LoanDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal? LateFee { get; set; }
        public string? Notes { get; set; }
    }

    public class CreateBookLoanDto
    {
        public int BookId { get; set; }
        public int ClientId { get; set; }
        public int LoanDurationDays { get; set; } = 14; // Default 2 weeks
        public string? Notes { get; set; }
    }

    public class ReturnBookDto
    {
        public int BookLoanId { get; set; }
        public string? Notes { get; set; }
    }
}
