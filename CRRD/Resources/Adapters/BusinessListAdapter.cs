using Android.Content;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using CRRD.Resources.Models;
using CRRD.Resources.Data;

namespace CRRD.Resources.Adapters
{
    class BusinessListAdapter : BaseAdapter<string>
    {
        private List<string> _Items;
        private Context _context;

		// Start class to Get and parse the local XML file to the associated classes (Business & Category)
		private XMLHandler _handler = new XMLHandler();

        public BusinessListAdapter(Context context, List<string> items)
        {
            _context = context;
            _Items = items;
        }

        public override int Count
        {
            get { return _Items.Count; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override string this[int position]
        {
            get { return _Items[position]; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;

            if (row == null)
            {
                row = LayoutInflater.From(_context).Inflate(Resource.Layout.ListCategory_Row, null, false);
            }

            // Set what each part will display
            TextView txtBusinessName = row.FindViewById<TextView>(Resource.Id.txtCategoryName);
            txtBusinessName.Text = _Items[position];

            return row;
        }
    }
}