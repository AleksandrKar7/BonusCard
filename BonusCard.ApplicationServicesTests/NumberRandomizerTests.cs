using BonusCardManager.ApplicationServices.Services;
using System.Linq;
using Xunit;

namespace BonusCardManager.ApplicationServicesTests
{
    public class NumberRandomizerTests
    {
        [Fact]
        public void GetUniqueNumber_FreeNumber4_ShouldBe4()
        {
            //Arrange
            var idsArr = new int[] { 1, 2, 3, 5, 6, 7, 8, 9, 10 };
            var expected = 4;

            //Act
            var actual = NumberRandomizer.GetUniqueNumber(idsArr, 10);
            
            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetUniqueNumber_FreeNumber2Or4and6In10ItemsArray_Return2Or4Or6()
        {
            //Arrange
            var idsArr = new int[] { 1, 3, 5, 7, 8, 9, 10 };
            var expected = new int[] { 2, 4, 6 };

            //Act
            var actual = NumberRandomizer.GetUniqueNumber(idsArr, 10);

            //Assert
            Assert.Contains(actual, expected);
        }

        //52 ms
        [Fact]
        public void GetUniqueNumber_FreeNumberLastItemIn999999_Return999999()
        {
            //Arrange
            var idsArr = Enumerable.Range(1, 999998);
            var expected = 999999;

            //Act
            var actual = NumberRandomizer.GetUniqueNumber(idsArr, 999999);

            //Assert
            Assert.Equal(actual, expected);
        }
    }
}
