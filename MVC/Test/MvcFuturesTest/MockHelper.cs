using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Moq;

namespace MvcFuturesTest {
    public static class MockHelper {
        public static StringBuilder SwitchResponseMockOutputToStringBuilder(HttpResponseBase response) {
            return SwitchResponseMockOutputToStringBuilder(Mock.Get(response));
        }

        public static StringBuilder SwitchResponseMockOutputToStringBuilder(Mock<HttpResponseBase> responseMock) {
            var sb = new StringBuilder();
            responseMock.Expect(response => response.Write(It.IsAny<string>())).Callback<string>(output => sb.Append(output));
            return sb;
        }
    }
}
