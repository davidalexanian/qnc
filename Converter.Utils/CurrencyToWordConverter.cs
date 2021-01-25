using System;
using System.Collections.Generic;

namespace Converter.Utils
{
    public static class CurrencyToWordConverter
    {
        public const decimal MAX = 999999999.99M;
        public const int DECIMAL_POINTS = 2;

        private static readonly Dictionary<uint, string> names = new Dictionary<uint, string>() {
            { 1, "one"},
            { 2, "two"},
            { 3, "three"},
            { 4, "four"},
            { 5, "five"},
            { 6, "six"},
            { 7, "seven"},
            { 8, "eight"},
            { 9, "nine"},

            { 11, "eleven" },
            { 12, "twelve" },
            { 13, "thirteen" },
            { 14, "fourteen" },
            { 15, "fifteen" },
            { 16, "sixteen" },
            { 17, "seventeen" },
            { 18, "eighteen" },
            { 19, "nineteen" },

            { 10, "ten" },
            { 20, "twenty" },
            { 30, "thirty" },
            { 40, "forty" },
            { 50, "fifty" },
            { 60, "sixty" },
            { 70, "seventy" },
            { 80, "eighty" },
            { 90, "ninety" },
        };

        public static string ToWords(decimal sum)
        {
            if (sum < 0 || sum > MAX) {
                throw new ArgumentOutOfRangeException($"{sum} is out of the [0, {MAX}] range.");
            }

            var wholePart = (uint)Math.Truncate(sum);
            var fractionalPart = (uint)(Math.Round(sum - wholePart, DECIMAL_POINTS) * 100);

            var result = ConvertInternal(wholePart) + (wholePart == 1 ? " dollar" : " dollars");
            if (fractionalPart != 0) {
                result += " and " + ConvertInternal(fractionalPart) + (fractionalPart == 1 ? " cent" : " cents");
            }
            return result;
        }

        private static string ConvertInternal(uint number)
        {
            var words = string.Empty;

            // numbers from [1000000,999999999] range
            var div = number / 1000000;
            if (div > 0) {
                words += (words.Length == 0 ? string.Empty : " ") + ConvertInternal(div) + " million";
                number -= div * 1000000;
            }

            // numbers from [1000,9999] range
            div = number / 1000;
            if (div > 0) {
                words += (words.Length == 0 ? string.Empty : " ") + ConvertInternal(div) + " thousand";
                number -= div * 1000;
            }

            // numbers from [100,999] range
            div = number / 100;
            if (div > 0) {
                words += (words.Length == 0 ? string.Empty : " ") + ConvertInternal(div) + " hundred";
                number -= div * 100;
            }

            // numbers from [1,99] range where naming rules r different for {1,2...9}, [11,19], {10,20...90} ranges
            if (number == 0) {
                words = words == string.Empty ? "zero" : words;
            }
            else if (names.ContainsKey(number)) {
                // names for following sequences {1,2...9}, [11,19], {10,20,..90}
                words += (words.Length == 0 ? string.Empty : " ") + names[number];
            }
            else {
                // numbers from range [21,99] without ending with 0 (e.g. 20/30/etc.)
                div = number % 10;
                words += (words.Length == 0 ? string.Empty : " ") + $"{names[number - div]}-{names[div]}";
            }

            return words;
        }

        public static bool IsValidNumericString(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) {
                return false;
            }

            // do not allow numbers like 00,1 which decimal.Parse is OK about
            if (value.StartsWith("0") && value != "0" && !value.StartsWith("0,")) {
                return false;
            }

            bool separator = false;
            for (int i = 0; i < value.Length; i++) {
                if (char.IsDigit(value[i])) {
                    continue;
                }
                if (value[i] == ',') {
                    if (separator) {
                        return false;   // more than 1
                    }
                    separator = true;

                    if (i == 0 || i == value.Length - 1) {
                        return false;   // , at the begend
                    }

                    if (value.Length - 1 - i > DECIMAL_POINTS) {
                        return false;   // more than 2 digits after separator, e.g. 1,234
                    }
                    continue;
                }
                return false;
            }

            return true;
        }

    }
}
