#region Usings
using FncControl.AppConfig;
using FncControl.Enums;
using FncControl.Models;
using FncControl.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
#endregion

namespace FncControl.ViewModels
{
	internal class MainPageViewModel : BaseViewModel
	{
		#region Fields
		// Core
		private readonly ISettingsService _service;
		private CurrentData _data;
		private Setting _setting;

		// UI fields
		private int _turnCounter = 0;
		private int _playerTime;
		private int _otherPlayerTime;
		private bool _timeControlActivated;
		private int _numberOfDrawing;
		private int _numberOfSpell;
		private int _numberOfSummon;
		private int _numberOfBuy;
		private int _numberOfAttach;
		private int _numberOfAttack;
		private string _diceImageSource;
		private bool _diceVisible;
		private Rising _nextRising;
		private string _restartImgButtonSource;
		private string _startStopImgButtonSource;
		private string _playerTimeLabel;
		private string _otherPlayerTimeLabel;

		// Logic
		private bool _initialized;
		private bool _isPaused;
		private List<int> _forbiddenNumbers;
		private List<int> _enabledNumbers;
		private Task _diceProcess;
		private Task _gameLoopTask;
		private bool _enableDrawing;
		private bool _enableSpell;
		private bool _enableSummon;
		private bool _enableBuy;
		private bool _enableAttach;
		private bool _enableAttack;
		private int _rotation;
		#endregion

		public MainPageViewModel()
		{
			_forbiddenNumbers = new List<int>();
			_enabledNumbers = new List<int>();

			NextTurnCommand = new Command(NextTurnCommandExecute);
			RestartCommand = new Command(RestartCommandExecute);
			StartStopCommand = new Command(StartStopCommandExecute);

			_service = AppSetup.Container.GetService<ISettingsService>();

			_service.SettingsChanged += SettingsChangedEventeExecute;
		}

		#region Properties
		public bool TimeControlActivated
		{
			get => _timeControlActivated;
			set
			{
				_timeControlActivated = value;
				OnPropertyChanged();
			}
		}
		public bool EnableDrawing
		{
			get => _enableDrawing;
			set
			{
				_enableDrawing = value;
				OnPropertyChanged();
			}
		}
		public bool EnableSpell
		{
			get => _enableSpell;
			set
			{
				_enableSpell = value;
				OnPropertyChanged();
			}
		}
		public bool EnableSummon
		{
			get => _enableSummon;
			set
			{
				_enableSummon = value;
				OnPropertyChanged();
			}
		}
		public bool EnableBuy
		{
			get => _enableBuy;
			set
			{
				_enableBuy = value;
				OnPropertyChanged();
			}
		}
		public bool EnableAttach
		{
			get => _enableAttach;
			set
			{
				_enableAttach = value;
				OnPropertyChanged();
			}
		}
		public bool EnableAttack
		{
			get => _enableAttack;
			set
			{
				_enableAttack = value;
				OnPropertyChanged();
			}
		}
		public int TurnCounter
		{
			get => _turnCounter;
			set
			{
				_turnCounter = value;
				OnPropertyChanged();
			}
		}
		public int PlayerTime
		{
			get => _playerTime;
			set
			{
				_playerTime = value;
				OnPropertyChanged();
			}
		}
		public string PlayerTimeLabel
		{
			get => _playerTimeLabel;
			set
			{
				_playerTimeLabel = value;
				OnPropertyChanged();
			}
		}
		public int OtherPlayerTime
		{
			get => _otherPlayerTime;
			set
			{
				_otherPlayerTime = value;
				OnPropertyChanged();
			}
		}
		public string OtherPlayerTimeLabel
		{
			get => _otherPlayerTimeLabel;
			set
			{
				_otherPlayerTimeLabel = value;
				OnPropertyChanged();
			}
		}
		public int NumberOfDrawing
		{
			get => _numberOfDrawing;
			set
			{
				_numberOfDrawing = value;
				OnPropertyChanged();
			}
		}
		public int NumberOfSpell
		{
			get => _numberOfSpell;
			set
			{
				_numberOfSpell = value;
				OnPropertyChanged();
			}
		}
		public int NumberOfSummon
		{
			get => _numberOfSummon;
			set
			{
				_numberOfSummon = value;
				OnPropertyChanged();
			}
		}
		public int NumberOfBuy
		{
			get => _numberOfBuy;
			set
			{
				_numberOfBuy = value;
				OnPropertyChanged();
			}
		}
		public int NumberOfAttach
		{
			get => _numberOfAttach;
			set
			{
				_numberOfAttach = value;
				OnPropertyChanged();
			}
		}
		public int NumberOfAttack
		{
			get => _numberOfAttack;
			set
			{
				_numberOfAttack = value;
				OnPropertyChanged();
			}
		}
		public int Rotation
		{
			get => _rotation;
			set
			{
				_rotation = value;
				OnPropertyChanged();
			}
		}
		public bool DiceVisible
		{
			get => _diceVisible;
			set
			{
				_diceVisible = value;
				OnPropertyChanged();
			}
		}
		public string DiceImageSource
		{
			get => _diceImageSource;
			set
			{
				_diceImageSource = value;
				OnPropertyChanged();
			}
		}
		public string RestartImgButtonSource
		{
			get => _restartImgButtonSource;
			set
			{
				_restartImgButtonSource = value;
				OnPropertyChanged();
			}
		}
		public string StartStopImgButtonSource
		{
			get => _startStopImgButtonSource;
			set
			{
				_startStopImgButtonSource = value;
				OnPropertyChanged();
			}
		}
		public Rising NextRising
		{
			get => _nextRising;
			set
			{
				_nextRising = value;
				OnPropertyChanged();
			}
		}
		#endregion

