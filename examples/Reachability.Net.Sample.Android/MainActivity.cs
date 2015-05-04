using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System;

namespace Reachability.Net.Sample.Android
{
	[Activity (Label = "Reachability.Net.Sample.Android", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		int count = 1;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

//			var reachability = new Reachability.Net.XamarinIOS.Reachability();

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			Button button = FindViewById<Button> (Resource.Id.myButton);
			TextView label = FindViewById<TextView>(Resource.Id.connectionStatusLabel);

			button.Click += delegate {

				var reachability = new Reachability.Net.XamarinAndroid.Reachability(this, "http://www.bing.com");
				var isConnected = reachability.IsHostReachable("www.google.com");
				var wifiStatus = reachability.LocalWifiConnectionStatus();
				var mobileConnStatus = reachability.InternetConnectionStatus();

				var connectionDetails = "Connection Status: " + (isConnected ? "Connected - " : "Disconnected - " + System.Environment.NewLine);
				connectionDetails += "Wifi Status: " + (wifiStatus == NetworkStatus.ConnectedViaWifi ? "Connected - " : "Disconnected - "+ System.Environment.NewLine);
				connectionDetails += "Mobile Status: " + (mobileConnStatus == NetworkStatus.ConnectedViaMobile ? "Connected" : "Disconnected");  

				label.Text = connectionDetails;
			};
		}
	}
}


