using System;
using System.Collections.Generic;

namespace SetVersionTask
{
    public class VersionUpdateRule
    {
        private string[] partRules;
        public VersionUpdateRule(string rule)
        {
            this.partRules = rule.Split('.');
            if (partRules.Length < 2 || partRules.Length > 4)
            {
                throw new ArgumentException("Expecting 2-4 version parts");
            }
            foreach (var partRule in partRules)
            {
                if (partRule == "+" || partRule == "=")
                {
                    // OK, valid rule
                }
                else
                {
                    // will throw an exception if not an int
                    int.Parse(partRule);
                }
            }
        }

        public string Update(string version)
        {
            return Update(new VersionString(version));
        }

        public string Update(VersionString version)
        {
            List<string> inParts = new List<string>() { version.Major, version.Minor, version.Build, version.Revision };
            List<string> outParts = new List<string>();
            for (int index = 0; index < partRules.Length; index++)
            {
                var rule = partRules[index];
                var inPart = inParts[index];
                if (rule == "=")
                {
                    if (inPart.Length > 0)
                    {
                        outParts.Add(inParts[index]);
                    }
                }
                else if (rule == "+")
                {
                    if (inPart.Length == 0)
                    {
                        throw new ArgumentException("Can't increment missing value");
                    }
                    int inNumber = 0;
                    int.TryParse(inPart, out inNumber); // * gets turned into a zero
                    inNumber++;
                    outParts.Add(inNumber.ToString());
                }
                else
                {
                    // must be a numeric literal
                    outParts.Add(partRules[index]);
                }
            }
            return String.Join(".", outParts.ToArray());
        }


    }
}
