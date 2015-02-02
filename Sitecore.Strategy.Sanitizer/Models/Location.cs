using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sitecore.Strategy.Sanitizer.Models
{
    public class Location
    {
        public static readonly string DEFAULT_CITY = "N/A";
        public static readonly string DEFAULT_AREACODE = "0";
        public static readonly string DEFAULT_METROCODE = "0";
        public static readonly string DEFAULT_POSTALCODE = "N/A";
        public Location()
        {
            this.AreaCode = DEFAULT_AREACODE;
            this.City = DEFAULT_CITY;
            this.MetroCode = DEFAULT_METROCODE;
            this.PostalCode = DEFAULT_POSTALCODE;
        }

        public string Country { get; set; }
        public string Region { get; set; }  //ISO-3166-2 codes for US and Canada, FIPS codes all other countries
        public string MetroCode { get; set; } //US only, 0 otherwise
        public string City { get; set; }
        public string PostalCode { get; set; } //US and CA only, N/A otherwise
        public string AreaCode { get; set; } //US only, 0 otherwise
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
