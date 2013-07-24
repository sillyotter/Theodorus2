namespace Theodorus2.Views
{
    public interface IUserPromptingService
    {
        bool PromptUserYesNo(string title, string question);
        void DisplayAlert(string messaege);
    }
}