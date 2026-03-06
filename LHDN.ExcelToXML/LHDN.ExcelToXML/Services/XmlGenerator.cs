using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Security;
using LHDN.ExcelToXML.Models;
using System.IO;

namespace LHDN.ExcelToXML.Services
{
    public static class XmlGenerator
    {
        public static void GenerateXml(int appType, List<Instrument> instruments, string outputPath)
        {
            var bulk = new XElement("bulkstamping",
                new XElement("applicationType", appType),
                instruments.Select(inst => new XElement("instrument",
                    new XElement("refNo", inst.RefNo),
                    new XElement("instrumentDate", inst.InstrumentDate),
                    new XElement("instrumentDateReceive", inst.InstrumentDateReceive),
                    appType == 43
                        ? new XElement("principal", inst.Principal)
                        : null,
                    appType == 43
                        ? new XElement("subsidiary", inst.Subsidiary)
                        : null,
                    new XElement("typeOfInstrument", inst.TypeOfInstrument),
                    new XElement("typeOfInstrumentOthers", SecurityElement.Escape(inst.TypeOfInstrumentOthers)),

                    // Parties
                    inst.Transferors.Select(t => new XElement("transferor",
                        new XElement("type", t.Type),
                        new XElement("name", SecurityElement.Escape(t.Name)),
                        new XElement("rocNo", SecurityElement.Escape(t.RocNo)),
                        new XElement("street1", SecurityElement.Escape(t.Street1))
                    )),
                    inst.Transferees.Select(t => new XElement("transferee",
                        new XElement("type", t.Type),
                        new XElement("name", SecurityElement.Escape(t.Name)),
                        new XElement("icNo", t.IcNo),
                        new XElement("street1", SecurityElement.Escape(t.Street1))
                    )),

                    // Conditional fields
                    appType == 43
                        ? new XElement("consideration", inst.Consideration)
                        : new XElement("noOfCopy", inst.NoOfCopy),

                    appType == 43
                        ? new XElement("duration", inst.Duration)
                        : new XElement("remissionOrExemption", inst.RemissionOrExemption),

                    appType == 43
                        ? new XElement("durationDesc", inst.DurationDesc)
                        : new XElement("payment", inst.Payment),

                    appType == 44
                        ? new XElement("aggrementInfo", inst.AggrementInfo)
                        : null,

                    // Attachment
                    new XElement("attachment",
                        new XAttribute("name", inst.AttachmentName),
                        inst.AttachmentBase64 ?? "")
                ))
            );

            var doc = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"), bulk);
            doc.Save(outputPath);

            // Validate 30MB limit
            var size = new FileInfo(outputPath).Length / (1024.0 * 1024.0);
            if (size > 30)
                throw new Exception($"❌ XML file too large ({size:F2} MB > 30MB limit)");
        }
    }
}
