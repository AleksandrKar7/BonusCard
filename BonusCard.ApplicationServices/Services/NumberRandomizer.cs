using System;
using System.Collections.Generic;
using System.Linq;

namespace BonusCardManager.ApplicationServices.Services
{
    public static class NumberRandomizer
    {
        private const int MaxNumber = 999999;

        public static int GetUniqueNumber(IEnumerable<int> uniqueArr, int maxNumber = MaxNumber)
        {
            if(uniqueArr == null)
            {
                throw new NullReferenceException("uniqueArr cannot be null");
            }
            if (maxNumber < 0)
            {
                throw new ArgumentException("maxNumber cannot be less than zero");
            }

            var allPossibleNumbers = Enumerable.Range(1, maxNumber);
            var missingItems = allPossibleNumbers.Except(uniqueArr).ToList();

            if(missingItems.Count() == 0)
            {
                //Not really sure about the correct type of exception
                throw new ArgumentException("No free numbers in this range");
            }

            var itemNumber = new Random().Next(0, missingItems.Count());

            return missingItems[itemNumber];
        }
    }
}
