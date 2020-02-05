using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sgop.Models
{
    public class LicitacionesRequisicionesViewModel
    {
        public int idLicitacion { get; set; }
        public int idProyecto { get; set; }
        public string idLicitacionEncrypt { get; set; }
        public int? idEmpresa { get; set; }
        public int? idMaterial { get; set; }
        public string razonSocial { get; set; }
        public string noLicitacion { get; set; }
        public string nombreObra { get; set; }
        public int? idMunicipio { get; set; }
        public string nombreMunicipio { get; set; }
        public string localidad { get; set; }
        public DateTime? fechaVisita { get; set; }
        public DateTime? fechaAclaraciones { get; set; }
        public DateTime? fechaPropuesta { get; set; }
        public DateTime? fechaFallo { get; set; }
        public DateTime? fechaContrato { get; set; }
        public string actaVisita { get; set; }
        public string actaAclaraciones { get; set; }
        public string actaPropuesta { get; set; }
        public string actaFallo { get; set; }
        public int? idEstatus { get; set; }
        public int? idRequisicion { get; set; }
        public int? idRequisicionRango { get; set; }
        public int? idConcepto { get; set; }
        public double? cantidad { get; set; }
        public double? total { get; set; }
        public string idConceptoEncrypt { get; set; }
        public string clave { get; set; }
        public string descripcion { get; set; }
        public string descripcionMaterial { get; set; }
        public string unidad { get; set; }
        public double? precioUnitario { get; set; }
        public double? cantReqXcantMat { get; set; }
        public double? cantReq { get; set; }
        public double? cantMat { get; set; }
        public double? cantDisponible { get; set; }
        public string idRequisicionEncrypt { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public int? usuarioCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public int? usuarioModificacion { get; set; }
    }
}