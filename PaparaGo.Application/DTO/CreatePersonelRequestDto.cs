namespace PaparaGo.DTO
{
    public class CreatePersonelRequestDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string IBAN { get; set; } = string.Empty;
    }
}
