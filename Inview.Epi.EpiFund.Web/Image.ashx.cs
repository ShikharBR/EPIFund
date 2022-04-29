using Inview.Epi.EpiFund.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inview.Epi.EpiFund.Web
{
    public class Image : IHttpHandler
    {
        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.QueryString["id"] == null || context.Request.QueryString["name"] == null)
            {
                context.Response.StatusCode = 404;
                return;
            }

            if (context.Request.QueryString["width"] == null || context.Request.QueryString["height"] == null)
            {
                context.Response.StatusCode = 422;
                return;
            }

            var id = new Guid(context.Request.QueryString["id"]);
            var name = context.Request.QueryString["name"];
            var width = Convert.ToInt32(context.Request.QueryString["width"]);
            var height = Convert.ToInt32(context.Request.QueryString["height"]);

            var fileManager = new FileManager();
            var bytes = fileManager.GetScaledImageBytes(Domain.Enum.FileType.Image, id, name, width, height);

            context.Response.ContentType = "image/jpg";
            context.Response.BinaryWrite(bytes);
        }
    }
}