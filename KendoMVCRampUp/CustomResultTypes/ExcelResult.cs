using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KendoMVCRampUp.CustomResultTypes
{
    public class ExcelResult : FileResult
    {
        private string _base64;

        public ExcelResult(string contentType, string base64, string fileDownloadName) : base(contentType)
        {
            FileDownloadName = fileDownloadName;
            _base64 = base64;
        }

        protected override void WriteFile(HttpResponseBase response)
        {
            response.AddHeader("Content-Disposition", string.Format("attachment; filename=\"{0}\"", FileDownloadName));
            byte[] fileContents = Convert.FromBase64String(_base64);

            response.BinaryWrite(fileContents);
            response.End();
        }
    }
}