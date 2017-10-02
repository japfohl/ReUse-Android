using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using CRRD.Resources.Models;
using Android.Content;

namespace CRRD.Resources.Data
{
    class XMLHandler
    {
        public Boolean isInitialized { get; private set; }

        private XDocument xDoc { get; set; }
        public List<Category> CategoryList { get; private set; }
        public List<Business> BusinessList { get; private set; }
        private MyDeviceIO deviceIO { get; set; }

        private const string fileName = "CRRD_XML.xml"; 
        private const string _ERR_NO_NETWORK = "No Active Network Found";
        private const string _ERR_BAD_URI = "Bad URI Request";

        // Current Link to the API
        private string BUSINESS_LIST_URI = "http://app.sustainablecorvallis.org/reuseDB";

        /// <summary>
        /// Constructor for the XMLHandler class. Instantiates and sets collection properties. It also runs all parsing methods 
        /// for setting the Business and Category classes. 
        /// </summary>
        public XMLHandler()
        {
            // instanciate the lists
            CategoryList = new List<Category>();
            BusinessList = new List<Business>();
            
            // Set the MyDeviceIO class
            deviceIO = new MyDeviceIO(fileName);

            // Get the XML Business List from the Database URL
            // Save the XML file to the local device
            string xmlResult = GetXMLFromURL();

            /*
                Checks the result of the XML File process
                - if _ERR_BAD_URI:      The requested URI is down or incorrect (changed) -> check if an old XML file exists on the device
                - if _ERR_NO_NETWORK:   The device is not currently connected to a network -> check if an old XML file exists on the device
                - default:              Will save the new XML file to the device and parse it to the class structure
            */
            switch (xmlResult)
            {
                case _ERR_BAD_URI:
                case _ERR_NO_NETWORK:
                    if (deviceIO.BusinessFileExists())
                    {
                        
                        // Get the locally saved XML document
                        xDoc = XDocument.Parse(GetXmlFromDevice());

                        // Set class collections
                        SetBusinessList();
                        SetCategoryList();
						
						isInitialized = true;
                        
                    }
                    else
                    {
                        isInitialized = false;
                    }
                    break;
                default:
                    
                    SaveXmlToDevice(xmlResult);
                    // Get the locally saved XML document
                    xDoc = XDocument.Parse(GetXmlFromDevice());

                    // Set class collections
                    SetBusinessList();
                    SetCategoryList();

                    isInitialized = true; 
                   
                    break;
            }
           

        }

        /// <summary>
        /// Parses the data in the CRRD Database XML file into the Business and Category classes.
        /// </summary>
        private void SetBusinessList()
        {
            // Collect all Elements (businesses) and all descendants
            var businesses = from qry in xDoc.Descendants("business")
                             select new
                             {
                                 // string values
                                 Name = qry.Element("name").Value,
                                 Addr1 = qry.Element("contact_info").Element("address").Element("address_line_1").Value,
                                 Addr2 = qry.Element("contact_info").Element("address").Element("address_line_2").Value,
                                 City = qry.Element("contact_info").Element("address").Element("city").Value,
                                 State = qry.Element("contact_info").Element("address").Element("state").Value,
                                 Phone = qry.Element("contact_info").Element("phone").Value,
                                 Website = qry.Element("contact_info").Element("website").Value,

                                 // collection values
                                 Categories = qry.Element("category_list").Elements("category"),
                                 Links = qry.Element("link_list").Elements("link"),

                                 // int & double values
                                 ID = qry.Element("id").Value,
                                 Zip = (qry.Element("contact_info").Element("address").Element("zip").Value != "")
                                                ? qry.Element("contact_info").Element("address").Element("zip").Value : "0",
                                 Lat = (qry.Element("contact_info").Element("latlong").Element("latitude").Value != "")
                                                ? qry.Element("contact_info").Element("latlong").Element("latitude").Value : "0",
                                 Lng = (qry.Element("contact_info").Element("latlong").Element("longitude").Value != "")
                                                ? qry.Element("contact_info").Element("latlong").Element("longitude").Value : "0"
                             };

            Business tmpBus;
            List<Category> tmpCatList;
            List<Link> tmpLinkList;
            List<string> tmpSubList;
            string tmpLinkName;
            string tmpLinkURI;

            // Fill the Business and Category objects with the gathered XML data.
            foreach (var bus in businesses)
            {
                tmpBus = new Business();
                tmpCatList = new List<Category>();
                tmpLinkList = new List<Link>();
                
                tmpBus.Name = bus.Name;
                tmpBus.Address_1 = bus.Addr1;
                tmpBus.Address_2 = bus.Addr2;
                tmpBus.City = bus.City;
                tmpBus.State = bus.State;
                tmpBus.Phone = bus.Phone;
                tmpBus.Website = bus.Website;
                tmpBus.Database_ID = Int32.Parse(bus.ID);
                tmpBus.Zip = Int32.Parse(bus.Zip);
                tmpBus.Latitude = double.Parse(bus.Lat);
                tmpBus.Longitude = double.Parse(bus.Lng);

                // Process for Subcategory Lists
                foreach (var c in bus.Categories)
                {
                    tmpSubList = new List<string>();
                    var subcategories = from qry in c.Descendants("subcategory")
                                        select new
                                        {
                                            Subcat = qry.Value
                                        };

                    foreach (var s in subcategories)
                    {
                        tmpSubList.Add(s.Subcat);
                    }

                    tmpCatList.Add(new Category(c.Element("name").Value, tmpSubList));
                    tmpSubList = null;
                }
                tmpBus.CategoryList = tmpCatList;

                // Process for Link Lists
                foreach (var l in bus.Links)
                {
                    tmpLinkName = l.Element("name").Value;
                    tmpLinkURI = l.Element("URI").Value;

                    tmpLinkList.Add(new Link(tmpLinkName, tmpLinkURI));
                }

                tmpBus.LinkList = tmpLinkList;
                BusinessList.Add(tmpBus);

            }

        }

