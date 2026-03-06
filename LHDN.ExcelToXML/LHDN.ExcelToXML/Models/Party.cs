using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHDN.ExcelToXML.Models
{
    public class Party
    {
        public int Type { get; set; }           // 0 = Individu, 1 = Syarikat
        public string Name { get; set; } = "";
        public string Nationality { get; set; } = "";
        public string IcNo { get; set; } = "";
        public string PassportNo { get; set; } = "";
        public string PassportCountry { get; set; } = "";
        public string RocNo { get; set; } = "";
        public string BusType { get; set; } = "";
        public string Street1 { get; set; } = "";
        public string Street2 { get; set; } = "";
        public string Street3 { get; set; } = "";
        public string Postcode { get; set; } = "";
        public string City { get; set; } = "";
        public string State { get; set; } = "";
        public string Country { get; set; } = "";
        public string TelNo { get; set; } = "";
        public string Email { get; set; } = "";
    }
}

