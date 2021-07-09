namespace Theater.Domain.Credentials
{
    public class Credential
    {
        public int CredentialID { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public bool ValidatePassword(string password)
        {
            return !string.IsNullOrWhiteSpace(password) && string.Equals(Password, password);
        }
    }
}
