using FncControl.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
namespace FncControl.Services
{
	internal class SettingsService : ISettingsService
	{
		private Setting _setting;
		private Setting DEFAULT_SETTING;
		private bool _settingsToLoad = false;
		private const string FILE_NAME = "Settings.json";
		public EventHandler SettingsChanged { get; set; }

		public SettingsService()
		{
			DEFAULT_SETTING = new Setting()
			{
				MaxPlayerTime1 = 45,
				MaxPlayerTime2 = 45,
				TimeOn = false,
				//PlayerTimesDifferent=false,
				MaxRounds = 90,
				EarlyEnd = false,
				EnableAttach = true,
				EnableAttack = true,
				EnableBuy = true,
				EnableDrawing = true,
				EnableMagic = true,
				EnableSummon = true
			};

			_setting = DEFAULT_SETTING;
		}

		public Setting GetSettings()
		{
			//_settingsToLoad = File.Exists(FILE_NAME);

			if (!_settingsToLoad)
			{
				// Default
				return DEFAULT_SETTING;
			}

			return _setting;

			//string fileContents = "";

			//using (var stream = await FileSystem.OpenAppPackageFileAsync(FILE_NAME))
			//{
			//	using (var reader = new StreamReader(stream))
			//	{
			//		 fileContents = await reader.ReadToEndAsync();
			//	}
			//}

			//return JsonConvert.DeserializeObject<Setting>(fileContents);
		}

		public void SaveSettingsAsync(Setting setting)
		{
			//var mainDir = FileSystem.AppDataDirectory;

			_setting = setting;
			_settingsToLoad = true;
			SettingsChanged?.Invoke(this, EventArgs.Empty);

			//var jsonString = JsonConvert.SerializeObject(setting);

			//using (var stream = await FileSystem.OpenAppPackageFileAsync(FILE_NAME))
			//{
			//	using (var writer = new StreamWriter(stream))
			//	{
			//		writer.WriteLine(jsonString);
			//	}
			//}
		}

	}
}
