using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.V7.App;

namespace CRRD.Resources.Activities
{
    /// <summary>
    /// Android Activity: Used for displaying information about the purpose of the app
    /// </summary>
    /// <seealso cref="Android.App.Activity" />
    [Activity(Label = "@string/AboutActivityLabel", Icon = "@drawable/CSCLogo")]
    public class AboutActivity : AppCompatActivity
    {
        /// <summary>
        /// Called on creation of the About Activity.
        /// </summary>
        /// <param name="bundle">The bundle, used for passing data between Activities.</param>
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.About);

            // Set the toolbar
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = this.ApplicationContext.GetString(Resource.String.AboutActivityLabel);

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