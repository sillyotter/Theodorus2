using System.Windows.Input;

namespace Theodorus2.Views
{
	public partial class About
	{
		public About()
		{
			InitializeComponent();
			DataContext = this;
		}

		private void OnMouseClick(object sender, MouseButtonEventArgs e)
		{
			Close();
		}

		public string Version
		{
			get { return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(); }
		}
	}
}