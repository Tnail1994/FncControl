using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FncControl.Controls
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ContentButton : ContentView
	{
		public ContentButton()
		{
			InitializeComponent();
		}

		public event EventHandler Tapped;

		public static readonly BindableProperty CommandProperty 
			= BindableProperty.Create<ContentButton, ICommand>(c => c.Command, null);

		public ICommand Command
		{
			get { return (ICommand)GetValue(CommandProperty); }
			set { SetValue(CommandProperty, value); }
		}

		private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
		{
			if (Tapped != null)
				Tapped(this, new EventArgs());
		}
	}
}