using System;
using System.Collections.Generic;
using System.Linq;

namespace BonusCardManager.ApplicationServices.Services
{
    public static class NumberRandomizer
    {
        private const int MaxNumber = 999999;

        public static int GetUniqueNumber(IEnumerable<int> usedNumbers, int maxNumber = MaxNumber)
        {
            if(usedNumbers == null)
            {
                throw new NullReferenceException("usedNumbers cannot be null");
            }
            if (maxNumber < 0)
            {
                throw new ArgumentException("maxNumber cannot be less than zero");
            }

            var allPossibleNumbers = Enumerable.Range(1, maxNumber);
            var freeNumbers = allPossibleNumbers.Except(usedNumbers).ToList();

            if(freeNumbers.Count() == 0)
            {
                //Not really sure about the correct type of exception
                throw new ArgumentException("No free numbers in this range");
            }

            var newNumber = freeNumbers[new Random().Next(0, freeNumbers.Count())];

            return newNumber;
        }
    }
}
