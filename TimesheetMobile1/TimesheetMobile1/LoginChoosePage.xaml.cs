using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimesheetMobile1
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginChoose : ContentPage
	{
		public LoginChoose ()
		{
			InitializeComponent ();
		}
        public async void KirjauduSisaan(object sender, EventArgs e)
        {

            await Navigation.PushAsync(new LoginPage());

        }
        public async void UusiKayttaja(object sender, EventArgs e)
        {

            await Navigation.PushAsync(new LoginNewUserPage());

        }
    }
}