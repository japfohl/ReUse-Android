using System.Collections.Generic;

namespace CRRD.Resources.Models
{
    class Category
    {
        public string Name { get; set; }
        public List<string> SubcategoryList { get; set; }

        public Category()
        {
            SubcategoryList = new List<string>();
        }

        public Category(string name, List<string> subcategoryList)
        {
            Name = name;
            SubcategoryList = subcategoryList;
        }
    }
}