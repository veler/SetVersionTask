using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace SetVersionTask
{
    public class CSharpVersionUpdateRule
    {
        private VersionUpdateRule updateRule;
        public CSharpVersionUpdateRule(string attributeName, string updateRule)
        {
            this.AttributeName = attributeName;
            this.updateRule = new VersionUpdateRule(updateRule);
        }
        public string AttributeName { get; private set; }
        public string Update(VersionString v) { return this.updateRule.Update(v); }
    }

    public class CSharpUpdater
    {
        private List<CSharpVersionUpdateRule> updateRules;

        public CSharpUpdater(string version)
        {
            this.updateRules = new List<CSharpVersionUpdateRule>();
            if (!String.IsNullOrEmpty(version))
            {
                this.updateRules.Add(new CSharpVersionUpdateRule("AssemblyVersion", version));
                this.updateRules.Add(new CSharpVersionUpdateRule("AssemblyFileVersion", version));
            }
        }

        public void UpdateFile(string fileName)
        {
            string[] lines = File.ReadAllLines(fileName);
            var outlines = new List<string>();
            foreach (var line in lines)
            {
                outlines.Add(UpdateLine(line));
            }
            File.WriteAllLines(fileName, outlines.ToArray());
        }

        private string UpdateLine(string line)
        {
            foreach (var rule in updateRules)
            {
                if (UpdateLineWithRule(ref line, rule))
                {
                    break;
                }
            }
            return line;
        }

        public static bool UpdateLineWithRule(ref string line, CSharpVersionUpdateRule rule)
        {
            VersionString v = null;
            bool updated = false;
            var g = GetVersionString(line, rule.AttributeName);
            if (g != null)
            {
                VersionString.TryParse(g.Value, out v);
            }
            if (v != null)
            {
                string newVersion = rule.Update(v);
                line = line.Substring(0, g.Index) + newVersion + line.Substring(g.Index + g.Length);
                updated = true;
            }
            return updated;
        }

        public static Group GetVersionString(string input, string attributeName)
        {
            var commentIndex = input.IndexOf("//");
            if (commentIndex != -1)
            {
                input = input.Substring(0, commentIndex);
            }
            string attributeMatch = String.Format("(?:(?:{0})|(?:{0}Attribute))", attributeName);

            string pattern = @"^\s*\[assembly: " + attributeMatch + @"\(""(?<Version>[0-9\.\*]+)""\)\]";
            Regex regex = new Regex(pattern);
            var m = regex.Match(input);
            if (m.Success)
            {
                return m.Groups["Version"];
            }
            return null;
        }
    }
}
