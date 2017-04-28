//Created by by Martin Cerny (martin1cerny@gmail.com)

using System.Collections.Generic;
using ColorCode.Common;

namespace ColorCode.Compilation.Languages
{
    public class STEP : ILanguage
    {
        public string Id
        {
            get { return LanguageId.Step; }
        }

        public string Name
        {
            get { return "STEP 21"; }
        }

        public string CssClassName
        {
            get { return "step"; }
        }

        public string FirstLinePattern
        {
            get
            {
                return null;
            }
        }

        public IList<LanguageRule> Rules
        {
            get
            {
                return new List<LanguageRule>
                           {
                               new LanguageRule(
                                   @"/\*([^*]|[\r\n]|(\*+([^*/]|[\r\n])))*\*+/",
                                   new Dictionary<int, string>
                                       {
                                           { 0, ScopeName.Comment },
                                       }),
                               new LanguageRule(
                                   @"'[^\n]*?(?<!\\)'",
                                   new Dictionary<int, string>
                                       {
                                           { 0, ScopeName.String }
                                       }),
                               new LanguageRule(
                                   @"#[0-9]+",
                                   new Dictionary<int, string>
                                       {
                                           { 0, ScopeName.Number }
                                       }),
                               new LanguageRule(
                                   @"IFC[A-Z]+",
                                   new Dictionary<int, string>
                                   {
                                       { 0, ScopeName.Type }
                                   })
                           };
            }
        }

        public bool HasAlias(string lang)
        {
            switch (lang.ToLower())
            {
                case "cs":
                case "c#":
                    return true;

                default:
                    return false;
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
