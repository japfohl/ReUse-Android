
using Android.App;
using Android.Content;
using Android.OS;
using Android.Webkit;
using Android.Support.V7.App;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Views;



namespace CRRD.Resources.Activities
{
    /// <summary>
    /// Android Activity: Used for displaying a local application view of a requested web page with the URL given as the extra "PDF_URI" and the page title given as "PDF_Title".
    /// </summary>
    /// <seealso cref="Android.App.Activity" />
    [Activity(Label = "@string/WebViewerActivityLabel", Icon = "@drawable/CSCLogo")]
    public class WebViwerActivity : AppCompatActivity
    {
        private string GDOCS_VIEWER = "https://docs.google.com/gview?embedded=true&url=";
        private string PDF_URI;
        private string PDF_Title;
        private WebView myWebView;

        /// <summary>
        /// Called when [create].
        /// </summary>
        /// <param name="bundle">The bundle, used to pass data between activities.</param>
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Get Passed Data
            PDF_URI = Intent.GetStringExtra("PDF_URI") ?? "Data not available";
            PDF_Title = Intent.GetStringExtra("PDF_Title") ?? this.ApplicationContext.GetString(Resource.String.WebViewerActivityLabel);

            // Set our view from the layout resource
            SetContentView(Resource.Layout.WebViewer);

            // Set the toolbar
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = PDF_Title ?? this.ApplicationContext.GetString(Resource.String.WebViewerActivityLabel);

            // Get view Elements
            myWebView = FindViewById<WebView>(Resource.Id.objWebView);

            // Display link, either using WebViewer if the document is a pdf, (probably), or opening it in the user's device's default browser otherwise, (since there were difficulties opening pdfs in-browser).
            if(PDF_URI.Length > 3 && PDF_URI[PDF_URI.Length - 3] == 'p' && PDF_URI[PDF_URI.Length - 2] == 'd' && PDF_URI[PDF_URI.Length - 1] == 'f')
            {
                myWebView.Settings.JavaScriptEnabled = true;
                myWebView.LoadUrl(GDOCS_VIEWER + PDF_URI);
                myWebView.SetWebViewClient(new WebClient());
            }
            else if (PDF_URI.Length > 3)
            {
                var uri = Android.Net.Uri.Parse(PDF_URI);
                var intent = new Intent(Intent.ActionView, uri);
                StartActivity(intent);
            }

            
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



    /// <summary>
    /// Custom class that inherits from WebViewClient. Used to enable the override of the ShouldOverrideUrlLoading() method.
    /// </summary>
    /// <seealso cref="Android.Webkit.WebViewClient" />
    public class WebClient : WebViewClient
    {
        /// <summary>
        /// Shoulds the override URL loading.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="url">The URL.</param>
        /// <returns>Return true with the requested URL loaded into the object.</returns>
        public override bool ShouldOverrideUrlLoading(WebView view, string url)
        {
            view.LoadUrl(url);
            return true;
        }
    }
}