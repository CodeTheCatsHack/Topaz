using System.Text.RegularExpressions;
using OfficeOpenXml;
using Scaffold.Model;

namespace Parser
{
    public class Interpreter : IDisposable
    {
        private readonly ExcelPackage _package;
        public readonly string Filepath;

        public Interpreter()
        {
        }

        public Interpreter(string filepath)
        {
            Filepath = filepath;
            _package = new ExcelPackage(filepath);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        private ExcelWorksheet _ws => _package.Workbook.Worksheets.First();

        public void Dispose()
        {
            _package.Dispose();
        }

        public MeasureInfo? ParseMeasureInfo(ref Measure measure)
        {
            try
            {
                return new MeasureInfo()
                {
                    CompanyName = _ws.GetValue<string>(5, 1).Trim(),
                    CompanyType = _ws.GetValue<string>(6, 1).Trim(),
                    CompanyAbbr = _ws.GetValue<string>(7, 1).TrimEnd(')').TrimStart('('),
                    CompanyFullname = _ws.GetValue<string>(8, 1),
                    Protocol = _ws.GetValue<string>(10, 1),
                    Measure = measure
                };
            }
            catch
            {
                return null;
            }
        }

        public Measure? ParseMeasure()
        {
            Measure measure = new Measure();
            string[] rawDates = Regex.Split(_ws.GetValue<string>(13, 1), @"\s+");

            measure.StartMeasure = DateOnly.ParseExact(rawDates[4], "dd.MM.yyyy");
            measure.EndMeasure = DateOnly.ParseExact(rawDates[6], "dd.MM.yyyy");
            measure.Place = _ws.GetValue<string>(14, 1).Substring(27).Trim();
            measure.Conditions = _ws.GetValue<string>(15, 1).Substring(29).Trim();
            measure.Equipment = _ws.GetValue<string>(16, 1).Substring(27).Trim();
            measure.MeasureInfo = ParseMeasureInfo(ref measure);

            int colIndex = 3;
            MeasureGroup? group = null;

            while (true)
            {
                group = ParseMeasureGroup(ref measure, colIndex++);
                if (group is null || group.MeasureSubject is null)
                {
                    break;
                }

                measure.MeasureGroup.Add(group);
            }

            return measure;
        }

        private MeasureGroup? ParseMeasureGroup(ref Measure measure, int column)
        {
            try
            {
                return new MeasureGroup()
                {
                    Measure = measure,
                    MeasureSubject = _ws.GetValue<string>(18, column),
                    VoiceConnectionMetric = new VoiceConnectionMetric()
                    {
                        VoiceServiceNonAcessibility = _ws.GetValue<float>(19, column),
                        VoiceServiceCutOfffRatio = _ws.GetValue<float>(20, column),
                        SpeechQualityCallBasis = _ws.GetValue<float>(21, column),
                        NegativeMOSsamplesRatio = _ws.GetValue<float>(22, column)
                    },
                    MessagingMetric = new MessagingMetric()
                    {
                        UndeliveredMessagePercentage = _ws.GetValue<float>(24, column),
                        AverageMessageDeliveryTime = _ws.GetValue<float>(25, column)
                    },
                    HttpTransmittingMetric = new HttpTransmittingMetric()
                    {
                        SessionFailureRatio = _ws.GetValue<float>(27, column),
                        ULMeanUserDataRate = _ws.GetValue<float>(28, column),
                        DLMeanUserDataRate = _ws.GetValue<float>(29, column),
                        SessionTime = _ws.GetValue<float>(30, column)
                    },
                    ReferenceInfoMetric = new ReferenceInfoMetric()
                    {
                        TotalTestVoiceConnections = _ws.GetValue<int>(32, column),
                        TotalVoiceSequences = _ws.GetValue<int>(33, column),
                        NegativeMOSsamplesCount = _ws.GetValue<int>(34, column),
                        TotalMessagesSent = _ws.GetValue<int>(35, column),
                        TotalConnectionAttempts = _ws.GetValue<int>(36, column),
                        TotalTestSessions = _ws.GetValue<int>(37, column)
                    }
                };
            }
            catch
            {
                return null;
            }
        }
    }
}