using System.Text.RegularExpressions;
using OfficeOpenXml;
using Scaffold.Model;

namespace Parser
{
    /// <summary>
    /// Интерпритатор Microsoft Excel файлов
    /// </summary>
    public class Interpreter : IDisposable
    {
        private readonly ExcelPackage _package;
        public readonly string Filepath;

        public Interpreter()
        {
        }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="filepath">путь до файла</param>
        public Interpreter(string filepath)
        {
            if (!File.Exists(filepath))
            {
                throw new FileNotFoundException("file not found: " + filepath);
            }
            else if (Path.GetExtension(filepath) != ".xlsx")
            {
                throw new Exceptions.FileWrongExtensionException(Path.GetExtension(filepath), ".xlsx");
            }

            try
            {
                Filepath = filepath;
                _package = new ExcelPackage(filepath);
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            }
            catch (Exception ex)
            {
                throw new Exceptions.ConstructorException(ex);
            }
        }

        private ExcelWorksheet _ws => _package.Workbook.Worksheets.First();

        public void Dispose()
        {
            _package.Dispose();
        }

        /// <summary>
        /// Получить информацию о MeasureInfo
        /// </summary>
        /// <param name="measure">родительский элемент</param>
        /// <returns></returns>
        public MeasureInfo? ParseMeasureInfo(ref Measure measure)
        {
            try
            {
                return new MeasureInfo
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

        /// <summary>
        /// Получить информацию о Measure
        /// </summary>
        /// <returns></returns>
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

                measure.MeasureGroups.Add(group);
            }

            return measure;
        }

        /// <summary>
        /// Спарсить MeasureGroup относительно колонки
        /// </summary>
        /// <param name="measure">родительский элемент</param>
        /// <param name="column">номер колонки Excel</param>
        /// <returns></returns>
        private MeasureGroup? ParseMeasureGroup(ref Measure measure, int column)
        {
            try
            {
                var measureGroup = new MeasureGroup
                {
                    Measure = measure,
                    MeasureSubject = _ws.GetValue<string>(18, column),
                    VoiceConnectionMetric = new VoiceConnectionMetric()
                    {
                        VoiceServiceNonAcessibility = _ws.GetValue<float>(19, column),
                        VoiceServiceCutOfffRatio = _ws.GetValue<float>(20, column),
                        SpeechQualityCallBasis = _ws.GetValue<float>(21, column),
                        NegativeMossamplesRatio = _ws.GetValue<float>(22, column)
                    },
                    MessagingMetric = new MessagingMetric()
                    {
                        UndeliveredMessagePercentage = _ws.GetValue<float>(24, column),
                        AverageMessageDeliveryTime = _ws.GetValue<float>(25, column)
                    },
                    HttpTransmittingMetric = new HttpTransmittingMetric()
                    {
                        SessionFailureRatio = _ws.GetValue<float>(27, column),
                        UlmeanUserDataRate = _ws.GetValue<float>(28, column),
                        DlmeanUserDataRate = _ws.GetValue<float>(29, column),
                        SessionTime = _ws.GetValue<float>(30, column)
                    },
                    ReferenceInfoMetric = new ReferenceInfoMetric()
                    {
                        TotalTestVoiceConnections = _ws.GetValue<int>(32, column),
                        TotalVoiceSequences = _ws.GetValue<int>(33, column),
                        NegativeMossamplesCount = _ws.GetValue<int>(34, column),
                        TotalMessagesSent = _ws.GetValue<int>(35, column),
                        TotalConnectionAttempts = _ws.GetValue<int>(36, column),
                        TotalTestSessions = _ws.GetValue<int>(37, column)
                    }
                };
                measureGroup.VoiceConnectionMetric.MeasureGroup = measureGroup;
                measureGroup.MessagingMetric.MeasureGroup = measureGroup;
                measureGroup.HttpTransmittingMetric.MeasureGroup = measureGroup;
                measureGroup.ReferenceInfoMetric.MeasureGroup = measureGroup;
                return measureGroup;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Кастомые ошибки
        /// </summary>
        public static class Exceptions
        {
            /// <summary>
            /// Представляет ошибку неверного расширения файла
            /// </summary>
            public class FileWrongExtensionException : Exception
            {
                public FileWrongExtensionException(string extActual, string extExpected) : base(
                    $"Получен неправильный формат файла: '{extActual}', ожидался: '{extExpected}'")
                {
                }
            }

            /// <summary>
            /// Представляет ошибку конструктора
            /// </summary>
            public class ConstructorException : Exception
            {
                public ConstructorException(Exception? inner) : base(
                    "Ошибка инициализации класса. См. внутреннее исключение", inner)
                {
                }
            }
        }
    }
}