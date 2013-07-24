namespace Theodorus2.Interfaces
{
    public interface IUserPromptingService
    {
        bool PromptUserYesNo(string title, string question);
        void DisplayAlert(string messaege);
    }
}