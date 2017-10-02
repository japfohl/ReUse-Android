using System.Collections.Generic;

namespace CRRD.Resources.Models
{
    class Business
    {
        public string Name { get; set; }
        public int Database_ID { get; set; }
        public string Address_1 { get; set; }
        public string Address_2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int Zip { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public List<Category> CategoryList { get; set; }
        public List<Link> LinkList { get; set; }


        public Business()
        {
            CategoryList = new List<Category>();
        }

        public Business(string name, List<Category> categoryList)
        {
            Name = name;
            CategoryList = categoryList;
        }

        public Business(string name, List<Category> categoryList, List<Link> linkList)
        {
            Name = name;
            CategoryList = categoryList;
            LinkList = linkList;
        }

        public Business(string name, double lat, double lng)
        {
            Name = name;
            Latitude = lat;
            Longitude = lng;
        }
    }
}