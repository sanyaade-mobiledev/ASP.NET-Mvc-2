namespace Microsoft.Web.Preview.Services {
    using System.Diagnostics.CodeAnalysis;

    public class ChangeList {
        private object[] _updated;
        private object[] _inserted;
        private object[] _deleted;

        public ChangeList() {
        }

        public ChangeList(object[] updated, object[] inserted, object[] deleted) {
            _updated = updated;
            _inserted = inserted;
            _deleted = deleted;
        }

        [SuppressMessage("Microsoft.Performance", "CA1819", Justification="Unknown.")]
        public object[] Updated {
            get { return _updated; }
            set { _updated = value; }
        }

        [SuppressMessage("Microsoft.Performance", "CA1819", Justification = "Unknown.")]
        public object[] Inserted {
            get { return _inserted; }
            set { _inserted = value; }
        }

        [SuppressMessage("Microsoft.Performance", "CA1819", Justification = "Unknown.")]
        public object[] Deleted {
            get { return _deleted; }
            set { _deleted = value; }
        }
    }
}
