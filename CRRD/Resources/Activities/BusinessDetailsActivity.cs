
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using CRRD.Resources.Models;
	
using Android.Support.V4.Graphics.Drawable;

using System;
using System.Collections.Generic;
using CRRD.Resources.Data;
using Android.Views;
using Android.Support.V7.App;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using System.Linq;

namespace CRRD.Resources.Activities
{
    /// <summary>
    /// Android Activity: Used for displaying the the details of a given business, which is passed
    /// </summary>
    /// <seealso cref="Android.App.Activity" />
    [Activity(Label = "@string/BusinessDetailsActivityLabel", Icon = "@drawable/CSCLogo")]
    public class BusinessDetailsActivity : AppCompatActivity
    {
        private TextView _txtBusName, _txtBusAddress, _txtBusPhone, _txtBusWebsite, _txtBusAccepts, _txtBusAcceptsLabel;
        private LinearLayout _layoutBusAddress, _layoutBusPhone, _layoutBusWebsite, _layoutBusAccepts, _layoutBusAcceptsLabel, _layoutBusLinks;
        private string _categoryName, _subcategoryName, _businessName;
        private Business _businessObj = new Business();
        private XMLHandler _handler = new XMLHandler();

        /// <summary>
        /// Called on creation of Business Details Activity.
        /// </summary>
        /// <param name="bundle">The bundle, used for passing data between Activities. This should include the extra "businessName"</param>
        protected override void OnCreate(Bundle bundle)
        {
            // Check if _handler has Categories and Businesses initialized
            ErrorCheckActivity.checkXMLHandlerInitialization(this.ApplicationContext, _handler.isInitialized);

            base.OnCreate(bundle);
            SetContentView(Resource.Layout.BusinessDetails);

            // Get the passed properties
            _categoryName = Intent.GetStringExtra("categoryName") ?? "No Data Found";
            _subcategoryName = Intent.GetStringExtra("subcategoryName") ?? "No Data Found";
            _businessName = Intent.GetStringExtra("businessName") ?? "No Data Found";
            _businessObj = _handler.GetBusinessByName(_businessName);

            // Set the toolbar
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = _businessName ?? this.ApplicationContext.GetString(Resource.String.BusinessDetailsActivityLabel);

            // Get layout objects
            _txtBusName = FindViewById<TextView>(Resource.Id.txtBusinessName);
            _txtBusAddress = FindViewById<TextView>(Resource.Id.txtBusinessAddress);
            _txtBusPhone = FindViewById<TextView>(Resource.Id.txtBusinessPhone);
            _txtBusWebsite = FindViewById<TextView>(Resource.Id.txtBusinessWebsite);
            _txtBusAccepts = FindViewById<TextView>(Resource.Id.txtBusinessAccepts);
            _txtBusAcceptsLabel = FindViewById<TextView>(Resource.Id.txtBusinessAcceptsLabel);
            _layoutBusAddress = FindViewById<LinearLayout>(Resource.Id.layoutBusinessAddress);
            _layoutBusPhone = FindViewById<LinearLayout>(Resource.Id.layoutBusinessPhone);
            _layoutBusWebsite = FindViewById<LinearLayout>(Resource.Id.layoutBusinessWebsite);
            _layoutBusAccepts = FindViewById<LinearLayout>(Resource.Id.layoutBusinessAccepts);
            _layoutBusAcceptsLabel = FindViewById<LinearLayout>(Resource.Id.layoutBusinessAcceptsLabel);
            _layoutBusLinks = FindViewById<LinearLayout>(Resource.Id.layoutBusinessLinks);

            // Set the layout objects
            _txtBusName.Text = _businessObj.Name;
            _txtBusAddress.Text = GetFormattedAddress();
            _txtBusPhone.Text = GetFormattedPhoneNumber();
            _txtBusWebsite.Text = _businessObj.Website;
            _txtBusAccepts.Text = GetSubcategoriesAccepted();
            _txtBusAcceptsLabel.Text = GetSubcategoriesAcceptedLabel();

            // Remove layouts if content is not available
            RemoveOnEmptyTextView(_txtBusAddress, _layoutBusAddress);
            RemoveOnEmptyTextView(_txtBusPhone, _layoutBusPhone);
            RemoveOnEmptyTextView(_txtBusWebsite, _layoutBusWebsite);
            RemoveOnEmptyTextView(_txtBusAccepts, _layoutBusAccepts);
            RemoveOnEmptyTextView(_txtBusAccepts, _layoutBusAcceptsLabel);

            // Event Listeners
            _layoutBusPhone.Clickable = true;
            _layoutBusPhone.Click += _txtBusPhone_Click;
            _layoutBusWebsite.Clickable = true;
            _layoutBusWebsite.Click += _txtBusWebsite_Click;
            _layoutBusAddress.Clickable = true;
            _layoutBusAddress.Click += _busAddress_Click;

            // Add Links and Link Listeners
            addBusLinks();

        }



/// <summary>
/// Sets a View's visibility to Gone if the TextView's text is an empty string.
/// </summary>
/// <param name="view1">A TextView with text set to "".</param>
/// <param name="view2">The view to be made hidden if view1's has an empty string.</param>
private void RemoveOnEmptyTextView(TextView view1, View view2)
{
if (string.Compare(view1.Text.Trim(), "") == 0)
{
    view2.Visibility = ViewStates.Gone;

}  
}

/// <summary>
/// Handles the Click event of a link, sending starting the WebViewer activity and passing the extras "PDF_URI" and "PDF_Title".
/// </summary>
/// <param name="sender">The source of the event.</param>
/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
private void _txtBusLink_Click(object sender, EventArgs e)
{
            
            LinearLayout linkLayout = (LinearLayout)sender;
            TextView linkTextView = (TextView)linkLayout.GetChildAt(1);
            string linkText = linkTextView.Text;
            

            foreach (var link in _businessObj.LinkList)
            {
                if(link.Name == linkText)
                {

                    var intent = new Intent(this, typeof(WebViwerActivity));
                    intent.PutExtra("PDF_URI", link.URI);
                    intent.PutExtra("PDF_Title", linkText);
                    StartActivity(intent);
                }
            }
           


        }

/// <summary>
/// Handles the Click event of the _txtBusWebsite control, opening the website on the user's device's default browser.
/// </summary>
/// <param name="sender">The source of the event.</param>
/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
private void _txtBusWebsite_Click(object sender, EventArgs e)
{
if (_businessObj.Website != "")
{
    var uri = Android.Net.Uri.Parse(_businessObj.Website);
    var intent = new Intent(Intent.ActionView, uri);
    StartActivity(intent);
}
}

/// <summary>
/// Opens the phone activity and sets the phone number as the business' phone number to call.
/// </summary>
/// <param name="sender">The source of the Event, the PhoneNumber TextView.</param>
/// <param name="e">The <see cref="EventArgs"/>The instance containing the event data.</param>
private void _txtBusPhone_Click(object sender, EventArgs e)
{
// Build the uri
var dialString = "tel:";
dialString += GetFormattedPhoneNumber();

var uri = Android.Net.Uri.Parse(dialString);

var intent = new Intent(Intent.ActionDial, uri);
StartActivity(intent);
}


/// <summary>
/// Opens the phone activity and sets the phone number as the business' phone number to call.
/// </summary>
/// <param name="sender">The source of the Event, the PhoneNumber TextView.</param>
/// <param name="e">The <see cref="EventArgs"/>The instance containing the event data.</param>
private void _busAddress_Click(object sender, EventArgs e)
{

var geoUri = Android.Net.Uri.Parse("geo:0,0?q=" + Android.Net.Uri.Parse(_businessObj.Address_1 + " " + _businessObj.Address_2 + " " + _businessObj.City + " " + _businessObj.State + " " + _businessObj.Zip.ToString()));
var mapIntent = new Intent(Intent.ActionView, geoUri);
StartActivity(mapIntent);

}

/// <summary>
/// Gets a USA-formatted Address from the business object.
/// </summary>
/// <returns>
/// A USA-formatted Address string value as per the USPS. 
/// </returns>
private string GetFormattedAddress()
{
string formatedAddr = "";
string addr_1Str = "";
string addr_2Str = "";
string zipStr = "";
string CityStateZip = "";

// Check if there are parts to an address
if (_businessObj.Address_1.Trim() != "")
{
    addr_1Str = String.Format("{0}\n", _businessObj.Address_1.Trim());
}
if (_businessObj.Address_2.Trim() != "" && _businessObj.Address_2.Trim() != "null")
{
    addr_2Str = String.Format("{0}\n", _businessObj.Address_2.Trim());
}

// Check Zip is 5 or 9 digits
switch (_businessObj.Zip.ToString().Length)
{
    case 5:
        zipStr = _businessObj.Zip.ToString();
        break;
    case 9:
        zipStr = GetFormattedZip(_businessObj.Zip.ToString());
        break;
}

CityStateZip = String.Format("{0} {1} {2}", _businessObj.City.Trim(), _businessObj.State.Trim(), zipStr.Trim());

// Put all the pieces together in standard Address Format USA
formatedAddr = String.Format("{0}{1}{2}", addr_1Str, addr_2Str, CityStateZip);

return formatedAddr;
}

/// <summary>
/// Gets a USA-formatted 9-digit Zip from the business object.
/// </summary>
/// <param name="zip">The zip in string form.</param>
/// <returns>
/// A USA 9-digit formatted zip string value. (Example: 12345-6789)
/// </returns>
private string GetFormattedZip(string zip)
{
return String.Format("{0}-{1}", zip.Substring(0, 5), zip.Substring(5, 4));
}

/// <summary>
/// Gets a USA-formatted 7, 10, or 11 digit Phone number from the business object.
/// </summary>
/// <returns>
/// A USA-formatted 7, 10, or 11 digit Phone number string value. (Example: 1(222)333-4444)
/// </returns>
private string GetFormattedPhoneNumber()
{
string phoneNum = _businessObj.Phone.ToString();
string formattedPhone = "";

switch (phoneNum.Length)
{
    case 7:
        formattedPhone = String.Format("{0}-{1}", phoneNum.Substring(0, 3), phoneNum.Substring(3, 4));
        break;
    case 10:
        formattedPhone = String.Format("1({0}){1}-{2}", phoneNum.Substring(0, 3), phoneNum.Substring(3, 3), phoneNum.Substring(6, 4));
        break;
    case 11:
        formattedPhone = String.Format("1({0}){1}-{2}", phoneNum.Substring(1, 3), phoneNum.Substring(2, 3), phoneNum.Substring(7, 4));
        break;
}

return formattedPhone;
}

/// <summary>
/// Gets the items/subcategories accepted by the business represented by _businessObj.
/// </summary>
/// <returns>A string with alphabetically-ordered, distinct items separated by the newline character. </returns>
private string GetSubcategoriesAccepted()
{

    List<string> items = new List<string>();
    string subcategoryList = "";

    foreach (var cat in _businessObj.CategoryList)
    {

        foreach (var sub in cat.SubcategoryList)
        {
                    
            items.Add(sub);
        }

    }
    items.Sort();
    items = items.Distinct().ToList();

    foreach (var item in items)
    {
        subcategoryList += (item + "\n");
    }

    return subcategoryList;
}

/// <summary>
/// Returns the string used to label the section listing items/subcategories a business accepts.
/// </summary>
/// <returns>A string with the business name concatenated with "Accepts The Following:".</returns>
private string GetSubcategoriesAcceptedLabel()
{
return _businessObj.Name + " Accepts The Following:";
}


/// <summary>
/// Creates the menu for the Toolbar/Action Bar to use/
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
        /// Dynamically add links associated with each business and on-click functions to these links, which are inserted into the LinearLayout _layoutBusLink.
        /// </summary>
        private void addBusLinks()
{

            foreach (var link in _businessObj.LinkList)
            {
               
                // Create New Views
                LinearLayout _layoutSingleLink = new LinearLayout(this);
                ImageView _iconSingleLink = new ImageView(this);
                TextView _textSingleLink = new TextView(this);

                // Set Link Layout Attributes
                float scale = this.ApplicationContext.Resources.DisplayMetrics.Density;
                int layoutPixels = (int)(70 * scale + 0.5f);//layoutPixels is equivalent to 70dp
                LinearLayout.LayoutParams _layoutSingleLinkParams = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, layoutPixels, 1f);
                _layoutSingleLink.LayoutParameters = _layoutSingleLinkParams;
                _layoutSingleLink.Orientation = Orientation.Horizontal;

                // Set Link ImageView Attributes
                LinearLayout.LayoutParams _iconSingleLinkParams = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent, 1f);
                _iconSingleLink.LayoutParameters = _iconSingleLinkParams;
                Android.Graphics.Drawables.Drawable linkIcon = Android.Support.V4.Content.ContextCompat.GetDrawable(this.ApplicationContext, Resource.Drawable.ic_link_black_24dp);
                linkIcon = DrawableCompat.Wrap(linkIcon);
                DrawableCompat.SetTint(linkIcon.Mutate(), Android.Support.V4.Content.ContextCompat.GetColor(this.ApplicationContext, Resource.Color.cscBlue));
                _iconSingleLink.SetImageDrawable(linkIcon);

                // Set Link TextView Attributes
                LinearLayout.LayoutParams _textSingleLinkParams = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent, 4f);
                int textPixels = (int)(5 * scale + 0.5f);//textPixels is equivalent to 5dp
                _textSingleLinkParams.BottomMargin = textPixels;
                _textSingleLink.TextSize = 20;
                _textSingleLink.LayoutParameters = _textSingleLinkParams;
                _textSingleLink.SetTextColor(Android.Support.V4.Content.ContextCompat.GetColorStateList(this.ApplicationContext, Resource.Color.cscBlue));
                
                _textSingleLink.Text = link.Name;

                // Set on-click action
                _layoutSingleLink.SetTag(link.Name.GetHashCode(), link.URI);
                _layoutSingleLink.Clickable = true;
                _layoutSingleLink.Click += _txtBusLink_Click;
                
                // Add views to layout
                _layoutSingleLink.AddView(_iconSingleLink);
                _layoutSingleLink.AddView(_textSingleLink);
                _layoutBusLinks.AddView(_layoutSingleLink);
                

            }
            
        }

    }
}

