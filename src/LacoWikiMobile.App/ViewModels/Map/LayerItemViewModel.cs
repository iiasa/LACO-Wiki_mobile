namespace LacoWikiMobile.App.ViewModels.Map
{
	using LacoWikiMobile.App.ViewModels.Main;

	public class LayerItemViewModel : ItemViewModel
	{
		public string Icon { get; set; }

		public bool IsEnabled { get; set; }

		public bool IsRaster { get; set; }

		public string FileName { get; set; }

		public string TextColor
		{
			get
			{
				if (IsEnabled)
				{
					return "#000000";
				}
				else
				{
					return "#CCCCCC";
				}
			}

			set
			{
			}
		}
	}
}