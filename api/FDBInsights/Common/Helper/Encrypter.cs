using System.Security.Cryptography;
using System.Text;

namespace FDBInsights.Common.Helper;

public static class Encrypter
{
    internal static string OneWayEncrypter(string textToBeEncrypted)
    {
        if (string.IsNullOrEmpty(textToBeEncrypted))
            return string.Empty;
        var hash = MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(textToBeEncrypted));
        var stringBuilder = new StringBuilder();
        for (var index = 0; index < hash.Length; ++index)
            stringBuilder.Append(hash[index].ToString("X2"));
        return stringBuilder.ToString();
    }
}