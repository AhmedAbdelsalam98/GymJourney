using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Plugin.CurrentActivity;

//=============================================
// Reference A3: externally sourced algorithm
// Purpose: This class has methods that take advantage of camera service plugins (Plugin.CurrentActivity Initialization).
// Date: 8/11/2020
// Source: The step by step set up of Media Plugin for Xamarin.Forms!
// Author: Udara Alwis
// url: https://theconfuzedsourcecode.wordpress.com/2020/01/28/the-step-by-step-set-up-of-media-plugin-for-xamarin-forms/
//=============================================

namespace GymJourney.Droid
{
#if DEBUG
	[Application(Debuggable = true)]
#else
	[Application(Debuggable = false)]
#endif
	public class MainApplication : Application
	{
		public MainApplication(IntPtr handle, JniHandleOwnership transer)
		  : base(handle, transer)
		{
		}

		public override void OnCreate()
		{
			base.OnCreate();
			CrossCurrentActivity.Current.Init(this);
		}
	}
}
//=============================================
// End reference A3
//=============================================