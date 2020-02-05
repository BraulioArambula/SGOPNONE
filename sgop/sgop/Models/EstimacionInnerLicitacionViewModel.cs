using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sgop.Models
{
    public class EstimacionInnerLicitacionViewModel
    {
        public string RequisicionRango { get; set; }
        public string IdRequisicion { get; set; }
        public string IdMunicipio { get; set; }
        public string NoLicitacion { get; set; }
        public string Localidad { get; set; }
        public string NombreObra { get; set; }
        public string Cantidad { get; set; }
        public string Total { get; set; }
        public string IdLicitacion { get; set; }
        public string IdProyecto { get; set; }
    }
}