using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.V7.App;

namespace CRRD.Resources.Activities
{
    /// <summary>
    /// Android Activity: Used for displaying information about the app
    /// </summary>
    /// <seealso cref="Android.App.Activity" />
    [Activity(Label = "@string/ContactActivityLabel", Icon = "@drawable/CSCLogo")]
    public class ContactActivity : AppCompatActivity
    {
        /// <summary>
        /// Called on creation of the About Activity.
        /// </summary>
        /// <param name="bundle">The bundle, used for passing data between Activities.</param>
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Contact);

            // Set the toolbar
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = this.ApplicationContext.GetString(Resource.String.ContactActivityLabel);

            // Get resources
            LinearLayout _emailLayout = FindViewById<LinearLayout>(Resource.Id.layoutEmail);
            LinearLayout _websiteLayout = FindViewById<LinearLayout>(Resource.Id.layoutWebsite);
            LinearLayout _facebookLayout = FindViewById<LinearLayout>(Resource.Id.layoutFacebook);
            LinearLayout _twitterLayout = FindViewById<LinearLayout>(Resource.Id.layoutTwitter);

            // Set on-click events
            _emailLayout.Clickable = true;
            _emailLayout.Click += _email_Click;
            _websiteLayout.Clickable = true;
            _websiteLayout.Click += _website_Click;
            _facebookLayout.Clickable = true;
            _facebookLayout.Click += _facebook_Click;
            _twitterLayout.Clickable = true;
            _twitterLayout.Click += _twitter_Click;

        }

        /// <summary>
		/// Creates the menu for the Toolbar/Action Bar to use.
		/// </summary>
		/// <param name="menu">The menu.</param>
		public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Layout.Menu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        /// <summary>
        /// Manages on-click actions when menu options are selected.
        /// </summary>
        /// <param name="item">The menu.</param>
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId.Equals(Resource.Id.menu_home))
            {
                var intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
                return base.OnOptionsItemSelected(item);
            }
            else if (item.ItemId.Equals(Resource.Id.menu_about))
            {
                var intent = new Intent(this, typeof(AboutActivity));
                StartActivity(intent);
                return base.OnOptionsItemSelected(item);
            }
            else if (item.ItemId.Equals(Resource.Id.menu_contact))
            {
                var intent = new Intent(this, typeof(ContactActivity));
                StartActivity(intent);
                return base.OnOptionsItemSelected(item);
            }
            else
            {
                return base.OnOptionsItemSelected(item);
            }
        }

        /// <summary>
        /// Handles the on-click event for the CSC Email, opening the default email application on the user's device with the subject "Corvallis Reuse and Repair Directory".
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/>The instance containing the event data.</param>
        private void _email_Click(object sender, EventArgs e)
        {
            var email = new Intent(Android.Content.Intent.ActionSend);

            email.PutExtra(Android.Content.Intent.ExtraEmail,
            new string[] { this.ApplicationContext.GetString(Resource.String.CSCEmail) });
            email.PutExtra(Android.Content.Intent.ExtraSubject, "Corvallis Reuse and Repair Directory");

            email.SetType("message/rfc822");

            StartActivity(email);

        }

        /// <summary>
        /// Handles the on-click event for the CSC website, pulling up the website on the user's device's default browser.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/>The instance containing the event data.</param>
        private void _website_Click(object sender, EventArgs e)
        {

            var uri = Android.Net.Uri.Parse(this.ApplicationContext.GetString(Resource.String.CSCOnline));
            var intent = new Intent(Intent.ActionView, uri);
            StartActivity(intent);
        }

        /// <summary>
        /// Handles the Click event for the CSC Facebook link, pulling up the site on the user's device's default browser.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/>The instance containing the event data.</param>
        private void _facebook_Click(object sender, EventArgs e)
        {

            var uri = Android.Net.Uri.Parse(this.ApplicationContext.GetString(Resource.String.CSCFacebook));
            var intent = new Intent(Intent.ActionView, uri);
            StartActivity(intent);
        }

        /// <summary>
        /// Handles the Click event for the CSC Twitter link, pulling up the site in the user's device's default browser.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/>The instance containing the event data.</param>
        private void _twitter_Click(object sender, EventArgs e)
        {

            var uri = Android.Net.Uri.Parse(this.ApplicationContext.GetString(Resource.String.CSCTwitter));
            var intent = new Intent(Intent.ActionView, uri);
            StartActivity(intent);
        }

    }
}