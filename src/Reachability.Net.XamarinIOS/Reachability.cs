using System;
using System.Net;
using CoreFoundation;
using SystemConfiguration;
using Reachability.Net;

namespace Reachability.Net.XamarinIOS
{
	public class Reachability : IReachability
	{
		public Reachability(string hostName = "www.bing.com")
		{
			HostName = hostName;
		}

		NetworkReachability _remoteHostReachability;

		private static string HostName = "www.bing.com";

		public NetworkStatus RemoteHostStatus ()
		{
			NetworkReachabilityFlags flags;
			bool reachable;

			using (_remoteHostReachability = new NetworkReachability (HostName))
			{
				// Need to probe before we queue, or we wont get any meaningful values
				// this only happens when you create NetworkReachability from a hostname
				reachable = _remoteHostReachability.TryGetFlags (out flags);

				_remoteHostReachability.SetNotification (OnChange);
				_remoteHostReachability.Schedule (CFRunLoop.Current, CFRunLoop.ModeDefault);

				if (!reachable)
				{
					return NetworkStatus.Disconnected;
				}

				if ((int)flags != 0 && !IsReachableWithoutRequiringConnection (flags))
				{
					return NetworkStatus.Disconnected;
				}
			}

			return (flags & NetworkReachabilityFlags.IsWWAN) != 0 ? NetworkStatus.ConnectedViaMobile : NetworkStatus.ConnectedViaWifi;
		}

		public NetworkStatus InternetConnectionStatus ()
		{
			NetworkReachabilityFlags flags;
			bool defaultNetworkAvailable = IsNetworkAvailable (out flags);

			if (defaultNetworkAvailable && ((flags & NetworkReachabilityFlags.IsDirect) != 0))
				return NetworkStatus.Disconnected;

			if ((flags & NetworkReachabilityFlags.IsWWAN) != 0)
				return NetworkStatus.ConnectedViaMobile;

			return flags == 0 ? NetworkStatus.Disconnected : NetworkStatus.ConnectedViaWifi;
		}

		public NetworkStatus LocalWifiConnectionStatus ()
		{
			NetworkReachabilityFlags flags;
			if (IsAdHocWiFiNetworkAvailable (out flags)){
				if ((flags & NetworkReachabilityFlags.IsDirect) != 0)
					return NetworkStatus.ConnectedViaWifi;
			}
			return NetworkStatus.Disconnected;
		}

		// Is the host reachable with the current network configuration
		public bool IsHostReachable (string host)
		{
			if (string.IsNullOrEmpty (host))
				return false;

			using (var r = new NetworkReachability (host)){
				NetworkReachabilityFlags flags;

				if (r.TryGetFlags (out flags)){
					return IsReachableWithoutRequiringConnection (flags);
				}
			}
			return false;
		}

		public static bool IsReachableWithoutRequiringConnection (NetworkReachabilityFlags flags)
		{
			// Is it reachable with the current network configuration?
			bool isReachable = (flags & NetworkReachabilityFlags.Reachable) != 0;

			// Do we need a connection to reach it?
			bool noConnectionRequired = (flags & NetworkReachabilityFlags.ConnectionRequired) == 0 || (flags & NetworkReachabilityFlags.IsWWAN) != 0;

			// Since the network stack will automatically try to get the WAN up,
			// probe that

			return isReachable && noConnectionRequired;
		}

		// Raised every time there is an interesting reachable event, 
		// we do not even pass the info as to what changed, and 
		// we lump all three status we probe into one
		//
		public static event EventHandler ReachabilityChanged;

		static void OnChange (NetworkReachabilityFlags flags)
		{
			var h = ReachabilityChanged;
			if (h != null)
				h (null, EventArgs.Empty);
		}

		//
		// Returns true if it is possible to reach the AdHoc WiFi network
		// and optionally provides extra network reachability flags as the
		// out parameter
		//
		static NetworkReachability adHocWiFiNetworkReachability;
		public static bool IsAdHocWiFiNetworkAvailable (out NetworkReachabilityFlags flags)
		{
			if (adHocWiFiNetworkReachability == null){
				adHocWiFiNetworkReachability = new NetworkReachability (new IPAddress (new byte [] {169,254,0,0}));
				adHocWiFiNetworkReachability.SetNotification (OnChange);
				adHocWiFiNetworkReachability.Schedule (CFRunLoop.Current, CFRunLoop.ModeDefault);
			}

			return adHocWiFiNetworkReachability.TryGetFlags (out flags) && IsReachableWithoutRequiringConnection (flags);
		}

		static NetworkReachability defaultRouteReachability;
		static bool IsNetworkAvailable (out NetworkReachabilityFlags flags)
		{
			if (defaultRouteReachability == null){
				defaultRouteReachability = new NetworkReachability (new IPAddress (0));
				defaultRouteReachability.SetNotification (OnChange);
				defaultRouteReachability.Schedule (CFRunLoop.Current, CFRunLoop.ModeDefault);
			}
			return defaultRouteReachability.TryGetFlags (out flags) && IsReachableWithoutRequiringConnection (flags);
		}	
	}
}

