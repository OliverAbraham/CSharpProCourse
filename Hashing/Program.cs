using System.Security.Cryptography;
using System.Text;

namespace Hashing
{
	class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Calculating a SHA-256 hash of a string");

            string Password = "HELLO";
            byte[] Hash = Calculate_SHA256(Password);

            Console.WriteLine($"Input  = {Password}");
            Console.WriteLine($"Output = {Convert_Byte_array_to_string(Hash)}");
            Console.ReadKey();
        }

        private static byte[] Calculate_SHA256(string input)
        {
            using (SHA256 mySHA256 = SHA256.Create())
            {
                byte[] PasswordAsBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashValue = mySHA256.ComputeHash(PasswordAsBytes);
                return hashValue;
            }
        }

        private static string Convert_Byte_array_to_string(byte[] input)
        {
            string result = "";
            foreach (byte b in input)
                result += string.Format("{0:X}", b);
            return result;
        }
    }
}
