using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sgop.Models
{
    public class ControlCobrosViewModel
    {
        public int noDocumento { get; set; }
        public int? idRequisicion { get; set; }
        public int? noEstimacion { get; set; }
        public string clDocumento { get; set; }
        public double? importe { get; set; }
        public DateTime? fechaDocumento { get; set; }
        public int? docCompensacion { get; set; }
        public DateTime? fechaCompensacion { get; set; }
        public string factura { get; set; }
        public int? usuarioCreacion { get; set; }
        public int? usuarioCompensacion { get; set; }
    }
}