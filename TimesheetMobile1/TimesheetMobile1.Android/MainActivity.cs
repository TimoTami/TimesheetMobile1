using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Locations;
using Android.Support.V4.Content;
using Android;
using System.Collections.Generic;

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

            //...
            
            Xamarin.Essentials.Platform.Init(this, bundle); // add this line to your code, it may also be called: bundle
                                                                        //...

            global::Xamarin.Forms.Forms.Init(this, bundle);

            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) != Permission.Granted)
            {
                List<string> permission = new List<string>();
                permission.Add(Manifest.Permission.AccessFineLocation);

                if (permission.Count > 0)
                {
                    string[] array = permission.ToArray();

                    RequestPermissions(array, array.Length);
                }
            }

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
            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

