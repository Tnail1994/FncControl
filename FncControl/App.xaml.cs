using FncControl.AppConfig;
using FncControl.Services;
using FncControl.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FncControl
{
	public partial class App : Application
	{

		public App()
		{
			InitializeComponent();

			AppSetup.Initialize();

			MainPage = new AppShell();
		}

		protected override void OnStart()
		{
		}

		protected override void OnSleep()
		{
		}

		protected override void OnResume()
		{
		}
	}
}
