using System;

namespace OnPoint.Universal
{
    // Helper methods for strings.
    public static class StringExtensions
    {
        public static bool IsNothing(this string input, bool considerWhiteSpaceNothing = true) => considerWhiteSpaceNothing ? String.IsNullOrWhiteSpace(input) : String.IsNullOrEmpty(input);

        public static bool IsSomething(this string input, bool considerWhiteSpaceNothing = true) => !IsNothing(input, considerWhiteSpaceNothing);

        public static bool EqualsIgnoreCase(this string input, string matchString) => (input == null && matchString == null) || (input != null && String.Compare(input, matchString, true) == 0);

        public static string EverythingAfterFirst(this string input, string character)
        {
            string retVal = default;
            int index = input.IndexOf(character);
            if (index >= 0)
            {
                retVal = input.Substring(index + 1, input.Length - index - 1);
            }
            return retVal;
        }

        public static string EverythingAfterLast(this string input, string character)
        {
            string retVal = default;
            int index = input.LastIndexOf(character);
            if (index >= 0)
            {
                retVal = input.Substring(index + 1, input.Length - index - 1);
            }
            return retVal;
        }
    }
}