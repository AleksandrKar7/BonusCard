using System;
using System.Collections.Generic;
using System.Linq;

namespace BonusCardManager.ApplicationServices.Services
{
    public static class NumberRandomizer
    {
        private const int MaxNumber = 999999;

        public static int GetUniqueNumber(IEnumerable<int> usedNumbers, int maxNumber = MaxNumber, int minNumber = 1)
        {
            if(usedNumbers == null)
            {
                throw new NullReferenceException("usedNumbers cannot be null");
            }
            if (maxNumber < 0)
            {
                throw new ArgumentException("maxNumber cannot be less than zero");
            }

            var allPossibleNumbers = Enumerable.Range(minNumber, maxNumber);
            var freeNumbers = allPossibleNumbers.Except(usedNumbers).ToList();

            if(freeNumbers.Count() == 0)
            {
                throw new ArgumentException("No free numbers in this range");
            }

            var newNumber = freeNumbers[new Random().Next(0, freeNumbers.Count())];

            return newNumber;
        }
    }
}
