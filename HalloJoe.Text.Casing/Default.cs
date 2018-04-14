using System;
using System.Linq;
using System.Text;

namespace HalloJoe.Text.Casing
{
    [Flags] // WIP
    public enum Case
    {
        Unknown = 0,
        Lower = 1,
        Upper = 2,
        Pascal = 4,
        Camel = 8,
        Kebab = 16, 
        Rat = 32 // WIP
    }

    public static class Default
    {
        /// <summary>
        /// -
        /// </summary>
        internal const char DASH = '-';

        /// <summary>
        /// Poor mans case detector
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Case DetectCase(this string s)
        {
            // trim if posible
            s = s?.Trim();
            // escape?
            if (string.IsNullOrEmpty(s))
                return Case.Unknown;
            // pascal?
            if (char.IsUpper(s[0]) && s.HasUpper())
                return Case.Pascal;
            // camel?
            else if (char.IsLower(s[0]) && s.HasUpper())
                return Case.Camel;
            // kebab?
            else if (s.All(x => char.IsLower(x) || x.Equals(DASH)))
                return Case.Kebab;
            // o_0!
            return Case.Unknown;
        }

        /// <summary>
        /// Has any upper cased chars?
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool HasUpper(this string s) => 
            s.Any(x => char.IsUpper(x));

        /// <summary>
        /// Is string all in upper caser? 
        /// </summary>
        /// <param name="s">String context</param>
        /// <param name="allowEdges">Ignore non-letter chars</param>
        /// <returns></returns>
        public static bool IsUpper(this string s, bool allowEdges = false) =>
            s.All(x => char.IsUpper(x) || (allowEdges ? !char.IsLetter(x) : false));

        /// <summary>
        /// Has any lower cased chars?
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool HasLower(this string s) => 
            s.Any(x => char.IsLower(x));

        /// <summary>
        /// Is string all in lower case?
        /// </summary>
        /// <param name="s">String context</param>
        /// <param name="allowEdges">Ignore non letter chars</param>
        /// <returns></returns>
        public static bool IsLower(this string s, bool allowEdges = false) =>
            s.All(x => char.IsLower(x) || (allowEdges ? !char.IsLetter(x) : false));

        /// <summary>
        /// Uppercase first char in string
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string UpperFirst(this string s) => 
            char.ToUpper(s[0]) + s.Substring(1);

        /// <summary>
        /// Lowercase first char in string
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string LowerFirst(this string s) => 
            char.ToLower(s[0]) + s.Substring(1);

        /// <summary>
        /// Lookup sets of uppercase chars and convert to single uppercase char. Ex. "AnyAPI" will become "AnyApi"
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string NormalizeUppers(this string s)
        {
            // in loop this guy indicates 
            // if previous char case is upper
            var previousWasUpper = false;
            // result container
            var result = new StringBuilder();
            {
                // go!
                foreach (var c in s.ToCharArray())
                {
                    // append char as lower if prev char was upper 
                    // and current char is upper
                    if (previousWasUpper && char.IsUpper(c))
                        result.Append(char.ToLower(c));
                    else
                        // just append char, it's all good
                        result.Append(c);
                    // set previousWasUpper
                    previousWasUpper = char.IsUpper(c) ? true : false;
                }
                // return result
                return result.ToString();
            }
        }

        /// <summary>
        /// Convert string to kebab case
        /// </summary>
        /// <param name="s"></param>
        /// <param name="normalize"></param>
        /// <returns></returns>
        public static string ToKebab(this string s, bool normalize = true)
        {
            // escape?
            if (string.IsNullOrEmpty(s?.Trim())) return s;
            // should bebab be normalized?
            if (normalize) s = s.NormalizeUppers();
            // result
            var result = new StringBuilder();
            // go!
            for (var i = 0; i < s.Length; i++)
                // if current char is uppper then 
                // add dash to result and then next char as lower
                if (char.IsUpper(s[i]))
                {
                    result.Append(DASH);
                    result.Append(char.ToLower(s[i]));
                }
                else
                // just add current char
                    result.Append(s[i]);
            // take care of  leading dashing and return result
            return result[0].Equals(DASH) ? result.Remove(0, 1).ToString() : result.ToString();
        }

        /// <summary>
        /// Convert string to pascal case
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToPascal(this string s)
        {
            // escape?
            if (string.IsNullOrEmpty(s?.Trim())) return s;
            // detect case
            var detectedCase = s.DetectCase();
            // is pascal?
            if (detectedCase.Equals(Case.Pascal))
                return s;
            // is camel?
            else if (detectedCase.Equals(Case.Camel))
                return s.UpperFirst();
            // is kebab?
            else if (detectedCase.Equals(Case.Kebab))
            {
                // result
                var result = new StringBuilder();
                // go!
                for (var i = 0; i < s.Length; i++)
                    // if is dash and length of s is greater than 
                    // current i + 1(next), then add next as upper
                    if (s[i].Equals(DASH) && s.Length >= i + 1)
                    {
                        // add next as upper
                        result.Append(char.ToUpper(s[i + 1]));
                        // increment i
                        i++;
                    }
                    else
                        // just add char
                        result.Append(s[i]);
                // ensure upper first and return result
                return result.ToString().UpperFirst();
            }
            return s;
        }

        /// <summary>
        /// Convert string to camel case
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToCamel(this string s) => 
            s.ToPascal().LowerFirst();
    }
}

