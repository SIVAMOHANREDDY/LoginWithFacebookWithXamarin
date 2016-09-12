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

namespace UserLoginWithFacebook
{
    class Constants
    {
        // APP Requirements
        public static string AppID = "332063047134905";
        public static string Scope = "public_profile";
        public static string CustomerKey = "1Qcd641An54lsJbao0jWgo26P";
        public static string CustomerSecret = "NCcfQSCjGmVCtGKDpNnm5FUTADppBnryRirQ2vaOccEqJgg5S0";
        
        // API Definitions
        public static string FacebookRestAPI = "https://m.facebook.com/dialog/oauth/";
        public static string LoginSuccessful = "http://www.facebook.com/connect/login_success.html";

        // Response Message
        public static string ErrorMessage = "Not Authenticated";

        public static string OK = "Ok";


    }
}