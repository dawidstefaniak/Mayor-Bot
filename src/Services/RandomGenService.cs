using System;
using System.Collections.Generic;
using System.Text;

namespace MayorBot.Services
{
    public class RandomGenService
    {
        Random random = new Random();
        
        //Getting random value for 20% range
        public int GetRangeValue(double num)
        {
            int num1 =(int) (num - 0.2 * num);
            int num2 =(int) (num + 0.2 * num);
            return random.Next(num1, num2);
        }

        //Getting value from zero to number
        public int GetRandomValueFromZero(int number)
        {
            return random.Next(0, number);
        }
    }
}
