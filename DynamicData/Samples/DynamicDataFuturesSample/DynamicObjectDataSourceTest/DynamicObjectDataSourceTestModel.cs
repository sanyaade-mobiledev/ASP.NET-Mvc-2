using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace DynamicDataFuturesSample {

    /// <summary>
    /// Summary description for AggregateData
    /// This is based on this MSDN sample:
    /// http://msdn.microsoft.com/en-us/library/system.web.ui.webcontrols.objectdatasource.dataobjecttypename.aspx
    /// </summary>
    public class AggregateData {

        static DataTable table;

        private DataTable CreateData() {
            table = new DataTable();
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Number", typeof(int));
            table.Columns.Add("Date", typeof(DateTime));
            table.Rows.Add(new object[] { "One", 1, new DateTime(1977, 7, 7) });
            table.Rows.Add(new object[] { "Two", 2, new DateTime(1988, 8, 8) });
            table.Rows.Add(new object[] { "Three", 3, new DateTime(2007, 1, 14) });
            return table;
        }

        public DataTable Select() {
            return (table == null) ? CreateData() : table;
        }

        public int Insert(NewData newRecord) {
            if (newRecord.Name.Length < 3) {
                throw new ValidationException("The name must have at least 3 characters");
            }

            table.Rows.Add(new object[] { newRecord.Name, newRecord.Number, newRecord.Date });
            return 1;
        }
    }

    public class NewData {
        [RegularExpression(@"[A-Z].*", ErrorMessage="The name must start with an upper case character")]
        [Required]
        [DisplayName("The name")]
        public string Name { get; set; }

        [Range(0, 1000)]
        [UIHint("IntegerSlider")]
        [DefaultValue(345)]
        public int Number { get; set; }

        [DataType(DataType.Date)]
        [UIHint("DateAjaxCalendar")]
        [Required]
        public DateTime Date { get; set; }
    }
}