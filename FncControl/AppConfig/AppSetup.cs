using FncControl.Resources;
using FncControl.Services;
using FncControl.ViewModels;
using Xamarin.CommunityToolkit.Helpers;

namespace FncControl.AppConfig
{
    public class AppSetup
    {
        public static SimpleInjector.Container Container { get; private set; }

        public static void Initialize()
        {
            // Create the container and register the dependencies
            Container = new SimpleInjector.Container();

            LocalizationResourceManager.Current.Init(AppResources.ResourceManager);

            RegisterServices();
            RegisterViewModels();

            // Verify the container to ensure that there are no configuration errors
            Container.Verify();
        }

        private static void RegisterViewModels()
        {
            Container.RegisterSingleton<MainPageViewModel>();
            Container.RegisterSingleton<SettingsViewModel>();
        }

        private static void RegisterServices()
        {
            Container.RegisterSingleton<ISettingsService, SettingsService>();
        }
    }
}
