using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorldPaymentGetway.Models
{
    public class PayModelView
    {
        public string Token { get; set; }
        public string Email  { get; set; }
        public double Total { get; set; }

    }
}
