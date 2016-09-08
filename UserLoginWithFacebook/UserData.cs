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
using System.Json;

namespace UserLoginWithFacebook
{
  public class UserData
    {
        public static JsonValue userData { get; set; }
    }
}