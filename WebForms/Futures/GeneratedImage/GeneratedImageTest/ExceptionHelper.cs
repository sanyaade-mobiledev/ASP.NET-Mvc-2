using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Web.Test {
    internal static class ExceptionHelper {
        private static TException ExpectExceptionHelper<TException>(Action del)
                where TException : Exception {
            return ExpectExceptionHelper<TException>(del, false);
        }

        private static TException ExpectExceptionHelper<TException>(Action del, bool allowDerivedExceptions)
                where TException : Exception {
            try {
                del();
                Assert.Fail("Expected exception of type {0}.", typeof(TException));
                throw new Exception("can't happen");
            }
            catch (TException e) {
                if (!allowDerivedExceptions) {
                    Assert.AreEqual(typeof(TException), e.GetType(), "Derrived exceptions are not allowed.");
                }
                return e;
            }
        }

        public static TException ExpectException<TException>(Action del)
                where TException : Exception {
            return ExpectException<TException>(del, false);
        }

        public static TException ExpectException<TException>(Action del, bool allowDerivedExceptions)
                where TException : Exception {
            if (typeof(TException).IsSubclassOf(typeof(ArgumentNullException))) {
                throw new InvalidOperationException("Use different method");
            }
            if (typeof(TException).IsSubclassOf(typeof(ArgumentException))) {
                throw new InvalidOperationException("Use different method");
            }
            return ExpectExceptionHelper<TException>(del, allowDerivedExceptions);
        }

        public static TException ExpectException<TException>(Action del, string errorMessage)
                where TException : Exception {
            TException e = ExpectException<TException>(del, false);
            Assert.AreEqual(errorMessage, e.Message, "Incorrect error message");
            return e;
        }

        public static ArgumentException ExpectArgumentException(Action del, string exceptionMessage) {
            ArgumentException e = ExpectExceptionHelper<ArgumentException>(del);
            Assert.AreEqual(exceptionMessage, e.Message, "Incorrect exception message.");
            return e;
        }

        public static ArgumentException ExpectArgumentExceptionNullOrEmpty(Action del, string paramName) {
            return ExpectArgumentException(del, "Value cannot be null or empty.\r\nParameter name: " + paramName);
        }

        public static ArgumentNullException ExpectArgumentNullException(Action del, string paramName) {
            ArgumentNullException e = ExpectExceptionHelper<ArgumentNullException>(del);
            Assert.AreEqual(paramName, e.ParamName, "Incorrect exception parameter name.");
            return e;
        }

        public static ArgumentOutOfRangeException ExpectArgumentOutOfRangeException(Action del, string paramName, string exceptionMessage) {
            ArgumentOutOfRangeException e = ExpectExceptionHelper<ArgumentOutOfRangeException>(del);
            Assert.AreEqual(paramName, e.ParamName, "Incorrect exception parameter name.");
            if (exceptionMessage != null) {
                Assert.AreEqual(exceptionMessage, e.Message, "Incorrect exception message.");
            }
            return e;
        }
    }
}
