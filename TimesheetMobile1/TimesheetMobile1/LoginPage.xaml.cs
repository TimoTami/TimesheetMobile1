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
using Xamarin.Essentials;
using Newtonsoft.Json;

namespace TimesheetMobile1
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
        bool ohitus = true;

		public LoginPage ()
		{
            AutoKirjautuminen();

            if (ohitus == false)
            {
                InitializeComponent();
            }
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
                    if (MuistaMinut.IsToggled==true)
                    {
                        try
                        {
                            await SecureStorage.SetAsync("oauth_token1", username + " " + password);
                        }
                        catch (Exception ex)
                        {
                            await DisplayAlert("Login tallennus ei onnistunut", ex.ToString(), "Close");
                            // Possible that device doesn't support secure storage on device.
                        }
                    }

                    var errorMessage1 = response.Content.ReadAsStringAsync().Result.Replace("\\", "").Trim(new char[1]
                    {
                    '"'
                    });
                    await DisplayAlert("", errorMessage1, "Close");
                    await Navigation.PushAsync(new EmployeePage());
                }
                else
                {
                    var errorMessage1 = response.Content.ReadAsStringAsync().Result.Replace("\\", "").Trim(new char[1]
                    {
                    '"'
                    });
                    await DisplayAlert("", errorMessage1, "Close");
                }
            }

        }

        public async void UusiKayttaja(object sender, EventArgs e)
        {

            await Navigation.PushAsync(new LoginNewUserPage());

        }

        //protected override async void OnAppearing()
        //{
        //    base.OnAppearing();
            

        //    string password = null;
        //    string username = null;
        //    try
        //    {
        //        var oauthToken = await SecureStorage.GetAsync("oauth_token1");
        //        //password = oauthToken;
        //        //string hemuli = "3";

        //        if (oauthToken != null)
        //        {

        //            string[] nameParts = oauthToken.ToString().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
        //            username = nameParts[0].ToString();
        //            password = nameParts[1].ToString();

        //            //await DisplayAlert("", "Toimii", "Close");

        //            if (!string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(username))
        //            {

        //                HttpClient client = new HttpClient();
        //                var uri = new Uri(string.Format("https://joonanmobiili.azurewebsites.net/api/login?username=" + username + "&password=" + password));

        //                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //                HttpResponseMessage response = await client.GetAsync(uri);
        //                if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
        //                {
        //        //            var errorMessage1 = response.Content.ReadAsStringAsync().Result.Replace("\\", "").Trim(new char[1]
        //        //            {
        //        //'"'
        //        //            });
        //        //            await DisplayAlert("", errorMessage1, "Close");
        //        //            //Toast.MakeText(this, errorMessage1, ToastLength.Long).Show();

        //                    await Navigation.PushAsync(new EmployeePage());
        //                }
        //                else
        //                {
        //                    var errorMessage1 = response.Content.ReadAsStringAsync().Result.Replace("\\", "").Trim(new char[1]
        //                    {
        //        '"'
        //                    });
        //                    await DisplayAlert("", errorMessage1, "Close");
        //                    //Toast.MakeText(this, errorMessage1, ToastLength.Long).Show();
        //                }
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        await DisplayAlert("AutoKirjautuminen", "oath token ei onnaa", "Close");
        //    }



            
        //    //await DisplayAlert("", "asdgasdg", "Close");
        //}
        protected async void AutoKirjautuminen()
        {
            string password = null;
            string username = null;
            try
            {
                var oauthToken = await SecureStorage.GetAsync("oauth_token1");

                if (oauthToken != null)
                {
                    string[] nameParts = oauthToken.ToString().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    username = nameParts[0].ToString();
                    password = nameParts[1].ToString();

                    if (!string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(username))
                    {

                        HttpClient client = new HttpClient();
                        var uri = new Uri(string.Format("https://joonanmobiili.azurewebsites.net/api/login?username=" + username + "&password=" + password));
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        HttpResponseMessage response = await client.GetAsync(uri);

                        if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
                        {
                            await Navigation.PushAsync(new EmployeePage());
                            await DisplayAlert("", "Tervetuloa takaisin "+username+"!", "Kiitos");
                        }
                        else
                        {
                            var errorMessage1 = response.Content.ReadAsStringAsync().Result.Replace("\\", "").Trim(new char[1]
                            {
                            '"'
                            });
                            await DisplayAlert("", errorMessage1, "Close");
                            
                        }
                    }
                }
                else
                {
                    ohitus = false;
                }
            }
            catch
            {
                await DisplayAlert("AutoKirjautuminen", "oath token ei onnaa", "Close");
            }
        }
    }
}