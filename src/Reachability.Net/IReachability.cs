using System;

namespace Reachability.Net
{
	public interface IReachability
	{
		bool IsHostReachable(string hostName);
		NetworkStatus RemoteHostStatus ();
		NetworkStatus InternetConnectionStatus ();
		NetworkStatus LocalWifiConnectionStatus ();
	}
}

