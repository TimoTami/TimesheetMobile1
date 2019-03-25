using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Locations;

namespace TimesheetMobile1.Droid
{
    [Activity(Label = "TimesheetMobile1", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity,ILocationListener
    {
        public static Android.Locations.LocationManager locationManager;
        public void OnLocationChanged(Location location)
        {
            TimesheetMobile1.Models.GpsLocationModel.Latitude = location.Latitude;
            TimesheetMobile1.Models.GpsLocationModel.Longitude = location.Longitude;
            TimesheetMobile1.Models.GpsLocationModel.Altitude = location.Altitude;
        }

        public void OnProviderDisabled(string provider)
        {
            //throw new NotImplementedException();
        }

        public void OnProviderEnabled(string provider)
        {
            //throw new NotImplementedException();
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
            //throw new NotImplementedException();
        }

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());

            try
            {
                locationManager = GetSystemService("location") as LocationManager;
                string Provider = LocationManager.GpsProvider;

                if (locationManager.IsProviderEnabled(Provider))
                {
                    locationManager.RequestLocationUpdates(Provider, 2000, 1, this);
                }
            }
            catch (Exception ex)
            {

                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
    }
}

