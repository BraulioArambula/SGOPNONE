using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sgop.Models
{
    public class RequisicionesParcialesViewModel
    {
        public int idRequisicionParcial { get; set; }
        public int? idRequisicionRango { get; set; }
        public double? cantReqXcantMat { get; set; }
        public string idRequisicionRangoEncrypt { get; set; }
        public int? noRequisicion { get; set; }
        public int idLicitacion { get; set; }
        public string idLicitacionEncrypt { get; set; }
        public int? idConcepto { get; set; }
        public string descripcionConcepto { get; set; }
        public string claveConcepto { get; set; }
        public string unidadConcepto { get; set; }
        public double? precioUnitario { get; set; }
        public string idConceptoEncrypt { get; set; }
        public int? idMaterial { get; set; }
        public double? cantidad { get; set; }
        public double? cantidadDisponible { get; set; }
        public string descripcionMaterial { get; set; }
        public string aprobada { get; set; }
        public double disponible { get; set; }
        public string agregado { get; set; }
    }
}