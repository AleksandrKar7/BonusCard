using BonusCardManager.ApplicationServices.Services;
using System;
using System.Linq;
using Xunit;

namespace BonusCardManager.ApplicationServicesTests
{
    public class NumberRandomizerTests
    {
        #region GetUniqueNumber tests

        #region Positive cases

        [Fact]
        public void GetUniqueNumber_FreeNumber4_ShouldBe4()
        {
            //Arrange
            var idsArr = new int[] { 1, 2, 3, 5, 6, 7, 8, 9, 10 };
            var maxNumber = 10;

            var expected = 4;

            //Act
            var actual = NumberRandomizer.GetUniqueNumber(idsArr, maxNumber);
            
            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetUniqueNumber_FreeNumber2Or4and6In10ItemsArray_Return2Or4Or6()
        {
            //Arrange
            var idsArr = new int[] { 1, 3, 5, 7, 8, 9, 10 };
            var maxNumber = 10;

            var expected = new int[] { 2, 4, 6 };

            //Act
            var actual = NumberRandomizer.GetUniqueNumber(idsArr, maxNumber);

            //Assert
            Assert.Contains(actual, expected);
        }

        //52 ms
        [Fact]
        public void GetUniqueNumber_FreeNumberLastItemIn999999_Return999999()
        {
            //Arrange
            var idsArr = Enumerable.Range(1, 999998);
            var maxNumber = 999999;

            var expected = 999999;

            //Act
            var actual = NumberRandomizer.GetUniqueNumber(idsArr, maxNumber);

            //Assert
            Assert.Equal(actual, expected);
        }

        #endregion Positive cases

        #region Negative cases

        [Fact]
        public void GetUniqueNumber_NullIdsArray_NullReferenceException()
        {
            //Arrange
            int[] idsArr = null;
            var maxNumber = 999999;

            //Act

            //Assert
            Assert.Throws<NullReferenceException>(() => NumberRandomizer.GetUniqueNumber(idsArr, maxNumber));
        }

        [Fact]
        public void GetUniqueNumber_NullIdsArray_CorrectExceptionMessage()
        {
            //Arrange
            int[] idsArr = null;
            var maxNumber = 999999;

            var expected = "uniqueArr cannot be null";

            //Act
            var actual = Record.Exception(() => NumberRandomizer.GetUniqueNumber(idsArr, maxNumber)).Message.Trim();

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetUniqueNumber_NegativeMaxNumber_ArgumentException()
        {
            //Arrange
            var idsArr = new int[] { 1, 2, 3, 5, 6, 7, 8, 9, 10 };
            var maxNumber = -10;

            //Act

            //Assert
            Assert.Throws<ArgumentException>(() => NumberRandomizer.GetUniqueNumber(idsArr, maxNumber));
        }

        [Fact]
        public void GetUniqueNumber_NegativeMaxNumber_CorrectExceptionMessage()
        {
            //Arrange
            var idsArr = new int[] { 1, 2, 3, 5, 6, 7, 8, 9, 10 };
            var maxNumber = -10;

            var expected = "maxNumber cannot be less than zero";

            //Act
            var actual = Record.Exception(() => NumberRandomizer.GetUniqueNumber(idsArr, maxNumber)).Message.Trim();

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetUniqueNumber_idsArrIsFull_ArgumentException()
        {
            //Arrange
            var idsArr = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var maxNumber = 10;

            //Act

            //Assert
            Assert.Throws<ArgumentException>(() => NumberRandomizer.GetUniqueNumber(idsArr, maxNumber));
        }

        [Fact]
        public void GetUniqueNumber_idsArrIsFull_CorrectExceptionMessage()
        {
            //Arrange
            var idsArr = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var maxNumber = 10;

            var expected = "No free numbers in this range";

            //Act
            var actual = Record.Exception(() => NumberRandomizer.GetUniqueNumber(idsArr, maxNumber)).Message.Trim();

            //Assert
            Assert.Equal(expected, actual);
        }

        #endregion Negative cases

        #endregion GetUniqueNumber tests
    }
}
