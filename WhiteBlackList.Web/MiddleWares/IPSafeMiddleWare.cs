using Microsoft.Extensions.Options;
using System.Net;

namespace WhiteBlackList.Web.MiddleWares
{

    // Program.cs içinde de aşağıdaki kod bloğu ile eklendi.
    // app.UseMiddleware<IPSafeMiddleWare>();

    public class IPSafeMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly IPList _ipList;


        // IPList doldurmak için IOptions interface kulllanılıyor. Program.cs içinde de aşağıdaki kod bloğu kullanıldı.
        // builder.Services.Configure<IPList>(builder.Configuration.GetSection("IPList"));
        public IPSafeMiddleWare(RequestDelegate next, IOptions<IPList> ipList)
        {
            _next = next;
            _ipList = ipList.Value;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            // API den gelen isteğin adresini al.
            var reqIpAddress=httpContext.Connection.RemoteIpAddress;

            var isWhiteList = _ipList.WhiteList.Where(x => IPAddress.Parse(x).Equals(reqIpAddress)).Any();

            if(!isWhiteList)
            {
                httpContext.Response.StatusCode=(int) HttpStatusCode.Forbidden; 
                return;
            }

            await _next(httpContext);
        }
    }
}
