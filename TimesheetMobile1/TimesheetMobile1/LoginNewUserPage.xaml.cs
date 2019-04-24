using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TimesheetMobile1.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimesheetMobile1
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginNewUserPage : ContentPage
	{

        public LoginNewUserPage ()
		{
			InitializeComponent ();
		}
        public async void NewUser(object sender, EventArgs e)
        {
            string passisana = "";
            if (UusiSana.Text == UusiSanaUudestaan.Text)
            {
                try
                {
                    using (var sha = SHA256.Create())
                    {
                        var bytes = Encoding.UTF8.GetBytes(UusiSana.Text);
                        var hash = sha.ComputeHash(bytes);

                        passisana = Convert.ToBase64String(hash);
                    }

                    NewUserModel data = new NewUserModel()
                    {
                        firstname = ENimi.Text,
                        lastname = SNimi.Text,
                        phonenumber = PNum.Text,
                        email = SPos.Text,
                        username = UusiTunnus.Text,
                        password = passisana
                    };


                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri("https://joonanmobiili.azurewebsites.net/");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var input = JsonConvert.SerializeObject(data);
                    var content = new StringContent(input, Encoding.UTF8, "application/json");
                    HttpResponseMessage Xamarin_reg = await client.PostAsync("/api/login", content);
                    if (Xamarin_reg.StatusCode == System.Net.HttpStatusCode.Accepted)
                    {
                        var errorMessage1 = Xamarin_reg.Content.ReadAsStringAsync().Result.Replace("\\", "").Trim(new char[1]
                        {
                        '"'
                        });
                        await DisplayAlert("", errorMessage1, "Close");
                        await Navigation.PushAsync(new LoginPage());
                    }
                    else
                    {
                        var errorMessage1 = Xamarin_reg.Content.ReadAsStringAsync().Result.Replace("\\", "").Trim(new char[1]
                        {
                        '"'
                        });
                        await DisplayAlert("", errorMessage1, "Close");
                    }

                }

                catch (Exception)
                {
                    await DisplayAlert("Alert", "Täytä kaikki kentät", "Close");

                }

            }
            else
            {
                await DisplayAlert("Alert", "Salasanat ei täsmää", "Close");
            }

        }
    }
}