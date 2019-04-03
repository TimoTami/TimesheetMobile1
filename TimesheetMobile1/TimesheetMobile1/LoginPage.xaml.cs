using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TimesheetMobile1.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimesheetMobile1
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
        

		public LoginPage ()
		{
			InitializeComponent ();
		}
        public async void KirjauduSisaan(object sender, EventArgs e)
        {

            //NewUserModel data = new NewUserModel()
            //{
            //username = Tunnus1.Text,
            //password = Sana1.Text
            //};

            string username = Tunnus1.Text;
            string password = Sana1.Text;
            



            HttpClient client = new HttpClient();
            var uri = new Uri(string.Format("https://joonanmobiili.azurewebsites.net/api/login?username=" + username + "&password=" + password));
            
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                var errorMessage1 = response.Content.ReadAsStringAsync().Result.Replace("\\", "").Trim(new char[1]
                {
                '"'
                });
                //Toast.MakeText(this, errorMessage1, ToastLength.Long).Show();

                await Navigation.PushAsync(new EmployeePage());
            }
            else
            {
                var errorMessage1 = response.Content.ReadAsStringAsync().Result.Replace("\\", "").Trim(new char[1]
                {
                '"'
                });
                //Toast.MakeText(this, errorMessage1, ToastLength.Long).Show();
            }

        }
        public async void UusiKayttaja(object sender, EventArgs e)
        {

            await Navigation.PushAsync(new LoginNewUserPage());

        }
    }
}