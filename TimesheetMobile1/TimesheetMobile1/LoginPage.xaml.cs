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
using System.Security.Cryptography;

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
            string username = "";
            string password = ""; 

            if (!string.IsNullOrEmpty(Sana1.Text)&& !string.IsNullOrEmpty(Tunnus1.Text))
            {
                using (var sha = SHA256.Create())
                {
                    var bytes = Encoding.UTF8.GetBytes(Sana1.Text);
                    var hash = sha.ComputeHash(bytes);


                    password = Convert.ToBase64String(hash);
                    username = Tunnus1.Text;
                    //return Convert.ToBase64String(hash);
                }
            }
            else
            {
                await DisplayAlert("", "Syötä käyttäjätunnus ja salasana.", "OK");
            }


            if (!string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(username))
            {

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
                    await DisplayAlert("", errorMessage1, "Close");
                    //Toast.MakeText(this, errorMessage1, ToastLength.Long).Show();

                    await Navigation.PushAsync(new EmployeePage());
                }
                else
                {
                    var errorMessage1 = response.Content.ReadAsStringAsync().Result.Replace("\\", "").Trim(new char[1]
                    {
                '"'
                    });
                    await DisplayAlert("", errorMessage1, "Close");
                    //Toast.MakeText(this, errorMessage1, ToastLength.Long).Show();
                }
            }

        }
        public async void UusiKayttaja(object sender, EventArgs e)
        {

            await Navigation.PushAsync(new LoginNewUserPage());

        }
    }
}