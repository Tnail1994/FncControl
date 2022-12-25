#region Usings
using FncControl.AppConfig;
using FncControl.Models;
using FncControl.Resources;
using FncControl.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
#endregion

namespace FncControl.ViewModels
{
	internal class SettingsViewModel : BaseViewModel
	{
		#region Fields
		// Core
		private readonly ISettingsService _service;
		private Setting _setting;

		// UI Fields
		private bool _enableDrawing;
		private bool _enableMagic;
		private bool _enableSummon;
		private bool _enableBuy;
		private bool _enableAttach;
		private bool _enableAttack;
		private bool _timeOn;
		private int _maxPlayerTime1;
		private int _maxPlayerTime2;
		private string _linkTimesButtonImg;

		// Logic
		private bool _initialized;
		private bool _settingsChanged;
		private bool _linkTimesOn = true;
		private Language _currentLanguage;
		#endregion

		public SettingsViewModel()
		{
			_setting = new Setting();

			PlayerTimes = new List<int>()
			{
				15,30,45,60,75,90
			};

			MaxRounds = new List<int>()
			{
				30,40,50,60,70,80,90,100,110,120
			};

			AvailableLanguages = new List<Language>
			{
				new Language(){Name = AppResources.English, CI = "en"},
				new Language(){Name = AppResources.German, CI = "de"}
			};

			LinkTimesCommand = new Command(LinkTimesCommandExecute);

			_service = AppSetup.Container.GetService<ISettingsService>();
		}

		private void LinkTimesCommandExecute(object obj)
		{
			_linkTimesOn = !_linkTimesOn;
			LinkTimesButtonImg = _linkTimesOn ? "link-variantSMALL.png" : "link-variant-offSMALL.png";
		}

		#region Properties
		public Setting Setting
		{
			get => _setting;
			set
			{
				_setting = value;
				OnPropertyChanged();
			}
		}
		public bool TimeOn
		{
			get => _timeOn;
			set
			{
				_timeOn = value;
				_settingsChanged = true;
				OnPropertyChanged();
			}
		}
		public List<int> PlayerTimes { get; set; }
		public List<int> MaxRounds { get; set; }
		public List<Language> AvailableLanguages { get; set; }
		public Language CurrentLanguage
		{
			get => _currentLanguage;
			set
			{
				_currentLanguage = value;
				OnPropertyChanged();
			}
		}
		public int MaxPlayerTime1
		{
			get => _maxPlayerTime1;
			set
			{
				_maxPlayerTime1 = value;
				_settingsChanged = true;
				OnPropertyChanged();
			}
		}
		public int MaxPlayerTime2
		{
			get => _maxPlayerTime2;
			set
			{
				_maxPlayerTime2 = value;
				_settingsChanged = true;
				OnPropertyChanged();
			}
		}
		public string LinkTimesButtonImg
		{
			get => _linkTimesButtonImg;
			set
			{
				_linkTimesButtonImg = value;
				OnPropertyChanged();
			}
		}
		public bool EnableDrawing
		{
			get => _enableDrawing;
			set
			{
				_enableDrawing = value;
				_settingsChanged = true;
				OnPropertyChanged();
			}
		}
		public bool EnableMagic
		{
			get => _enableMagic;
			set
			{
				_enableMagic = value;
				_settingsChanged = true;
				OnPropertyChanged();
			}
		}
		public bool EnableSummon
		{
			get => _enableSummon;
			set
			{
				_enableSummon = value;
				_settingsChanged = true;
				OnPropertyChanged();
			}
		}
		public bool EnableBuy
		{
			get => _enableBuy;
			set
			{
				_enableBuy = value;
				_settingsChanged = true;
				OnPropertyChanged();
			}
		}
		public bool EnableAttach
		{
			get => _enableAttach;
			set
			{
				_enableAttach = value;
				_settingsChanged = true;
				OnPropertyChanged();
			}
		}
		public bool EnableAttack
		{
			get => _enableAttack;
			set
			{
				_enableAttack = value;
				_settingsChanged = true;
				OnPropertyChanged();
			}
		}
		#endregion

		public ICommand LinkTimesCommand { get; set; }

		internal void Init()
		{
			if (_initialized)
				return;

			_initialized = true;

			Setting = _service.GetSettings();

			EnableDrawing = Setting.EnableDrawing;
			EnableMagic = Setting.EnableMagic;
			EnableSummon = Setting.EnableSummon;
			EnableBuy = Setting.EnableBuy;
			EnableAttach = Setting.EnableAttach;
			EnableAttack = Setting.EnableAttack;
			MaxPlayerTime1 = Setting.MaxPlayerTime1;
			MaxPlayerTime2 = Setting.MaxPlayerTime2;
			TimeOn = Setting.TimeOn;

			LinkTimesButtonImg = "link-variantSMALL.png";

			CurrentLanguage = AvailableLanguages.FirstOrDefault(
				x => x.CI == LocalizationResourceManager.Current.CurrentCulture.TwoLetterISOLanguageName);

			_settingsChanged = false;
		}

		internal void Save()
		{
			if (!_settingsChanged)
				return;

			_settingsChanged = false;

			Setting.EnableBuy = EnableBuy;
			Setting.EnableDrawing = EnableDrawing;
			Setting.EnableAttack = EnableAttack;
			Setting.EnableAttach = EnableAttach;
			Setting.EnableMagic = EnableMagic;
			Setting.EnableSummon = EnableSummon;
			Setting.MaxPlayerTime1 = MaxPlayerTime1;
			Setting.MaxPlayerTime2 = MaxPlayerTime2;
			Setting.TimeOn = TimeOn;

			//Setting.MaxRounds = 
			//Setting.EarlyEnd =

			_service.SaveSettingsAsync(Setting);
		}

		internal void SelectionChangedEventExecute(int player)
		{
			if (!_linkTimesOn)
				return;

			if (player == 0)
				MaxPlayerTime2 = MaxPlayerTime1;
			else
				MaxPlayerTime1 = MaxPlayerTime2;
		}

		internal void ChangeLanguageEventExecute()
		{
			//CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.UserCustomCulture | CultureTypes.SpecificCultures);

			//var getlanguage = System.Globalization.CultureInfo.CurrentUICulture.Name;

			LocalizationResourceManager.Current.CurrentCulture
				= CultureInfo.GetCultureInfo(CurrentLanguage.CI);
		}
	}
}
