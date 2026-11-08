namespace InvoiceTracker.Application.DTOs
{
    public class RegisterUserResponse
    {
        public bool Success { get; set; }   
        public string FirstName { get; set; } = null!;    
        public string LastName { get; set; } = null!;            
        public string Email { get; set; } = null!;
        public string Message { get; set; } = string.Empty;

    }
}
