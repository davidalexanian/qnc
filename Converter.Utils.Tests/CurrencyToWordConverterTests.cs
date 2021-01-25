using System;
using Converter.Utils;
using Xunit;

namespace Converter.Tests
{
    public class CurrencyToWordConverterTests
    {
        [Fact]
        public void ToWords_Tests()
        {
            /* test cases from accompining PDF file
             * 0            - zero dollars
             * 1            - one dollar
             * 25.1         - twenty-five dollars and ten cents
             * 0.01         - zero dollars and one cent
             * 45100        - forty-five thousand one hundred dollars
             * 999999999.99 - nine hundred ninety-nine million nine hundred ninety-nine thousand nine hundred ninety-nine dollars and ninety-nine cents
             */

            Assert.Equal("zero dollars", CurrencyToWordConverter.ToWords(0));
            Assert.Equal("one dollar", CurrencyToWordConverter.ToWords(1));
            Assert.Equal("twenty-five dollars and ten cents", CurrencyToWordConverter.ToWords(25.1M));
            Assert.Equal("zero dollars and one cent", CurrencyToWordConverter.ToWords(0.01M));
            Assert.Equal("forty-five thousand one hundred dollars", CurrencyToWordConverter.ToWords(45100));
            Assert.Equal("nine hundred ninety-nine million nine hundred ninety-nine thousand nine hundred ninety-nine dollars and ninety-nine cents",
                CurrencyToWordConverter.ToWords(999999999.99M));


            /* edge cases
             *  fractional rounding correctness
             *  out of range exception
             *  examples not from pdf
             */
            Assert.Throws<ArgumentOutOfRangeException>(() => CurrencyToWordConverter.ToWords(-1));
            Assert.Throws<ArgumentOutOfRangeException>(() => CurrencyToWordConverter.ToWords(CurrencyToWordConverter.MAX + 1));

            Assert.Equal("zero dollars", CurrencyToWordConverter.ToWords(0.001M));
            Assert.Equal("one dollar and nineteen cents", CurrencyToWordConverter.ToWords(1.186M));
            Assert.Equal("one dollar and eighteen cents", CurrencyToWordConverter.ToWords(1.185M));
            Assert.Equal("one dollar and eighteen cents", CurrencyToWordConverter.ToWords(1.1791M));

            Assert.Equal("ten dollars", CurrencyToWordConverter.ToWords(10));
            Assert.Equal("one hundred dollars", CurrencyToWordConverter.ToWords(100));
            Assert.Equal("one thousand dollars", CurrencyToWordConverter.ToWords(1000));
            Assert.Equal("ten thousand dollars", CurrencyToWordConverter.ToWords(10000));
            Assert.Equal("one hundred thousand dollars", CurrencyToWordConverter.ToWords(100000));
            Assert.Equal("one million dollars", CurrencyToWordConverter.ToWords(1000000));
            Assert.Equal("ten million dollars", CurrencyToWordConverter.ToWords(10000000));
            Assert.Equal("one hundred million dollars", CurrencyToWordConverter.ToWords(100000000));

            Assert.Equal("nine dollars", CurrencyToWordConverter.ToWords(9));
            Assert.Equal("eleven dollars", CurrencyToWordConverter.ToWords(11));
            Assert.Equal("nineteen dollars", CurrencyToWordConverter.ToWords(19));
            Assert.Equal("ninety dollars", CurrencyToWordConverter.ToWords(90));
            Assert.Equal("ninety-nine dollars", CurrencyToWordConverter.ToWords(99));
            Assert.Equal("nine hundred ninety-nine dollars", CurrencyToWordConverter.ToWords(999));
            Assert.Equal("nine dollars and ninety cents", CurrencyToWordConverter.ToWords(9.9M));
        }

        [Fact]
        public void IsValidString_Tests()
        {
            Assert.True(CurrencyToWordConverter.IsValidNumericString("100"));
            Assert.True(CurrencyToWordConverter.IsValidNumericString("100,0"));
            Assert.True(CurrencyToWordConverter.IsValidNumericString("100,00"));
            Assert.True(CurrencyToWordConverter.IsValidNumericString("12,34"));
            Assert.True(CurrencyToWordConverter.IsValidNumericString("12,3"));
            Assert.True(CurrencyToWordConverter.IsValidNumericString("0"));
            Assert.True(CurrencyToWordConverter.IsValidNumericString("0,0"));
            Assert.True(CurrencyToWordConverter.IsValidNumericString("1,0"));
            Assert.True(CurrencyToWordConverter.IsValidNumericString("0,00"));
            Assert.True(CurrencyToWordConverter.IsValidNumericString("0,01"));
            Assert.True(CurrencyToWordConverter.IsValidNumericString("999999999,99"));

            Assert.False(CurrencyToWordConverter.IsValidNumericString("-1"));
            Assert.False(CurrencyToWordConverter.IsValidNumericString("00"));
            Assert.False(CurrencyToWordConverter.IsValidNumericString("00,1"));
            Assert.False(CurrencyToWordConverter.IsValidNumericString("1,234"));
            Assert.False(CurrencyToWordConverter.IsValidNumericString("0,123"));
            Assert.False(CurrencyToWordConverter.IsValidNumericString("0.0"));
            Assert.False(CurrencyToWordConverter.IsValidNumericString(",1"));
            Assert.False(CurrencyToWordConverter.IsValidNumericString("1,"));
            Assert.False(CurrencyToWordConverter.IsValidNumericString(".1"));
            Assert.False(CurrencyToWordConverter.IsValidNumericString("1.2"));
            Assert.False(CurrencyToWordConverter.IsValidNumericString("1,,2"));
            Assert.False(CurrencyToWordConverter.IsValidNumericString("123,2,3"));
            Assert.False(CurrencyToWordConverter.IsValidNumericString("a123"));
            Assert.False(CurrencyToWordConverter.IsValidNumericString("123,a1"));
        }
    }

}
