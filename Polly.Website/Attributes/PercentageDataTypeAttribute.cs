using System.ComponentModel.DataAnnotations;

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