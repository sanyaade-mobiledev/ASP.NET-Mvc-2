using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.DynamicData;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.Web.DynamicData {
    public class ImprovedDynamicValidator : DynamicValidator {
        protected override void ValidateException(Exception exception) {
            // IDynamicValidatorExceptions are used by LinqDataSource to wrap exceptions caused by problems
            // with setting model properties (columns), such as exceptions thrown from the OnXYZChanging
            // methods
            IDynamicValidatorException e = exception as IDynamicValidatorException;
            if (e != null) {
                if (!String.IsNullOrEmpty(ColumnName)) {
                    // Assemble a list of column names to check that are relevant to this column. This includes
                    // this column as well as any weekly typed foreign key columns if this column is a foreign
                    // key relationship
                    List<string> columnNames = new List<string>(4); // 4 should be a reasonable default for most cases
                    columnNames.Add(ColumnName); // add it first so that it gets checked first
                    if (Column is MetaForeignKeyColumn) {
                        columnNames.AddRange((Column as MetaForeignKeyColumn).Provider.Association.ForeignKeyNames);
                    }

                    Exception inner;
                    foreach (string name in columnNames) {
                        // see if the exception wraps any child exceptions relevant to this column
                        if (e.InnerExceptions.TryGetValue(name, out inner)) {
                            if (inner is ValidationException) {
                                // only include ValidationExceptions
                                ValidationException = inner;
                            }
                            // stop as soon as we find any exception. If it was a ValidationException then the rest
                            // of the DynamicValidator logic will ensure that it gets displayed as a validator error
                            // Otherwise, we want a non-ValidationException to be rethrown to the page.
                            return;
                        }
                    }
                }
            } else {
                // defer to base for other cases
                base.ValidateException(exception);
            }
        }
    }
}
