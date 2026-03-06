using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHDN.ExcelToXML.Models
{
    public class Instrument
    {
        public string RefNo { get; set; } = "";
        public string InstrumentDate { get; set; } = "";
        public string InstrumentDateReceive { get; set; } = "";
        public int Principal { get; set; } = -1;
        public string Subsidiary { get; set; } = "";
        public string TypeOfInstrument { get; set; } = "";
        public string TypeOfInstrumentOthers { get; set; } = "";
        public List<Party> Transferors { get; set; } = new();
        public List<Party> Transferees { get; set; } = new();
        public string Consideration { get; set; } = "";
        public string Duration { get; set; } = "";
        public string DurationDesc { get; set; } = "";
        public string AttachmentName { get; set; } = "";
        public string AttachmentBase64 { get; set; } = "";
    }
}
