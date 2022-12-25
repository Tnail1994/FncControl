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
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
			BindingContext = ViewModel;
		}

		internal MainPageViewModel ViewModel { get; set; } = AppSetup.Container.GetInstance<MainPageViewModel>();

		protected override void OnAppearing()
		{
			base.OnAppearing();
			ViewModel.Init();
		}
    }
}