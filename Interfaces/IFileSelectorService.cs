namespace Theodorus2.Interfaces
{
    interface IFileSelectorService
    {
        string PromptForFile(string defaultExtension, string filters);
    }
}