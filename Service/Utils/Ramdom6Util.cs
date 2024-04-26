using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Utils
{
    public static class Ramdom6Util
    {
        public static string GenerateRandomString()
        {
            // Chuỗi chứa tất cả các ký tự 
            const string chars = "123456789abcdefghijklmnopqrstuvwxyz";
            Random random = new Random();
            StringBuilder stringBuilder = new StringBuilder();

            // Tạo chuỗi ngẫu nhiên bằng cách chọn ngẫu nhiên các ký tự từ chuỗi `chars`
            for (int i = 0; i < 6; i++)
            {
                int index = random.Next(chars.Length);
                stringBuilder.Append(chars[index]);
            }

            return stringBuilder.ToString();
        }
    }
}
