using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using CRRD.Resources.Data;
using System.Collections.Generic;
using System.Linq;
using CRRD.Resources.Adapters;
using Android.Support.V7.App;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Views;

namespace CRRD.Resources.Activities
{
    /// <summary>
    /// Android Activity: Used for displaying the List of Subcategories in the category given as the extra "categoryName" that the user may select.
    /// </summary>
    /// <seealso cref="Android.App.Activity" />
    [Activity(Label = "@string/SubcategoryListActivityLabel", Icon = "@drawable/CSCLogo")]
    public class SubcategoryListActivity : AppCompatActivity
    {
        private ListView _ListView;
        private List<string> _SubcategoryList;
        private string _categoryName;
        private XMLHandler _handler = new XMLHandler();

        /// <summary>
        /// Called when [create].
        /// </summary>
        /// <param name="bundle">The bundle, used to pass information between activities.</param>
        protected override void OnCreate(Bundle bundle)
        {
            // Check if _handler has Categories and Businesses initialized
            ErrorCheckActivity.checkXMLHandlerInitialization(this.ApplicationContext, _handler.isInitialized);

            base.OnCreate(bundle);
            SetContentView(Resource.Layout.ListSubcategory);

            // Set the toolbar
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = Intent.GetStringExtra("categoryName") ?? this.ApplicationContext.GetString(Resource.String.SubcategoryListActivityLabel);

            _ListView = FindViewById<ListView>(Resource.Id.lvListArea);

            // Get the passed categoryName
            _categoryName = Intent.GetStringExtra("categoryName") ?? "No Data Found";

            // Get unique List of all possible subcategories for the given categoryName
            _SubcategoryList = GetSubcategoryListUnique(_categoryName);

            // Set the custom adapter
            SubcategoryListAdapter adapter = new SubcategoryListAdapter(this, _SubcategoryList);
            _ListView.Adapter = adapter;

            // Set the ListView on-click function.
            _ListView.ItemClick += _ListView_ItemClick;
        }

        /// <summary>
        /// Gets a unique List of all possible subcategories for the given categoryName.
        /// </summary>
        /// <param name="categoryName">The name of the category.</param>
        /// <returns>The sorted, unique list of subcategories for the given categoryName.</returns>
        private List<string> GetSubcategoryListUnique(string categoryName)
        {
            List<string> subcategoryList = new List<string>();

            foreach (var c in _handler.CategoryList)
            {
                if (c.Name == categoryName)
                {
                    // A category may have n number of subcategories
                    foreach (var sc in c.SubcategoryList)
                    {
                        subcategoryList.Add(sc);

                    }
                    break;
                }
            }
            subcategoryList = subcategoryList.Distinct().ToList();
            subcategoryList.Sort();

            return subcategoryList;
        }

        /// <summary>
        /// Handles the on-click activity for the ListView, starting BusinessListActivity and passing the extras "categoryName" and "subcategoryName".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var intent = new Intent(this, typeof(BusinessListActivity));
            intent.PutExtra("categoryName", _categoryName);
            intent.PutExtra("subcategoryName", _SubcategoryList[e.Position]);
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