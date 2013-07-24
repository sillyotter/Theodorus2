namespace Theodorus2.Interfaces
{
    public interface IFileSelectionService
    {
        string PromptToOpenFile(string defaultExtension, string filters);
        string PromptToSaveFile(string defaultExtension, string filters);
    }
}