		#region Command
		public ICommand NextTurnCommand { get; set; }
		public ICommand RestartCommand { get; set; }
		public ICommand StartStopCommand { get; set; }
		#endregion

		internal void Init()
		{
			if (_initialized)
				return;

			_isPaused = true;

			_setting = _service.GetSettings();

			// Event settings changed
			_data = new CurrentData()
			{
				RoundCounter = 0,
				PlayerTime1 = _setting.MaxPlayerTime1 * 60,
				PlayerTime2 = _setting.MaxPlayerTime2 * 60,
				NumberOfAttach = 1,
				NumberOfAttack = 1,
				NumberOfBuy = 1,
				NumberOfDrawing = 4,
				NumberOfSummon = 1,
				NumberOfSpell = 1,
			};

			TurnCounter = _data.RoundCounter;

			PlayerTime = _data.PlayerTime1;
			TimeSpan time = TimeSpan.FromSeconds(PlayerTime);
			PlayerTimeLabel = time.ToString(@"hh\:mm\:ss");

			OtherPlayerTime = _data.PlayerTime2;
			time = TimeSpan.FromSeconds(OtherPlayerTime);
			OtherPlayerTimeLabel = time.ToString(@"hh\:mm\:ss");

			NumberOfDrawing = _data.NumberOfDrawing;
			NumberOfSummon = _data.NumberOfSummon;
			NumberOfSpell = _data.NumberOfSpell;
			NumberOfBuy = _data.NumberOfBuy;
			NumberOfAttack = _data.NumberOfAttack;
			NumberOfAttach = _data.NumberOfAttach;

			TimeControlActivated = _setting.TimeOn;

			EnableAttach= _setting.EnableAttach;
			EnableAttack= _setting.EnableAttack;	
			EnableBuy= _setting.EnableBuy;
			EnableDrawing= _setting.EnableDrawing;
			EnableSummon= _setting.EnableSummon;
			EnableSpell = _setting.EnableMagic;

			RestartImgButtonSource = "restart.png";
			StartStopImgButtonSource = "play1.png";

			Rotation = 0;

			_initialized = true;

			CreateEnabledNumbersList();

			DetermineNextRising();

			StartGameLoop();
		}

