using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KendoMVCRampUp.Dtos
{
    public class FilterDto
    {
        public object[] Parameters { get; set; }
        public string DynamicLinqString { get; set; }
        public bool IsFilterApplying { get; set; }
    }
}