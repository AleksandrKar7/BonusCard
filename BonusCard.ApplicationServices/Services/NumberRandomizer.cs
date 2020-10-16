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
            var allPossibleNumbers = Enumerable.Range(1, maxNumber);
            var missingItems = allPossibleNumbers.Except(uniqueArr).ToList();

            var itemNumber = new Random().Next(0, missingItems.Count());

            return missingItems[itemNumber];
        }
    }
}
