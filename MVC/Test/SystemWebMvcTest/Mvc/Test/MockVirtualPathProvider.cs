namespace System.Web.Mvc.Test {
    using System;
    using System.Collections.Generic;
    using System.Web.Hosting;

    public class MockVirtualPathProvider : VirtualPathProvider {
        private List<KeyValuePair<string, bool>> _expectations;
        private int _currentExpectation;

        public MockVirtualPathProvider(List<KeyValuePair<string, bool>> expectations) {
            _expectations = expectations;
        }

        public override bool FileExists(string virtualPath) {
            if (_currentExpectation >= _expectations.Count) {
                throw new InvalidOperationException("No more expectations remaining");
            }
            KeyValuePair<string, bool> nextExpectation = _expectations[_currentExpectation];
            if (nextExpectation.Key != virtualPath) {
                throw new InvalidOperationException("Path didn't match expectation");
            }
            _currentExpectation++;
            return nextExpectation.Value;
        }

        public void Verify() {
            if (_currentExpectation != _expectations.Count) {
                throw new InvalidOperationException("Not all expectations have been met");
            }
        }
    }
}