		private void CreateEnabledNumbersList()
		{
			_enabledNumbers.Clear();

			if (EnableDrawing)
				_enabledNumbers.Add(1);	
			if (EnableSpell)
				_enabledNumbers.Add(2);
			if (EnableSummon)
				_enabledNumbers.Add(3);	
			if (EnableBuy)
				_enabledNumbers.Add(4);
			if (EnableAttach)
				_enabledNumbers.Add(5);	
			if (EnableAttack)
				_enabledNumbers.Add(6);
		}

		private async void DetermineNextRising()
		{
			DiceImageSource = "1.png";

			DiceVisible = true;

			int randomNumber = 0;

			_diceProcess = new Task(() =>
			{
				int i = 0;
				while (i < 11)
				{
					if (_enabledNumbers.Count == 1)
					{
						randomNumber = _enabledNumbers.First();
						i = 11;
					}

					Random r = new Random();
					var randomIndex = r.Next(_enabledNumbers.Count);

					randomNumber = _enabledNumbers[randomIndex];					

					DiceImageSource = string.Format("{0}.png", randomNumber);

					Thread.Sleep(125);

					i++;
				}

				_enabledNumbers.Remove(randomNumber);

				if (_enabledNumbers.Count <= 0)
					CreateEnabledNumbersList();
			});
			_diceProcess.Start();

			await _diceProcess;

			NextRising = (Rising)randomNumber;

			await Task.Delay(945);
			DiceVisible = false;
		}

		private void StartGameLoop()
		{
			if (_gameLoopTask == null)
				_gameLoopTask = new Task(() =>
				{
					while (true)
					{
						if (!_isPaused)
						{
							//PlayerTimeLabel = string.Format("{0}:00", Math.Floor((decimal)PlayerTime / 60));

							Thread.Sleep(1000);

							PlayerTime--;
							TimeSpan time = TimeSpan.FromSeconds(PlayerTime);
							PlayerTimeLabel = time.ToString(@"hh\:mm\:ss");
						}
					}
				});

			if (_gameLoopTask.Status != TaskStatus.Running)
				_gameLoopTask.Start();
		}

		private async void NextTurnCommandExecute(object obj)
		{
			if (_isPaused || DiceVisible)
				return;

			if (TurnCounter % 2 == 0)
			{
				_data.PlayerTime1 = PlayerTime;
				_data.PlayerTime2 = OtherPlayerTime;

				Rotation = 180;
			}
			else
			{
				_data.PlayerTime1 = OtherPlayerTime;
				_data.PlayerTime2 = PlayerTime;

				Rotation = 0;
			}

			TurnCounter++;

			_data.RoundCounter = _turnCounter;

			// Change PlayerTimes
			var t = PlayerTime;
			var l = PlayerTimeLabel;
			PlayerTime = OtherPlayerTime;
			PlayerTimeLabel = OtherPlayerTimeLabel;
			OtherPlayerTimeLabel = l;
			OtherPlayerTime = t;

			if (TurnCounter % 5 == 0)
			{
				await _diceProcess;

				// NextRising			
				ExecuteRising();
				DetermineNextRising();
			}
		}

		private async void RestartCommandExecute(object obj)
		{
			bool answer = await App.Current.MainPage.DisplayAlert("Restart?", "Would you like to restart the control?", "Yes", "No");

			if (answer)
			{
				_initialized = false;
				Init();
			}
		}

		private void StartStopCommandExecute(object obj)
		{
			_isPaused = !_isPaused;
			StartStopImgButtonSource = _isPaused ? "play1.png" : "pause1.png";
		}

		private void ExecuteRising()
		{
			switch (NextRising)
			{
				case Rising.DRAW:
					NumberOfDrawing++;
					break;
				case Rising.SPELL:
					NumberOfSpell++;
					break;
				case Rising.SUMMON:
					NumberOfSummon++;
					break;
				case Rising.BUY:
					NumberOfBuy++;
					break;
				case Rising.ATTACH:
					NumberOfAttach++;
					break;
				case Rising.ATTACK:
					NumberOfAttack++;
					break;
				default:
					break;
			}
		}

		private void SettingsChangedEventeExecute(object sender, EventArgs args)
		{
			RestartCommandExecute("");
		}
	}
}
