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

namespace TimesheetMobile1
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EmployeePage : ContentPage
	{
		public EmployeePage ()
		{
			InitializeComponent ();

            employeeList.ItemsSource = new string[] { "" };
            employeeList.ItemSelected += EmployeeList_ItemSelected;
            
            
        }
        public async void LoadEmployees(object sender, EventArgs e)
        {

            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://joonanmobiili.azurewebsites.net");
                string json = await client.GetStringAsync("/api/employee");
                string[] employees = JsonConvert.DeserializeObject<string[]>(json);

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
                    client.BaseAddress = new Uri("http://joonanmobiili.azurewebsites.net");
                    string json = await client.GetStringAsync("/api/employee?employeeName="+ employee);
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
                Navigation.PushAsync(new WorkassignmentPage());
            }
        }
        public async void Logout(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoginPage());
        }
    }
}