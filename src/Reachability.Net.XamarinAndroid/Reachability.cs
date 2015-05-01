using Android.App;
using Android.Content;
using Android.Net;
using Java.Net;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Reachability.Net;

namespace Reachability.Net.XamarinAndroid
{
	public class Reachability : IReachability
	{
		public static string HostName = "http://www.bing.com";
		readonly ConnectivityManager _connectivityManager;
		readonly Context _context;
		readonly int _connectionTimeOutInMillisec = 3000;

		public Reachability(Context context, string host = "http://www.bing.com")
		{
			this._context = context;
			HostName = host;
			_connectivityManager = (ConnectivityManager)_context.GetSystemService(Context.ConnectivityService);
		}

		#region IReachability implementation

		public bool IsHostReachable (string host = null)
		{
			host = host ?? HostName;
			var isConnected = false;
			var activeConnection = _connectivityManager.ActiveNetworkInfo;
			if ((activeConnection != null)  && activeConnection.IsConnected)
			{
				try 
				{
					var task = Task.Factory.StartNew(() => 
					{
						URL url = new URL(HostName);
			            HttpURLConnection urlc = (HttpURLConnection) url.OpenConnection();
			            urlc.SetRequestProperty("User-Agent", "Android Application");
			            urlc.SetRequestProperty("Connection", "close");
			            urlc.ConnectTimeout = _connectionTimeOutInMillisec;
			            urlc.Connect();
			            isConnected = (urlc.ResponseCode == HttpStatus.Ok);
						isConnected = true;
					});
					task.Wait();
		        } 
		        catch (Exception e) 
		        {
					System.Diagnostics.Trace.WriteLine("Connectivity issue: " + e.ToString());
		        }			  	
			}
			return isConnected;
		}

		public NetworkStatus RemoteHostStatus ()
		{
			var networkStatus = InternetConnectionStatus ();
			if (networkStatus == NetworkStatus.Disconnected)
				return NetworkStatus.Disconnected;


			return networkStatus;
		}

		public NetworkStatus InternetConnectionStatus ()
		{
			if (_connectivityManager.ActiveNetworkInfo == null)
				return NetworkStatus.Disconnected;
			if (!_connectivityManager.ActiveNetworkInfo.IsConnected)
				return NetworkStatus.Disconnected;

			if (_connectivityManager.ActiveNetworkInfo.Type == ConnectivityType.Wifi && IsHostReachable())
				return NetworkStatus.ConnectedViaWifi;
			if (_connectivityManager.ActiveNetworkInfo.Type == ConnectivityType.Mobile && IsHostReachable())
				return NetworkStatus.ConnectedViaMobile;

			return NetworkStatus.Disconnected;
		}

		public NetworkStatus LocalWifiConnectionStatus ()
		{
			if (_connectivityManager.ActiveNetworkInfo == null)
				return NetworkStatus.Disconnected;

			if (_connectivityManager.ActiveNetworkInfo.Type == ConnectivityType.Wifi && IsHostReachable())
				return NetworkStatus.ConnectedViaWifi;

			return NetworkStatus.Disconnected;
		}

		#endregion
	}
}

