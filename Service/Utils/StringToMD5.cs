using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Service.Utils
{
    public static class StringToMD5
    {
       public static string GetMD5Hash(string input)
        {
            // Tạo một instance của lớp MD5
            using (MD5 md5 = MD5.Create())
            {
                // Chuyển đổi chuỗi đầu vào thành mảng byte
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);

                // Tính toán MD5 hash cho mảng byte
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Chuyển đổi mảng byte thành chuỗi hex
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
}
