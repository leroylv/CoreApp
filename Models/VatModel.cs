using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreApp.Data.Enums;

namespace CoreApp.Models
{
    public class VatModel
    {
        public VatCheckService.checkVatResponse VatResult { get; set; }
        public string Error { get; set; }
        public string VatNumber { get; set; }
        public VatNumberStatus Status { get; set; }
    }
}
