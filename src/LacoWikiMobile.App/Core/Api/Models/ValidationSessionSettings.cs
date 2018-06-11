namespace LacoWikiMobile.App.Core.Api.Models
{
	// Source: LacoWiki.Portal.Mvc.Models.Mobile
	public class ValidationSessionSettings
	{
		public bool CardinalDirectionPhotosOptional { get; set; }

		// Maximum distance in meters to allow the validation, null => no limitation
		public int? Distance { get; set; }

		public bool PointPhotoOptional { get; set; }

		public bool TakeCardinalDirectionPhotos { get; set; }

		public bool TakePointPhoto { get; set; }
	}
}