using System;
using System.Collections.Generic;
using WebApplication2.Models;

namespace WebApplication2
{
    public static class CodeGenerator
    {
        private static readonly Random random = new Random();
        private static readonly string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public static string GenerateUniqueCode(Dictionary<string, Room>? rooms)
        {
            if (rooms  == null) throw new NotImplementedException();
            string code;

            do
            {
                code = GenerateCode();
            } while (rooms.ContainsKey(code));

            return code;
        }

        private static string GenerateCode()
        {
            char[] codeArray = new char[4];

            for (int i = 0; i < 4; i++)
            {
                codeArray[i] = chars[random.Next(chars.Length)];
            }

            return new string(codeArray);
        }
    }
}
