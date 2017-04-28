using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xbim.SiteBuilder
{
    public static class Extensions
    {
        /// <summary>
        /// Produces optional, URL-friendly version of a title, "like-this-one". 
        /// hand-tuned for speed, reflects performance refactoring contributed
        /// by John Gietzen (user otac0n) 
        /// Original code by Jeff Atwood from: http://stackoverflow.com/questions/25259/how-does-stack-overflow-generate-its-seo-friendly-urls/25486#25486
        /// </summary>
        public static string URLFriendly(this string title)
        {
            if (title == null) return "";

            const int maxlen = 80;
            int len = title.Length;
            bool prevdash = false;
            var sb = new StringBuilder(len);
            char c;

            for (int i = 0; i < len; i++)
            {
                c = title[i];
                if ((c >= 'a' && c <= 'z') || (c >= '0' && c <= '9'))
                {
                    sb.Append(c);
                    prevdash = false;
                }
                else if (c >= 'A' && c <= 'Z')
                {
                    // tricky way to convert to lowercase
                    sb.Append((char)(c | 32));
                    prevdash = false;
                }
                else if (c == ' ' || c == ',' || c == '.' || c == '/' ||
                    c == '\\' || c == '-' || c == '_' || c == '=')
                {
                    if (!prevdash && sb.Length > 0)
                    {
                        sb.Append('-');
                        prevdash = true;
                    }
                }
                else if ((int)c >= 128)
                {
                    int prevlen = sb.Length;
                    sb.Append(RemapInternationalCharToAscii(c));
                    if (prevlen != sb.Length) prevdash = false;
                }
                if (i == maxlen) break;
            }

            if (prevdash)
                return sb.ToString().Substring(0, sb.Length - 1);
            else
                return sb.ToString();
        }

        /// <summary>
        /// Original code by Jeff Atwood from http://meta.stackexchange.com/questions/7435/non-us-ascii-characters-dropped-from-full-profile-url/7696#7696
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private static string RemapInternationalCharToAscii(char c)
        {
            string s = c.ToString().ToLowerInvariant();
            if ("àåáâäãåą".Contains(s))
            {
                return "a";
            }
            else if ("çćčĉ".Contains(s))
            {
                return "c";
            }
            else if ("đď".Contains(c))
            {
                return "d";
            }
            else if ("èéêëęě".Contains(s))
            {
                return "e";
            }
            else if ("ğĝ".Contains(s))
            {
                return "g";
            }
            else if (c == 'ĥ')
            {
                return "h";
            }
            else if ("ìíîïı".Contains(s))
            {
                return "i";
            }
            else if (c == 'ĵ')
            {
                return "j";
            }
            else if (c == 'ł')
            {
                return "l";
            }
            else if ("ñńň".Contains(s))
            {
                return "n";
            }
            else if ("òóôõöøőð".Contains(s))
            {
                return "o";
            }
            else if (c == 'ř')
            {
                return "r";
            }
            else if ("śşšŝ".Contains(s))
            {
                return "s";
            }
            else if (c == 'ß')
            {
                return "ss";
            }
            else if (c == 'ť')
            {
                return "t";
            }
            else if (c == 'Þ')
            {
                return "th";
            }
            else if ("ùúûüŭů".Contains(s))
            {
                return "u";
            }
            else if ("ýÿ".Contains(s))
            {
                return "y";
            }
            else if ("żźž".Contains(s))
            {
                return "z";
            }
            else
            {
                return "";
            }
        }
    }
}
