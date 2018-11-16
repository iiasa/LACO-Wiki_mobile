namespace LacoWikiMobile.App.ViewModels.Map
{
	using System.ComponentModel;
	using LacoWikiMobile.App.ViewModels.Main;
	using LacoWikiMobile.App.ViewModels.Shared;

	public class LayerItemViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public string Icon { get; set; }

		public int Id { get; set; }

		public bool IsChecked { get; set; }

		public string Name { get; set; }
	}
}