﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Faces.WebMVC.ViewModel
{
    public class OrderViewModel
    {
        [Display(Name = "Order Id")]
        public Guid OrderId { get; set; }
        [Display(Name = "Image File")]
        public IFormFile File { get; set; }
        [Display(Name = "Email")]
        public string UserEmail { get; set; }
        [Display(Name = "Image Url")]
        public string ImageUrl { get; set; }
        [Display(Name = "Order Status")]
        public string StatusString { get; set; }
        public byte[] ImageData { get; set; }
        public List<OrderDetailsViewModel> orderDetails { get; set; }
    }
}
