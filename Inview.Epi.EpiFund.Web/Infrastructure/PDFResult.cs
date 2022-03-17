using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inview.Epi.EpiFund.Web.Infrastructure
{
    public class PDFResult : ActionResult
    {
        private MemoryStream _ms;
        private bool _download;
        private string _fileName = string.Empty;

        public PDFResult(MemoryStream ms, bool download, string fileName)
        {
            _ms = ms;
            _download = download;
            _fileName = fileName;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (_ms == null) return;
            var response = context.HttpContext.Response;
            try
            {
                string viewOrDownload = (_download == true) ? "attachment;filename=\"" : "inline;filename=\"";
                _ms.Seek(0, SeekOrigin.Begin);
                response.Clear();
                response.ContentType = "application/pdf";
                response.AddHeader("content-disposition", viewOrDownload + _fileName + ".pdf" + "\";");
                response.AddHeader("Content-Length", _ms.Length.ToString());
                _ms.WriteTo(response.OutputStream);

                if (response.IsClientConnected)
                {
                    response.Flush();
                }
                response.Close();
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                if (ex is InvalidOperationException)
                {
                    // ignore this exception, it means they canceled the download
                }
            }
        }
    }
}