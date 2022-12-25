using FncControl.AppConfig;
using FncControl.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FncControl.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SettingsPage : ContentPage
	{
		public SettingsPage()
		{
			InitializeComponent();
			BindingContext = ViewModel;
		}

		internal SettingsViewModel ViewModel { get; set; } = AppSetup.Container.GetInstance<SettingsViewModel>();

		protected override void OnAppearing()
		{
			base.OnAppearing();
			ViewModel.Init();
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			ViewModel.Save();
		}

		private void MaxPlayerTimePicker1_SelectionChanged(object sender, EventArgs e)
		{
			ViewModel.SelectionChangedEventExecute(0);
        }

		private void MaxPlayerTimePicker2_SelectionChanged(object sender, EventArgs e)
		{
			ViewModel.SelectionChangedEventExecute(1);
		}

		private void LanguagePicker_SelectionChanged(object sender, EventArgs e)
		{
			ViewModel.ChangeLanguageEventExecute();
		}
    }
}