using System.IO;
using Theodorus2.Interfaces;

namespace Theodorus2.Support
{
    public class DefaultTextFileIOService : ITextFileIOService
    {
        public void WriteFile(string fileName, string data)
        {
            File.WriteAllText(fileName, data);
        }

        public string ReadFile(string fileName)
        {
            return File.ReadAllText(fileName);
        }
    }
}