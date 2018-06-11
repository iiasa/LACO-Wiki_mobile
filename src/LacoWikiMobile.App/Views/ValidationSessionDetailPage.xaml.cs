// <copyright file="ValidationSessionDetailPage.xaml.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LacoWikiMobile.App.Views
{
	using System;
	using System.Threading.Tasks;
	using LacoWikiMobile.App.UserInterface;
	using LacoWikiMobile.App.ViewModels;
	using Xamarin.Forms;
	using Xamarin.Forms.Xaml;

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ValidationSessionDetailPage : ContentPage
	{
		public ValidationSessionDetailPage()
		{
			try
			{
				InitializeComponent();
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();

			ValidationSessionDetailPageViewModel viewModel = BindingContext as ValidationSessionDetailPageViewModel;

			if (viewModel != null)
			{
				viewModel.PropertyChanged += (sender, args) =>
				{
					if (args.PropertyName == nameof(ValidationSessionDetailPageViewModel.ViewModel))
					{
						if (viewModel.ViewModel != null)
						{
							Task.Run(async () =>
							{
								await Task.Delay(500);

								this.circularProgressBar.Progress = 0;
								this.circularProgressBar.ProgressTo(viewModel.ViewModel.Progress, 500, Easing.CubicOut);
							});
						}
					}
				};
			}
		}
	}
}