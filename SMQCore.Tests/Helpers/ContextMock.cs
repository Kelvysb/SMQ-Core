using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace SMQCore.Tests.Helpers
{
    public static class ContextMock
    {
        public static ControllerContext Build(int id)
        {
            ControllerContext context = new ControllerContext();
            Mock<ClaimsPrincipal> principal = new Mock<ClaimsPrincipal>();
            Mock<HttpContext> httpContext = new Mock<HttpContext>();

            principal.Setup(p => p.FindFirst(It.IsAny<string>())).Returns(new Claim("sub", id.ToString()));
            httpContext.SetupGet(h => h.User).Returns(principal.Object);
            context.HttpContext = httpContext.Object;

            return context;
        }
    }
}