using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Polly.Website
{
    public class PercentageDataTypeAttribute : DataTypeAttribute
    {
        public PercentageDataTypeAttribute() 
            : base("Percentage")
        {
            DisplayFormat = new DisplayFormatAttribute();
            DisplayFormat.DataFormatString = "{0:0.00}%";
        }
    }

}