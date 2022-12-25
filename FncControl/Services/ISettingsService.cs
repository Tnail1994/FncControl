using FncControl.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FncControl.Services
{
	internal interface ISettingsService
	{
		EventHandler SettingsChanged { get; set; }
		Setting GetSettings();
		void SaveSettingsAsync(Setting setting);
	}
}
