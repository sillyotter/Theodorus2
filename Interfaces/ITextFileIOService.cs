namespace Theodorus2.Interfaces
{
    public interface ITextFileIOService
    {
        void WriteFile(string fileName, string data);
        string ReadFile(string fileName);
    }
}