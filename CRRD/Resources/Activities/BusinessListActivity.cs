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
    /// Android Activity: Used for displaying the List of associated Businesses for the given Category and Subcategory, given as extras "categoryName" and "subcategoryName".
    /// </summary>
    /// <seealso cref="Android.App.Activity" />
    [Activity(Label = "@string/BusinessListActivityLabel", Icon = "@drawable/CSCLogo")]
    public class BusinessListActivity : AppCompatActivity
    {
        private ListView _ListView;
        private string _categoryName, _subcategoryName;
        private List<string> _businessList = new List<string>();

        // Start class to Get and parse the local XML file to the associated classes (Business & Category)
        private XMLHandler _handler = new XMLHandler();

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
            SupportActionBar.Title = Intent.GetStringExtra("subcategoryName") ?? this.ApplicationContext.GetString(Resource.String.ApplicationName);

            // Get Views
            _ListView = FindViewById<ListView>(Resource.Id.lvListArea);

            // Get the passed categoryName
            _categoryName = Intent.GetStringExtra("categoryName") ?? "No Data Found";
            _subcategoryName = Intent.GetStringExtra("subcategoryName") ?? "No Data Found";

            // Get unique List of all possible Businesses for the given categoryName & subcategoryName
            GetBusinessList(_categoryName, _subcategoryName);

            // Set the custom adapter
            BusinessListAdapter adapter = new BusinessListAdapter(this, _businessList);
            _ListView.Adapter = adapter;

            // Events ...
            _ListView.ItemClick += _ListView_ItemClick;
        }

        /// <summary>
        /// Gets the list of all possible business names for a given categoryName and subcategoryName combination key.
        /// </summary>
        /// <param name="categoryName">Name of the category.</param>
        /// <param name="subcategoryName">Name of the subcategory.</param>
        /// <returns>
        /// A sorted, uniqe list of all possible business names for the given categoryName and subcategoryName combination key.
        /// </returns>
        private List<string> GetBusinessList(string categoryName, string subcategoryName)
        {
            foreach (var b in _handler.BusinessList)
            {
                // A business may have n number of categories
                foreach (var c in b.CategoryList)
                {
                    if (c.Name == categoryName)
                    {
                        // A category may have n number of subcategories
                        foreach (var sc in c.SubcategoryList)
                        {
                            if (sc == subcategoryName)
                            {
                                _businessList.Add(b.Name);

                                
                            }
                        }
                    }
                }
            }
            _businessList = _businessList.Distinct().ToList();
            _businessList.Sort();

            return _businessList;
        }

        /// <summary>
        /// Handles the clicking of a specific item displayed as part of the ListView, starting BusinessDetailsActivity and passing the extras of "categoryName", "subcategoryName", and "businessName".
        /// </summary>
        /// <param name="sender">The object calling the function, the ListView.</param>
        /// <param name="e">The specific ListView item selected by the user.</param>
        private void _ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var intent = new Intent(this, typeof(BusinessDetailsActivity));
            intent.PutExtra("categoryName", _categoryName);
            intent.PutExtra("subcategoryName", _subcategoryName);
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
        /// Manages on-click actions when menu options are selected
        /// </summary>
        /// <param name="item">The menu</param>
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