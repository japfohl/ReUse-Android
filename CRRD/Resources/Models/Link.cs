using System.Collections.Generic;

namespace CRRD.Resources.Models
{
    class Link
    {
        public string Name { get; set; }
        public string URI { get; set; }

        public Link()
        {
            
        }

        public Link(string name, string uri)
        {
            Name = name;
            URI = uri;
        }
    }
}