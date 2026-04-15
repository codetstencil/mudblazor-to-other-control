namespace Application.Common.Models
{
    public static class HashHelper
    {
        public static (byte[] hash, byte[] salt) CreatePasswordHash(string password)
        {
            using var hmac = new HMACSHA512();
            var salt = hmac.Key;
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return (hash, salt);
        }
    }
}
