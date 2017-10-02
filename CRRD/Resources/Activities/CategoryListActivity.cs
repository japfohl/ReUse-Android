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
    /// Android Activity: Used for displaying the List of Categories a user may select.
    /// </summary>
    /// <seealso cref="Android.App.Activity" />
    [Activity(Label = "@string/CategoryListActivityLabel", Icon = "@drawable/CSCLogo")]
    public class CategoryListActivity : AppCompatActivity
    {
        private ListView _ListView;
        private List<string> _categoryListStrings;
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

            // Set our view from the layout resource
             SetContentView(Resource.Layout.ListCategory);

            // Set the toolbar
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = this.ApplicationContext.GetString(Resource.String.CategoryListActivityLabel);

            _ListView = FindViewById<ListView>(Resource.Id.lvListArea);
            
            // Get a sorted Unique List<sring> of Categories
            _categoryListStrings = new List<string>();
            
            foreach (var c in _handler.CategoryList)
            {
                _categoryListStrings.Add(c.Name);
            }
            
            _categoryListStrings = _categoryListStrings.Distinct().ToList();
            _categoryListStrings.Sort();

            // Remove the Repair subcategory from _categoryListStrings 
            _categoryListStrings.Remove(this.ApplicationContext.GetString(Resource.String.RepairCategoryName));

            // Set the custom adapter
            CategoryListAdapter adapter = new CategoryListAdapter(this, _categoryListStrings);
            _ListView.Adapter = adapter;

            // Set the ListView on-click function
            _ListView.ItemClick += _ListView_ItemClick;
        
        }

        /// <summary>
        /// Handles the clicking of a category name, starting the intent SubcategoryListActivity and passing the extra "categoryName".
        /// </summary>
        /// <param name="sender">The sending object.</param>
        /// <param name="e">The specific item selected by the user.</param>
        private void _ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            
            var intent = new Intent(this, typeof(SubcategoryListActivity));
            intent.PutExtra("categoryName", _categoryListStrings[e.Position]);
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