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
    class RecycleXMLHandler
    {
        public Boolean isInitialized { get; private set; }

        private XDocument xDoc { get; set; }
        public List<Category> CategoryList { get; private set; }
        public List<Business> BusinessList { get; private set; }
        private MyDeviceIO deviceIO { get; set; }

        private const string fileName = "CRRD_RECYCLE_XML.xml"; //An arbitrary filename
        private const string _ERR_NO_NETWORK = "No Active Network Found";
        private const string _ERR_BAD_URI = "Bad URI Request";

        // Current Link to the API
        private string BUSINESS_LIST_URI = "http://app.sustainablecorvallis.org/recycleXML";

        /// <summary>
        /// Constructor for the XMLHandler class. Instantiates and sets collection properties. It also runs all parsing methods
        /// for setting the Business and Category classes. 
        /// </summary>
        public RecycleXMLHandler()
        {
            // Instantiate the lists
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
                   
                    isInitialized = true;

                    break;
            }


        }

        /// <summary>
        /// Parses the data in the CRRD Database XML file into the Business and Category classes.
        /// </summary>
        private void SetBusinessList()
        {
            // Collect all Elements (business) and all its descendants.
            var businesses = from qry in xDoc.Descendants("business")
                             select new
                             {
                                 // string values
                                 Name = qry.Element("name").Value,
                                 
                             };

            Business tmpBus;


            // Fill the Business and Category objects with the gathered XML data
            foreach (var bus in businesses)
            {
                tmpBus = new Business();
                tmpBus.Name = bus.Name;
                BusinessList.Add(tmpBus);
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
        /// Gets the XML from URL.
        /// </summary>
        /// <returns>A string result of the requested XML file</returns>
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
        /// Retieves a specific Business object from the BusinessList.
        /// </summary>
        /// <param name="businessName">The name of the business.</param>
        /// <returns>
        /// The Business object of the with the given name.
        /// </returns>
        public Business GetBusinessByName(string businessName)
        {
            return BusinessList.FirstOrDefault(x => x.Name == businessName);
        }

    }
}