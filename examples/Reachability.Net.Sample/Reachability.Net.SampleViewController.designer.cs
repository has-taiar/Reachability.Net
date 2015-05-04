// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Reachability.Net.Sample
{
	[Register ("Reachability_Net_SampleViewController")]
	partial class Reachability_Net_SampleViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton CheckButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextView ConnectionStatusLabel { get; set; }

		[Action ("UIButton5_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void UIButton5_TouchUpInside (UIButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (CheckButton != null) {
				CheckButton.Dispose ();
				CheckButton = null;
			}
			if (ConnectionStatusLabel != null) {
				ConnectionStatusLabel.Dispose ();
				ConnectionStatusLabel = null;
			}
		}
	}
}
