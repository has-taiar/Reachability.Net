using System;
using System.Drawing;

using Foundation;
using UIKit;

namespace Reachability.Net.Sample
{
	public partial class Reachability_Net_SampleViewController : UIViewController
	{
		public Reachability_Net_SampleViewController (IntPtr handle) : base (handle)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		#region View lifecycle

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			// Perform any additional setup after loading the view, typically from a nib.
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
		}

		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
		}


		partial void UIButton5_TouchUpInside (UIButton sender)
		{
			var reachability = new Reachability.Net.XamarinIOS.Reachability();
			var isConnected = reachability.IsHostReachable("www.google.com");
			var wifiStatus = reachability.LocalWifiConnectionStatus();
			var mobileConnStatus = reachability.InternetConnectionStatus();

			var connectionDetails = "Connection Status: " + (isConnected ? "Connected - " : "Disconnected - " + Environment.NewLine);
			connectionDetails += "Wifi Status: " + (wifiStatus == NetworkStatus.ConnectedViaWifi ? "Connected - " : "Disconnected - "+ Environment.NewLine);
			connectionDetails += "Mobile Status: " + (mobileConnStatus == NetworkStatus.ConnectedViaMobile ? "Connected - " : "Disconnected - " + Environment.NewLine);  

			ConnectionStatusLabel.Text = connectionDetails;
			var alert = new UIAlertView("Reachability.Net", connectionDetails, null, "OK", null);
			alert.Show();
		}
		#endregion
	}
}

