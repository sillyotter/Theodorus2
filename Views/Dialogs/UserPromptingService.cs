using System.Windows;
using Theodorus2.Interfaces;

namespace Theodorus2.Views.Dialogs
{
    public class UserPromptingService : IUserPromptingService
    {
        public bool PromptUserYesNo(string title, string question)
        {
            return MessageBox.Show(Application.Current.MainWindow, question, title, MessageBoxButton.YesNo,
                MessageBoxImage.Question,
                MessageBoxResult.No) == MessageBoxResult.Yes;
        }

        public void DisplayAlert(string message)
        {
            MessageBox.Show(Application.Current.MainWindow, message, "Alert", MessageBoxButton.OK,
                MessageBoxImage.Exclamation,
                MessageBoxResult.OK);
        }
    }
}