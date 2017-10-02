using Android.App;
using Android.Content;
using Android.Net;
using System;
using System.IO;
using CRRD.Resources.Models;

namespace CRRD.Resources.Data
{
    class MyDeviceIO
    {
        public string BasePath { get; set; }
        public string FullFilenamePath { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MyDeviceIO"/> class.
        /// </summary>
        /// <param name="filename">The filename.</param>
        public MyDeviceIO(string filename)
        {
            BasePath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            FullFilenamePath = Path.Combine(BasePath, filename);
        }

        /// <summary>
        /// Saves data to the device.
        /// </summary>
        /// <param name="item">The item.</param>
        public void SaveToDevice(string item)
        {
            using (var streamWriter = new StreamWriter(FullFilenamePath, false))
            {
                streamWriter.WriteLine(item);
            }
        }

        /// <summary>
        /// Reads from the device.
        /// </summary>
        public string ReadFromDevice()
        {
            string content = "";
            try
            {
                using (var streamReader = new StreamReader(FullFilenamePath))
                {
                    content = streamReader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message;

                // some other error occured
                return "Error in ReadFromDevice()";
            }

            return content;
        }

        /// <summary>
        /// Determines whether the device is connected to a network.
        /// </summary>
        /// <returns>
        /// Returns true if the device is connected to a network.
        /// </returns>
        public bool HasNetwork()
        {
            // Get the connectivity manager service
            var connectivityManager = (ConnectivityManager)Application.Context.GetSystemService(Context.ConnectivityService);

            NetworkInfo activeConnection = connectivityManager.ActiveNetworkInfo;
            return (activeConnection != null) && activeConnection.IsConnected;
        }

        /// <summary>
        /// Used to find a bool value of if a BusinessList File exists on the local device. 
        /// </summary>
        /// <returns>
        /// Returns true if a BusinessList File exists on the local device.
        /// </returns>
        public bool BusinessFileExists()
        {
            return File.Exists(FullFilenamePath);
        }
    }
}