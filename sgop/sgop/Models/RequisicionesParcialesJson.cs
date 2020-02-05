using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sgop.Models
{
    public class RequisicionesParcialesJson
    {
        public int idConcepto { get; set; }
        public int idMaterial { get; set; }
        public double cantidad { get; set; }
        public double disponible { get; set; }
        public string agregado { get; set; }
    }
}