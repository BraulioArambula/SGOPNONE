using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sgop.Models
{
    public class controlPagosViewModel
    {
        public int? noDocumento { get; set; }
        public string descripcion { get; set; }
        public double? cantidad { get; set; }
        public int? idRequisicion { get; set; }
        public int? idRequisicionParcial { get; set; }
        public string clDocumento { get; set; }
        public double? importe { get; set; }
        public DateTime? fechaDocumento { get; set; }
        public int? docCompensacion { get; set; }
        public DateTime? fechaCompensacion { get; set; }
        public string factura { get; set; }
        public int? usuarioCreacion { get; set; }
        public int? usuarioCompensacion { get; set; }
        public int idConcepto { get; set; }
        public string clave { get; set; }
        public string descripcionConcepto { get; set; }
    }
}