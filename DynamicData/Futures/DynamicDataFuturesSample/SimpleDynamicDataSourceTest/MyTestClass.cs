using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.Web.DynamicData;

namespace DynamicDataFuturesSample {
    public class MyTestClass {
        private string _myString;

        static MyTestClass() {
            // This demonstrates add attributes via code instead of declaratively
            InMemoryMetadataManager.AddColumnAttributes<MyTestClass>(t => t.MyString,
                new DescriptionAttribute("Description attribute added programmatically"));
        }

        [Required]
        public string MyString {
            get {
                return _myString;
            }
            set {
                if (value != null && Char.IsLower(value[0])) {
                    throw new Exception("Should start with upper case!");
                }

                _myString = value;
            }
        }

        [UIHint("IntegerSlider")]
        [DisplayName("My cool integer")]
        [Range(0, 100)]
        [Display(Order = -1)] // Causes the column to be first (since default order is 0)
        public int MyInteger { get; set; }

        [Range(50, 150)]
        public int MySecondInteger { get; set; }

        [DataType(DataType.Date)]
        public DateTime MyDateTime { get; set; }
    }
}
