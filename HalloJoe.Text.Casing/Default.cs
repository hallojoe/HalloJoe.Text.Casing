using System.Linq;
using System.Text;

namespace HalloJoe.Text.Casing
{
    public enum CaseTypes
    {
        Unknown = 0,
        Pascal = 1,
        Camel = 2,
        Kebab = 4, 
    }

    public static class Default
    {
        /// <summary>
        /// A dash -
        /// </summary>
        internal const char Dash = '-';

        /// <summary>
        /// Poor mans case detector
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static CaseTypes DetectCase(this string s)
        {
            // trim
            s = s?.Trim();
            // escape?
            if (string.IsNullOrEmpty(s)) return CaseTypes.Unknown;
            // pascal?
            if (char.IsUpper(s[0]) && s.HasUpper()) return CaseTypes.Pascal;
            // camel?
            else if (char.IsLower(s[0]) && s.HasUpper()) return CaseTypes.Camel;
            // kebab?
            else if (s.All(x => char.IsLower(x) || x.Equals(Dash))) return CaseTypes.Kebab;
            // o_0!
            return CaseTypes.Unknown;
        }

        /// <summary>
        /// Has any upper cased chars?
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool HasUpper(this string s) => s.Any(char.IsUpper);

        /// <summary>
        /// Is string all in upper caser? 
        /// </summary>
        /// <param name="s">String context</param>
        /// <param name="allowEdges">Ignore non-letter chars</param>
        /// <returns></returns>
        public static bool IsUpper(this string s, bool allowEdges = false) => s.All(x => char.IsUpper(x) || (allowEdges && !char.IsLetter(x)));

        /// <summary>
        /// Has any lower cased chars?
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool HasLower(this string s) => s.Any(char.IsLower);

        /// <summary>
        /// Is string all in lower case?
        /// </summary>
        /// <param name="s">String context</param>
        /// <param name="allowEdges">Ignore non letter chars</param>
        /// <returns></returns>
        public static bool IsLower(this string s, bool allowEdges = false) => s.All(x => char.IsLower(x) || (allowEdges && !char.IsLetter(x)));

        /// <summary>
        /// Uppercase first char in string
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string UpperFirst(this string s) => char.ToUpper(s[0]) + s.Substring(1);

        /// <summary>
        /// Lowercase first char in string
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string LowerFirst(this string s) => char.ToLower(s[0]) + s.Substring(1);

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
                foreach (var c in s)
                {
                    // append char as lower if prev char was upper and current char is upper
                    if (previousWasUpper && char.IsUpper(c)) result.Append(char.ToLower(c));
                    else result.Append(c); // just append char, it's all good
                    // set previousWasUpper
                    previousWasUpper = char.IsUpper(c);
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
            // should kebab be normalized?
            if (normalize) s = s.NormalizeUppers();
            // result
            var result = new StringBuilder();
            // go!
            foreach (var t in s)
            {
                if (char.IsUpper(t))
                {
                    result.Append(Dash);
                    result.Append(char.ToLower(t));
                }
                else result.Append(t); // just add current char
            }
            // take care of leading dash and return result
            return result[0].Equals(Dash) ? result.Remove(0, 1).ToString() : result.ToString();
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
            if (detectedCase.Equals(CaseTypes.Pascal)) return s;

            // is camel?
            if (detectedCase.Equals(CaseTypes.Camel)) return s.UpperFirst();
            
            // is kebab?
            if (detectedCase.Equals(CaseTypes.Kebab))
            {
                // result
                var result = new StringBuilder();
                // go!
                for (var i = 0; i < s.Length; i++)
                {
                    // if is dash and length of s is greater than current i + 1(next), then add next as upper
                    if (s[i].Equals(Dash) && s.Length >= i + 1)
                    {                        
                        result.Append(char.ToUpper(s[i + 1])); // add next as upper                        
                        i++;
                    }
                    else result.Append(s[i]); // just add char
                }
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
        public static string ToCamel(this string s) => s.ToPascal().LowerFirst();
    }
}

