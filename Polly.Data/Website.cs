using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace Polly.Data
{
    public class Website
    {
        [Key]
        public long Id { get; set; }

        public string Name { get; set; }

        public string Domain { get; set; }

        public string UserAgent { get; set; }

        [ForeignKey("DataSourceTypeId")]
        public virtual DataSourceType DataSourceType { get; set; }

        public DataSourceTypeEnum DataSourceTypeId { get; set; }

        public string HeadingXPath { get; set; }

        public string SubHeadingXPath { get; set; }

        public string DescriptionXPath { get; set; }

        public string PriceXPath { get; set; }

        public string CategoryXPath { get; set; }

        public string BreadcrumbXPath { get; set; }

        public string MainImageXPath { get; set; }

        public string RobotsText { get; set; }

        public DateTime Schedule { get; set; }

        [Column(TypeName = "varbinary(max)")]
        public byte[] Logo { get; set; }
    }
}