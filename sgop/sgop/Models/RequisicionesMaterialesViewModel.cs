using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sgop.Models
{
    public class RequisicionesMaterialesViewModel
    {
        public int idRequisicion { get; set; }
        public string idRequisicionEncrypt { get; set; }
        public int idRelacion { get; set; }
        public int idLicitacion { get; set; }
        public string idLicitacionEncrypt { get; set; }
        public int? idConcepto { get; set; }
        public string idConceptoEncrypt { get; set; }
        public int? idMaterial { get; set; }
        public double? cantidadMaterial { get; set; }
        public string descripcionMaterial { get; set; }
    }
}