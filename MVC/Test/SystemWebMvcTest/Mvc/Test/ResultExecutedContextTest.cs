﻿namespace System.Web.Mvc.Test {
    using System;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ResultExecutedContextTest {

        [TestMethod]
        public void ConstructorWithNullControllerContextThrows() {
            // Execute & verify
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    new ResultExecutedContext(null /* controllerContext */, null /* result */, null /* exception */);
                },
                "controllerContext");
        }

        [TestMethod]
        public void ConstructorWithNullExceptionIsOk() {
            // Execute
            ActionResult result = new EmptyResult();
            ResultExecutedContext context = new ResultExecutedContext(ControllerContextTest.GetControllerContext(), result, null /* exception */);

            // Verify
            Assert.IsNull(context.Exception);
        }

        [TestMethod]
        public void ConstructorWithNullResultThrows() {
            // Execute & verify
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    new ResultExecutedContext(ControllerContextTest.GetControllerContext(), null /* result */, null /* exception */);
                },
                "result");
        }

        [TestMethod]
        public void GetException() {
            // Setup
            ActionResult result = new EmptyResult();
            Exception exception = new Exception();
            ResultExecutedContext context = new ResultExecutedContext(ControllerContextTest.GetControllerContext(), result, exception);

            // Execute & verify
            Assert.AreSame(exception, context.Exception);
        }

        [TestMethod]
        public void GetResult() {
            // Setup
            ActionResult result = new EmptyResult();
            Exception exception = new Exception();
            ResultExecutedContext context = new ResultExecutedContext(ControllerContextTest.GetControllerContext(), result, exception);

            // Execute & verify
            Assert.AreSame(result, context.Result);
        }

        [TestMethod]
        public void SetExceptionHandled() {
            // Setup
            ActionResult result = new EmptyResult();
            Exception exception = new Exception();
            ResultExecutedContext context = new ResultExecutedContext(ControllerContextTest.GetControllerContext(), result, exception);

            // Execute
            bool origVal = context.ExceptionHandled;
            context.ExceptionHandled = true;
            bool newVal = context.ExceptionHandled;

            // Verify
            Assert.IsFalse(origVal);
            Assert.IsTrue(newVal);
        }

    }
}