using Microsoft.AspNetCore.Mvc.Filters;

namespace WhiteBlackList.Web.Filters
{
    // ActionFilterAttribute filterlar syesinde controllerdaki herhangi bir methoda istek gelmeden önce/sonra  methoda girdikten/çıktıktan sonra gelen istekler yakalanabiliyor.
    public class CheckWhiteList:ActionFilterAttribute
    {
    }
}
