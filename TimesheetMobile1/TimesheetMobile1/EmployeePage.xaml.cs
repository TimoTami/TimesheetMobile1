using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using System.IO;
using Xamarin.Essentials;
using TimesheetMobile1.Models;
using System.Net.Http.Headers;

namespace TimesheetMobile1

//Logout pitää muuttaa niin, että ei käytä tokenia vaan employeeListiä ja sitten vois ehkä tehdä myös erillisen osion adminille, jossa näkyy kaikki employeet
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmployeePage : ContentPage
    {
        
        public string[] employees;
        public EmployeePage()
        {
            InitializeComponent();
            employeeList.ItemsSource = new string[] { "" };
            employeeList.ItemSelected += EmployeeList_ItemSelected;
            LoadEmployees();

        }
        //public async void LoadEmployees(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        HttpClient client = new HttpClient();
        //        client.BaseAddress = new Uri("http://joonanmobiili.azurewebsites.net");
        //        string json = await client.GetStringAsync("/api/employee");
        //        employees = JsonConvert.DeserializeObject<string[]>(json);

        //        employeeList.ItemsSource = employees;
        //    }
        //    catch (Exception ex)
        //    {
        //        string errorMessage = ex.GetType().Name + ": " + ex.Message;
        //        employeeList.ItemsSource = new string[] { errorMessage };
        //    }
        //}
        public async void LoadEmployees()
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("https://joonanmobiili.azurewebsites.net");
                string json = await client.GetStringAsync("/api/employee");
                employees = JsonConvert.DeserializeObject<string[]>(json);

                employeeList.ItemsSource = employees;
            }
            catch (Exception ex)
            {
                string errorMessage = ex.GetType().Name + ": " + ex.Message;
                employeeList.ItemsSource = new string[] { errorMessage };
            }
        }

        private async void EmployeeList_ItemSelected(object sender,
            SelectedItemChangedEventArgs e)
        {
            string employee = employeeList.SelectedItem?.ToString();
            if (!string.IsNullOrEmpty(employee))
            {
                try
                {
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri("https://joonanmobiili.azurewebsites.net");
                    string json = await client.GetStringAsync("/api/employee?employeeName=" + employee);
                    byte[] imageBytes = JsonConvert.DeserializeObject<byte[]>(json);

                    employeeImage.Source = ImageSource.FromStream(
                        () => new MemoryStream(imageBytes));
                }
                catch (Exception ex)
                {
                    string errorMessage = ex.GetType().Name + ": " + ex.Message;
                    employeeList.ItemsSource = new string[] { errorMessage };
                }
            }
        }

        public async void LoadWorkassignments(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new WorkassignmentPage());
        }
        private async void ListWorkAssignments(object sender, EventArgs e)
        {
            string employee = employeeList.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(employee))
            {
                await DisplayAlert("List Work", "You must select employee first.", "OK");
            }
            else
            {
                await Navigation.PushAsync(new WorkassignmentPage());
            }
        }
        public async void Logout(object sender, EventArgs e)
        {
            SecureStorage.RemoveAll();
            await Navigation.PushAsync(new LoginPage());

        }
        protected override bool OnBackButtonPressed()
        {
            Takas();
            return true;
            
        }
        async void Takas()
        {
            await Navigation.PushAsync(new LoginPage());
            
        }


    }


}