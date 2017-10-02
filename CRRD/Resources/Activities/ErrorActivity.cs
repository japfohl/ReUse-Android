using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace CRRD.Resources.Activities
{
    /// <summary>
    /// Android Activity: Used for displaying an error message, either one in the extra "errorMessage" or the "errorUnrecognized" string.
    /// </summary>
    /// <seealso cref="Android.App.Activity" />
    [Activity(Label = "@string/ErrorActivityLabel", Icon = "@drawable/CSCLogo")]
    public class ErrorActivity : AppCompatActivity
    {
        private string _errorMessage;
        private TextView _errorMessageShown;

        /// <summary>
        /// Called when [create].
        /// </summary>
        /// <param name="bundle">The bundle, used to pass data between Activities.</param>
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.AppError);

            // Set the toolbar
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = this.ApplicationContext.GetString(Resource.String.ErrorActivityLabel);

            // Get the passed error message
            _errorMessage = Intent.GetStringExtra("errorMessage") ?? "@string/errorUnrecognized";

            //set the TextView to the error message passed in
            _errorMessageShown = FindViewById<TextView>(Resource.Id.errorMessage);
            _errorMessageShown.Text = _errorMessage;
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

    }

}