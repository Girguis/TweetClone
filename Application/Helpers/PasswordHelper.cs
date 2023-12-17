using System.Security.Cryptography;
using System.Text;

namespace Application.Helpers;

public static class PasswordHelper
{
    public static string Hash(string password)
    {
        var sha512 = SHA512.Create();
        var passWithKey = "ChangeOriginal" + password + "Hash";
        var bytes = sha512.ComputeHash(Encoding.UTF8.GetBytes(passWithKey));
        return BitConverter.ToString(bytes).Replace("-", "");
    }
}
