using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ClosedXML.Excel;
using LHDN.ExcelToXML.Models;

namespace LHDN.ExcelToXML.Services
{
    public static class ExcelReader
    {
        public static (int ApplicationType, List<Instrument> Instruments) LoadFromExcel(string filePath)
        {
            var instruments = new List<Instrument>();
            int detectedAppType = 44; // Default to Penyeteman Am

            using (var workbook = new XLWorkbook(filePath))
            {
                var ws = workbook.Worksheet(1);
                var rows = ws.RangeUsed().RowsUsed().Skip(1); // skip header row

                foreach (var row in rows)
                {
                    // Detect application type
                    int appType = int.TryParse(row.Cell("A").GetString(), out var typeVal) ? typeVal : 44;
                    detectedAppType = appType;

                    // Shared fields
                    var instrument = new Instrument
                    {
                        RefNo = row.Cell("B").GetString().Trim(),
                        InstrumentDate = row.Cell("C").GetString().Trim(),
                        InstrumentDateReceive = row.Cell("D").GetString().Trim(),
                        TypeOfInstrument = row.Cell("E").GetString().Trim(),
                        TypeOfInstrumentOthers = row.Cell("F").GetString().Trim(),
                    };

                    // Common Transferor (Pihak Pertama)
                    var transferor = new Party
                    {
                        Type = int.TryParse(row.Cell("G").GetString(), out var t1) ? t1 : 1,
                        Name = row.Cell("H").GetString().Trim(),
                        Nationality = row.Cell("I").GetString().Trim(),
                        IcNo = row.Cell("J").GetString().Trim(),
                        PassportNo = row.Cell("K").GetString().Trim(),
                        PassportCountry = row.Cell("L").GetString().Trim(),
                        RocNo = row.Cell("M").GetString().Trim(),
                        BusType = row.Cell("N").GetString().Trim(),
                        IncomeTaxNo = row.Cell("O").GetString().Trim(),
                        IncomeTaxBranch = row.Cell("P").GetString().Trim(),
                        Street1 = row.Cell("Q").GetString().Trim(),
                        Street2 = row.Cell("R").GetString().Trim(),
                        Street3 = row.Cell("S").GetString().Trim(),
                        Postcode = row.Cell("T").GetString().Trim(),
                        City = row.Cell("U").GetString().Trim(),
                        State = row.Cell("V").GetString().Trim(),
                        Country = row.Cell("W").GetString().Trim(),
                        TelNo = row.Cell("X").GetString().Trim(),
                        Email = row.Cell("Y").GetString().Trim()
                    };

                    // Common Transferee (Pihak Kedua)
                    var transferee = new Party
                    {
                        Type = int.TryParse(row.Cell("Z").GetString(), out var t2) ? t2 : 0,
                        Name = row.Cell("AA").GetString().Trim(),
                        Nationality = row.Cell("AB").GetString().Trim(),
                        IcNo = row.Cell("AC").GetString().Trim(),
                        PassportNo = row.Cell("AD").GetString().Trim(),
                        PassportCountry = row.Cell("AE").GetString().Trim(),
                        RocNo = row.Cell("AF").GetString().Trim(),
                        BusType = row.Cell("AG").GetString().Trim(),
                        IncomeTaxNo = row.Cell("AH").GetString().Trim(),
                        IncomeTaxBranch = row.Cell("AI").GetString().Trim(),
                        Street1 = row.Cell("AJ").GetString().Trim(),
                        Street2 = row.Cell("AK").GetString().Trim(),
                        Street3 = row.Cell("AL").GetString().Trim(),
                        Postcode = row.Cell("AM").GetString().Trim(),
                        City = row.Cell("AN").GetString().Trim(),
                        State = row.Cell("AO").GetString().Trim(),
                        Country = row.Cell("AP").GetString().Trim(),
                        TelNo = row.Cell("AQ").GetString().Trim(),
                        Email = row.Cell("AR").GetString().Trim()
                    };

                    instrument.Transferors.Add(transferor);
                    instrument.Transferees.Add(transferee);

                    // Distinguish between Sekuriti (43) vs Am (44)
                    if (appType == 43)
                    {
                        // Sekuriti fields
                        instrument.Principal = int.TryParse(row.Cell("AS").GetString(), out var p) ? p : -1;
                        instrument.Subsidiary = row.Cell("AT").GetString().Trim();
                        instrument.Consideration = row.Cell("AU").GetString().Trim();
                        instrument.Duration = row.Cell("AV").GetString().Trim();
                        instrument.DurationDesc = row.Cell("AW").GetString().Trim();
                        instrument.AttachmentName = row.Cell("AX").GetString().Trim();
                    }
                    else
                    {
                        // Penyeteman Am fields
                        instrument.NoOfCopy = row.Cell("AS").GetString().Trim();
                        instrument.RemissionOrExemption = row.Cell("AT").GetString().Trim();
                        instrument.Payment = row.Cell("AU").GetString().Trim();
                        instrument.AggrementInfo = row.Cell("AV").GetString().Trim();
                        instrument.AttachmentName = row.Cell("AW").GetString().Trim();
                    }

                    // Attachment file encoding
                    string attachPath = row.LastCellUsed()?.CellRight()?.GetString().Trim() ?? "";
                    if (File.Exists(attachPath))
                    {
                        instrument.AttachmentBase64 = Convert.ToBase64String(File.ReadAllBytes(attachPath));
                    }

                    instruments.Add(instrument);
                }
            }

            return (detectedAppType, instruments);
        }
    }
}
