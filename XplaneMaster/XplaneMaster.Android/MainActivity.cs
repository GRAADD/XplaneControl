using System;
using System.Collections.Generic;
using Android;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Xamarin.Forms;
//using Application = Android.App.Application;

namespace XplaneMaster.Droid
{
    [Activity(Label = "XplaneMaster", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        string[] PermissionsArray = null;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
            updateNonGrantedPermissions();
            //AppCenter.Start("f9622183-2f2a-4d7a-b877-68451210acfa",
            //    typeof(Analytics), typeof(Crashes));updateNonGrantedPermissions();

            try
            {
                if (PermissionsArray != null && PermissionsArray.Length > 0)
                {
                    if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
                    {
                        ActivityCompat.RequestPermissions(this, PermissionsArray, 0);
                    }
                }
            }
            catch (Exception)
            {

            }

        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        private void updateNonGrantedPermissions()
        {
            try
            {
                List<string> PermissionList = new List<string>();
                PermissionList.Add(Manifest.Permission.MediaContentControl);
                if (ContextCompat.CheckSelfPermission(ApplicationContext, Manifest.Permission.RecordAudio) != (int)Android.Content.PM.Permission.Granted)
                {
                    PermissionList.Add(Manifest.Permission.RecordAudio);
                }
                if (ContextCompat.CheckSelfPermission(ApplicationContext, Manifest.Permission.WriteExternalStorage) != (int)Android.Content.PM.Permission.Granted)
                {
                    PermissionList.Add(Manifest.Permission.WriteExternalStorage);
                }
                if (ContextCompat.CheckSelfPermission(ApplicationContext, Manifest.Permission.ReadPhoneState) != (int)Android.Content.PM.Permission.Granted)
                {
                    PermissionList.Add(Manifest.Permission.ReadPhoneState);
                }
                PermissionsArray = new string[PermissionList.Count];
                for (int index = 0; index < PermissionList.Count; index++)
                {
                    PermissionsArray.SetValue(PermissionList[index], index);
                }
            }
            catch (Exception)
            {

            }
        }

    }
}