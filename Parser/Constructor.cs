using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing;
using Scaffold.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    public class Constructor
    {
        private readonly ExcelPackage _package;
        private ExcelWorksheet _ws => _package.Workbook.Worksheets.First();
        private readonly string _newFile;
        private readonly string _template;

        public Constructor()
        {
        }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="newFilepath">путь до нового файла</param>
        /// 
        public Constructor(string newFilepath, string templateFilepath = "template.xlsx")
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            if (!File.Exists(templateFilepath))
            {
                throw new FileNotFoundException("template not found: " + templateFilepath);
            }
            if (Path.GetExtension(newFilepath) != ".xlsx" 
                || Path.GetExtension(templateFilepath) != ".xlsx")
            {
                throw new ArgumentException("Файл и шаблон должен быть расширения .xlsx");
            }
            

            FileInfo newFile = new FileInfo(newFilepath);
            if (newFile.Exists)
            {
                newFile.Delete();                
            }
            File.Copy(templateFilepath, newFilepath);

            _newFile = newFilepath;
            _template = templateFilepath;

            _package = new ExcelPackage(_newFile);
        }

        public async Task ReplaceAsync(string find, string? replace)
        {
            ExcelRangeBase range = _ws.Cells.First(cell => cell.Value?.ToString()?.Contains(find) == true);
            range.Value = range.Value?.ToString()?.Replace(find, replace);
        }

        public string? FindText(string find)
        {
            ExcelRangeBase? range = _ws.Cells.FirstOrDefault(cell => cell.Value?.ToString()?.Contains(find) == true);
            return range?.Address;
        }

        public async Task InjectMeasure(Measure measure)
        {
            foreach (MeasureGroup group in measure.MeasureGroups)
            {
                //string namef = "%" + nameof(group.VoiceConnectionMetric.IdVoiceConnectionMetric);
                //string? address = FindText(namef);
                //Console.WriteLine(_ws.Cells[address]?.Value);

                InjectAsync(group.VoiceConnectionMetric);
                InjectAsync(group.MessagingMetric);
                InjectAsync(group.HttpTransmittingMetric);
                InjectAsync(group.ReferenceInfoMetric);
                InjectAsync(group);
            }

            InjectAsync(measure);
            InjectAsync(measure.MeasureInfo);
        }

        public async Task InjectAsync<IModelContext>(IModelContext obj)
        {            
            if (obj is null)
            {
                return;
            }

            foreach (PropertyInfo property in obj.GetType().GetProperties())
            {
                try
                {
                    /*
                    if (property.propertytype.getinterface(nameof(imodelcontext)) is not null)
                    {
                        object? inner = property.getvalue(obj);
                        if (inner is not null)
                        {
                            inject(inner);
                        }
                    }*/
                    string? result;
                    if (property.GetValue(obj) is DateOnly date)
                    {
                        result = date.ToString("dd.MM.yyyy");
                    } else
                    {
                        result = property.GetValue(obj)?.ToString();
                    }

                    
                    await ReplaceAsync("%" + property.Name, result);
                }
                catch
                {

                }
            }
            return;
        }

        public void Save(string? saveFilepath = null)
        {
            _package.SaveAs(saveFilepath ?? _newFile);
        }
    }
}
