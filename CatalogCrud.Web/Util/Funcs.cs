using AutoMapper;
using CatalogCrud.BLL.DTO;
using CatalogCrud.Web.Models.ViewModels;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace CatalogCrud.Web.Util
{
    public static class Funcs
    {
        public static string ClearStringAndToLower(string input)
        {
            return input.Replace("\r\n", "")
                .Replace("\r", "")
                .Replace("\n", "")
                .ToLower();
        }

        public static IEnumerable<RowVM> ConvertDTOValuesByRowsToVMValuesByRows(IEnumerable<IOrderedEnumerable<ValueDTO>> valuesByRows)
        {
            List<RowVM> rows = new List<RowVM>();

            foreach (var valuesByRow in valuesByRows)
            {
                var row = new RowVM
                {
                    Number = valuesByRow.First().Row
                };
                foreach (var valueDTO in valuesByRow)
                    row.Values.Add(Mapper.Map<ValueVM>(valueDTO));
                rows.Add(row);
            }

            return rows;
        }

        public static string[] GetFileContentByLines(HttpPostedFileBase file)
        {
            StreamReader reader = new StreamReader(file.InputStream);
            string[] lines = reader.ReadToEnd().Split('\n');
            reader.Close();

            return lines;
        }
    }
}