﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polly.Data
{
    public class ProductCategory
    {
        [Key, Column(Order = 0), ForeignKey("Product")]
        public long ProductId { get; set; }

        [Key, Column(Order = 1), ForeignKey("Category")]
        public int CategoryId { get; set; }

        public Product Product { get; set; }

        public Category Category { get; set; }
    }
}
