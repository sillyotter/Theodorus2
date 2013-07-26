using Microsoft.Win32;
using Theodorus2.Interfaces;

namespace Theodorus2.Views.Dialogs
{
    public class FileSelectionService : IFileSelectionService
    {
        public string PromptToOpenFile(string defaultExtension, string filters)
        {
            var ofd = new OpenFileDialog
            {
                AddExtension = false,
                CheckFileExists = false,
                CheckPathExists = true,
                DefaultExt = defaultExtension,
                Filter = filters,
                FilterIndex = 0,
                RestoreDirectory = true,
                Multiselect = false,
                DereferenceLinks = true,
                ValidateNames = true,
            };

            return ofd.ShowDialog() == true ? ofd.FileName : null;
        }

        public string PromptToSaveFile(string defaultExtension, string filters)
        {
            var ofd = new SaveFileDialog()
            {
                AddExtension = true,
                CheckFileExists = false,
                CheckPathExists = true,
                OverwritePrompt = true,
                DefaultExt = defaultExtension,
                Filter = filters,
                FilterIndex = 0,
                RestoreDirectory = true,
                DereferenceLinks = true,
                ValidateNames = true,
            };

            return ofd.ShowDialog() == true ? ofd.FileName : null;
        }
    }
}