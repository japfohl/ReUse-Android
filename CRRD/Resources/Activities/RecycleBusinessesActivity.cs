using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using System.Collections.Generic;
using System.Linq;
using CRRD.Resources.Adapters;
using CRRD.Resources.Data;
using Android.Support.V7.App;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Views;

namespace CRRD.Resources.Activities
{
    /// <summary>
    /// Android Activity: Used for displaying the local recycling facilities.
    /// </summary>
    /// <seealso cref="Android.App.Activity" />
    [Activity(Label = "@string/RecyclingInfoActivityLabel", Icon = "@drawable/CSCLogo")]
    public class RecycleBusinessesActivity : AppCompatActivity
    {
        private ListView _ListView;
        private List<string> _businessList = new List<string>();
        private RecycleXMLHandler _handler = new RecycleXMLHandler();

        /// <summary>
        /// Called when [create].
        /// </summary>
        /// <param name="bundle">The bundle.</param>
        protected override void OnCreate(Bundle bundle)
        {
            // Check if _handler has Categories and Businesses initialized
            ErrorCheckActivity.checkXMLHandlerInitialization(this.ApplicationContext, _handler.isInitialized);

            base.OnCreate(bundle);
            SetContentView(Resource.Layout.ListBusiness);

            // Set the toolbar
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = this.ApplicationContext.GetString(Resource.String.RecyclingInfoActivityLabel);

            // Getting Views
            _ListView = FindViewById<ListView>(Resource.Id.lvListArea);


            // Get unique List of all possible Businesses 
            GetBusinessList();

            // Set the custom adapter
            BusinessListAdapter adapter = new BusinessListAdapter(this, _businessList);
            _ListView.Adapter = adapter;

            // Set the ListView on-click function
            _ListView.ItemClick += _ListView_ItemClick;
        }

        /// <summary>
        /// Gets the list of business names from _handler.
        /// </summary>
        /// <returns>
        /// A sorted, uniqe list of all business names in _handler.
        /// </returns>
        private List<string> GetBusinessList()
        {
            foreach (var b in _handler.BusinessList)
            {
                    _businessList.Add(b.Name);

            }
            _businessList = _businessList.Distinct().ToList();
            _businessList.Sort();

            return _businessList;
        }

        /// <summary>
        /// Handles the on-click activity for the ListView.
        /// </summary>
        /// <param name="sender">The sending object.</param>
        /// <param name="e">The specific item selected by the user.</param>
        private void _ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var intent = new Intent(this, typeof(BusinessDetailsActivity));
            intent.PutExtra("businessName", _businessList[e.Position]);
            StartActivity(intent);
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