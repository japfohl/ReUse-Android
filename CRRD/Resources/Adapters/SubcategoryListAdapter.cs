using Android.Content;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using CRRD.Resources.Models;
using CRRD.Resources.Data;

namespace CRRD.Resources.Adapters
{ 
    class SubcategoryListAdapter : BaseAdapter<string>
    {
        private List<string> _Items;
        private Context _context;

        public SubcategoryListAdapter(Context context, List<string> items)
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


            // Set what each row will display
            TextView txtCategoryName = row.FindViewById<TextView>(Resource.Id.txtCategoryName);
            txtCategoryName.Text = _Items[position];

            return row;
        }
    }
}