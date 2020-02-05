using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sgop.Models
{
    public class RequisicionViewModel
    {
        public int IdRequisicion { get; set; }
        public int IdConcepto { get; set; }
        public float Cantidad { get; set; }
    }
}