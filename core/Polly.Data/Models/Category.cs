﻿using System.ComponentModel.DataAnnotations;

namespace Polly.Data
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100), Required]
        public string? Description { get; set; }
    }
}
