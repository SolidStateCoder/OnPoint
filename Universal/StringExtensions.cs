using System;

namespace OnPoint.Universal
{
    /// <summary>
    /// Helper methods for <see cref="String"/>.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Calls either <see cref="String.IsNullOrWhiteSpace"/> or <see cref="String.IsNullOrWhiteSpace"/>; fluent.
        /// </summary>
        /// <param name="input">The string to act on</param>
        /// <param name="considerWhiteSpaceNothing">True to call <see cref="String.IsNullOrWhiteSpace"/></param>
        /// <returns>True if there is no text present</returns>
        public static bool IsNothing(this string input, bool considerWhiteSpaceNothing = true) => considerWhiteSpaceNothing ? String.IsNullOrWhiteSpace(input) : String.IsNullOrEmpty(input);

        /// <summary>
        /// Calls either <see cref="String.IsNullOrWhiteSpace"/> or <see cref="String.IsNullOrWhiteSpace"/> and negates the result; fluent.
        /// </summary>
        /// <param name="input">The string to act on</param>
        /// <param name="considerWhiteSpaceNothing">True to call <see cref="String.IsNullOrWhiteSpace"/></param>
        /// <returns>True if there is no text present</returns>
        public static bool IsSomething(this string input, bool considerWhiteSpaceNothing = true) => !IsNothing(input, considerWhiteSpaceNothing);

        /// <summary>
        /// Performs a null comparison check and then calls <see cref="String.Compare"/>.
        /// </summary>
        /// <param name="input">The first <see cref="String"/> to compare</param>
        /// <param name="input2">The second <see cref="String"/> to compare</param>
        /// <returns>True if the <see cref="String"/> are both null or contain the same text</returns>
        public static bool EqualsIgnoreCase(this string input, string input2) => (input == null && input2 == null) || (input != null && String.Compare(input, input2, true) == 0);

        /// <summary>
        /// Returns all text after, but not including, the first occurrence of <paramref name="character"/>.
        /// </summary>
        /// <param name="input">The <see cref="String"/> to search</param>
        /// <param name="character">The <see cref="Char" to look for</param>
        /// <returns>All text after, but not including, the first occurrence of <paramref name="character"/></returns>
        public static string EverythingAfterFirst(this string input, string character)
        {
            string retVal = null;
            if (input != null)
            {
                retVal = string.Empty;
                int index = input.IndexOf(character);
                if (index >= 0)
                {
                    retVal = input.Substring(index + 1, input.Length - index - 1);
                }
            }
            return retVal;
        }

        /// <summary>
        /// Returns all text after, but not including, the last occurrence of <paramref name="character"/>.
        /// </summary>
        /// <param name="input">The <see cref="String"/> to search</param>
        /// <param name="character">The <see cref="Char" to look for</param>
        /// <returns>Returns all text after, but not including, the last occurrence of <paramref name="character"/></returns>
        public static string EverythingAfterLast(this string input, string character)
        {
            string retVal = null;
            if (input != null)
            {
                retVal = string.Empty;
                int index = input.LastIndexOf(character);
                if (index >= 0)
                {
                    retVal = input.Substring(index + 1, input.Length - index - 1);
                }
            }
            return retVal;
        }

        // ABC -> ABC
        // AxByCz -> Ax By Cz
        // ABCxyz -> AB Cxyz
        // xyzABC -> xyzABC
        public static string SplitAtCapitals(this string input)
        {
            string retVal = null;
            if (!input.IsNothing())
            {
                retVal = input[0].ToString();
                for (int x = 1; x < input.Length; x++) // skip the first character to avoid getting: " A"
                {
                    if (x == 1 && Char.IsLower(input[0]) && Char.IsUpper(input[x])) // eGEMS
                    {
                        retVal += input[x];
                    }
                    else if (input.Length > 0 && Char.IsUpper(input[x]) && (Char.IsLower(input[x - 1]) || Char.IsDigit(input[x - 1]))) // AxBy
                    {
                        retVal += " " + input[x];
                    }
                    else if (input.Length > x + 1 && Char.IsUpper(input[x]) && (Char.IsUpper(input[x - 1]) || Char.IsDigit(input[x - 1])) && Char.IsLower(input[x + 1])) // ABCxyz
                    {
                        retVal += " " + input[x];
                    }
                    else
                    {
                        retVal += input[x];
                    }
                }
            }
            return retVal;
        }
    }
}