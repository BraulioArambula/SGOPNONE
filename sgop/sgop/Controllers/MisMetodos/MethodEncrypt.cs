using System;
using System.Linq;

class MethodEncrypt
{
    public string getEncrypt(string pass)
    {
        return Encrypt(Encrypt(pass));
    }

    public string getDecrypt(string passEncrypt)
    {
        return Decrypt(Decrypt(passEncrypt));
    }

    private static string Encrypt(string password)
    {
        string result = string.Empty;
        byte[] encryted =
        System.Text.Encoding.Unicode.GetBytes(password);
        result = Convert.ToBase64String(encryted);
        return result;
    }

    private static string Decrypt(string password)
    {
        try
        {
            string result = string.Empty;
            byte[] decryted =
            Convert.FromBase64String(password);
            result =
            System.Text.Encoding.Unicode.GetString(decryted, 0, decryted.ToArray().Length);
            result = System.Text.Encoding.Unicode.GetString(decryted);
            return result;
        }
        catch (Exception)
        {
            return "";
        }
    }
}