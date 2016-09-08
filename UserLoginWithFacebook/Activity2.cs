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
using Android.Graphics;
using System.Net;

namespace UserLoginWithFacebook
{
    [Activity(Label = "Activity2")]
    public class Activity2 : Activity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            var userName=UserData.userData["name"];
            SetContentView(Resource.Layout.second);
            TextView txt = FindViewById<TextView>(Resource.Id.textView1);
            txt.Text = userName;
            var userImage = UserData.userData["picture"]["data"]["url"];
            ImageView imageView = FindViewById<ImageView>(Resource.Id.demoImageView);
           var siva= GetImageBitmapFromUrl(userImage);
            imageView.SetImageBitmap(siva);
        }
        private Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }

            return imageBitmap;
        }
    }
}