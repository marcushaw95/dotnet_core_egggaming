using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace CashEntertainment.Helper
{
    public class Common
    {
        private readonly Random _random = new Random();
   
        public string GenerateRandomNumber(int length)
    {
            var characters = "abcdefghijklmnopqrstuvwxyz0123456789";
            var charactersLength = characters.Length;

            StringBuilder result = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                int a = _random.Next(charactersLength);
                result.Append(characters.ElementAt(a));
                Console.WriteLine(result);
            }

            return result.ToString();
        }
    }
}
