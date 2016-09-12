using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Auth;
using System.Threading.Tasks;
using System.Json;

namespace UserLoginWithFacebook
{
    [Activity(Label = "UserLoginWithFacebook", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            var Twitter = FindViewById<Button>(Resource.Id.TwitterButton);
            Twitter.Click += delegate { LoginToTwitter(); };

            var facebook = FindViewById<Button>(Resource.Id.FacebookButton);
            facebook.Click += delegate { LoginToFacebook(true); };

            var facebookNoCancel = FindViewById<Button>(Resource.Id.FacebookButtonNoCancel);
            facebookNoCancel.Click += delegate { LoginToFacebook(false); };
            
        }
        //Login with Twitter
        private void LoginToTwitter()
        {
            var auth = new OAuth1Authenticator(
            Constants.CustomerKey,
             Constants.CustomerSecret,
             new Uri("https://api.twitter.com/oauth/request_token"),  
           new Uri("https://api.twitter.com/oauth/authorize"),  
             new Uri("https://api.twitter.com/oauth/access_token"),  
            new Uri("http://mobile.twitter.com")  

            );

            auth.Completed += twitter_auth_Completed;
            StartActivity(auth.GetUI(this));
        }

        private async void twitter_auth_Completed(object sender, AuthenticatorCompletedEventArgs eventArgs)
        {
            if (eventArgs.IsAuthenticated)
            {
                Toast.MakeText(this, "Authenticated.!!", ToastLength.Long).Show();
                //use the account object and make the desired API call
                var request = new OAuth1Request(
                                      "GET",
                                      new Uri("https://api.twitter.com/1.1/account/verify_credentials.json"),
                                      null,
                                      eventArgs.Account);

                await request.GetResponseAsync().ContinueWith(t =>
                {
                    var resString = t.Result.GetResponseText();
                    var data = JsonValue.Parse(resString);

                  //  UserData.userData = data;
                });
            }
        }
        void LoginToFacebook(bool allowCancel)
        {
            var auth = new OAuth2Authenticator(
                clientId: Constants.AppID,
                scope: Constants.Scope,
                authorizeUrl: new Uri(Constants.FacebookRestAPI),
                redirectUrl: new Uri(Constants.LoginSuccessful));

            auth.AllowCancel = allowCancel;
            // If authorization succeeds or is canceled, .Completed will be fired.
            auth.Completed += (s, EventArgs) => {
                if (!EventArgs.IsAuthenticated)
                {
                    var builder = new AlertDialog.Builder(this);
                    builder.SetMessage(Constants.ErrorMessage);
                    builder.SetPositiveButton(Constants.OK, (o, e) => { });
                    builder.Create().Show();
                    return;
                }

                // Now that we're logged in, make a OAuth2 request to get the user's info.
                var request = new OAuth2Request("GET", new Uri("https://graph.facebook.com/me?fields=id,name,picture.type(large)"), null, EventArgs.Account);
                request.GetResponseAsync().ContinueWith(t => {
                    var builder = new AlertDialog.Builder(this);
                    if (t.IsFaulted)
                    {
                        builder.SetTitle("Error");
                        builder.SetMessage(t.Exception.Flatten().InnerException.ToString());
                    }
                    else if (t.IsCanceled)
                        builder.SetTitle("Task Canceled");
                    else
                    {
                        var obj = JsonValue.Parse(t.Result.GetResponseText());
                        UserData.userData = obj;
                        builder.SetTitle("Logged in");
                        builder.SetMessage("Name: " + obj["name"]);
                        StartActivity(typeof(Activity2));
                    }

                    builder.SetPositiveButton("Ok", (o, e) => { });
                    builder.Create().Show();
                }, UIScheduler);
            };

            StartActivity(auth.GetUI(this));
        }

        private static readonly TaskScheduler UIScheduler = TaskScheduler.FromCurrentSynchronizationContext();
    }
}

