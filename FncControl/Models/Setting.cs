using System;
using System.Collections.Generic;
using System.Text;

namespace FncControl.Models
{
	internal class Setting
	{
		public int MaxPlayerTime1 { get; set; }
		public int MaxPlayerTime2 { get; set; }
		public bool TimeOn { get; set; }
		public bool EarlyEnd { get; set; }
		public int MaxRounds { get; set; }
		public bool EnableDrawing { get; set; }
		public bool EnableMagic { get; set; }
		public bool EnableSummon { get; set; }
		public bool EnableBuy { get; set; }
		public bool EnableAttach { get; set; }
		public bool EnableAttack { get; set; }
	}
}
