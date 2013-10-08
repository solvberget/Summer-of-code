using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Solvberget.Service.Util;

namespace Solvberget.Service.Controllers
{
    public class ChildJsonResult : JsonResult
    {

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (JsonRequestBehavior == JsonRequestBehavior.DenyGet &&
                String.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("JSONRequest not allowed");
            }

            HttpResponseBase response = context.HttpContext.Response;

            if (!String.IsNullOrEmpty(ContentType))
            {
                response.ContentType = ContentType;
            }
            else
            {
                response.ContentType = "application/json";
            }
            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }
            if (Data != null)
            {
                var serializer = new JavaScriptSerializer();
                var javascriptConverters = new List<JavaScriptConverter> {new ExcludeNullPropertiesConverter()};
                serializer.RegisterConverters(javascriptConverters);
                response.Write(serializer.Serialize(Data));
            }
        }

    }
}