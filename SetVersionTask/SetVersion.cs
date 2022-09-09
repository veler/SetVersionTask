using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.Diagnostics;

namespace SetVersionTask
{
    public class SetVersion : Task
    {
        [Required]
        public string FileName { get; set; }

        public string Version { get; set; }

        public override bool Execute()
        {
            try
            {
                if (this.FileName.EndsWith(".cs", StringComparison.OrdinalIgnoreCase))
                {
                    var updater = new CSharpUpdater(Version);
                    updater.UpdateFile(FileName);
                }
                else if (this.FileName.EndsWith(".appxmanifest", StringComparison.OrdinalIgnoreCase))
                {
                    var updater = new AppxManifestUpdater(Version);
                    updater.UpdateFile(FileName);
                }
                else if (this.FileName.EndsWith(".nuspec", StringComparison.OrdinalIgnoreCase))
                {
                    UpdateNuSpec();
                }
            }
            catch (Exception e)
            {
                Log.LogError(e.Message);
                return false;
            }
            return true;
        }

        private void UpdateNuSpec()
        {


        }

        private void ValidateArguments()
        {

        }

    }
}