        /// <summary>
        /// Sets the CategoryList for XMLHandler
        /// </summary>
        private void SetCategoryList()
        {
            Predicate<Category> catFinder;
            Category oldCat, newCat;

            // Iterate through the list of Business to get each Category
            foreach (var b in BusinessList)
            {
                // A Business will have n Categories (a given Category may have a unique list of Subcategories)
                foreach (var c in b.CategoryList)
                {
                    
                    catFinder = (Category catToFind) => { return catToFind.Name == c.Name; };
                    oldCat = CategoryList.Find(catFinder);

                    // Insert new Category or update the SubcategoryList of a Category already represented in CategoryList
                    if (oldCat == null)
                    {
                        CategoryList.Add(c);
                    }
                    else
                    {
                        newCat = new Category();
                        newCat.Name = oldCat.Name;

                        foreach (var oldCatSC in oldCat.SubcategoryList)
                        {
                            newCat.SubcategoryList.Add(oldCatSC);

                        }

                        foreach (var sc in c.SubcategoryList)
                        {
                                newCat.SubcategoryList.Add(sc);
                               
                        }

                        CategoryList.Insert(CategoryList.FindIndex(catFinder), newCat);
                        oldCat = null;
                        
                    }
                    
                    
                    
                }
            }
        }

        /// <summary>
        /// Gets the XML from device.
        /// </summary>
        /// <returns>The XML string.</returns>
        private string GetXmlFromDevice()
        {
            return deviceIO.ReadFromDevice();
        }

        /// <summary>
        /// Gets the XML from the URL, BUSINESS_LIST_URI.
        /// </summary>
        /// <returns>The string that is the requested XML file.</returns>
        private string GetXMLFromURL()
        {
            var result = "";

            if (deviceIO.HasNetwork())
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(BUSINESS_LIST_URI);

                try
                {
                    HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

                    StreamReader sr = new StreamReader(resp.GetResponseStream());
                    result = sr.ReadToEnd();
                    sr.Close();
                }
                catch
                {
                    result = _ERR_BAD_URI;
                }

                return result;
            }

            return _ERR_NO_NETWORK;
        }

        /// <summary>
        /// Saves the XML to device.
        /// </summary>
        /// <param name="xmlString">The XML string.</param>
        private void SaveXmlToDevice(string xmlString)
        {
            deviceIO.SaveToDevice(xmlString);
        }

        /// <summary>
        /// Gets a Business object with a given name from the BusinessList.
        /// </summary>
        /// <param name="businessName">The name of the business.</param>
        /// <returns>
        /// The Business object with the given name.
        /// </returns>
        public Business GetBusinessByName(string businessName)
        {
            return BusinessList.FirstOrDefault(x => x.Name == businessName);
        }

    }
}