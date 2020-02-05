using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sgop.Models
{
    public class EstimacionConceptoViewModel
    {
        public string PrecioUnitario { get; set; }
        public string IdEstimacion { get; set; }
        public string IdConcepto { get; set; }
        public string Descripcion { get; set; }
        public string Cantidad { get; set; }
        public string Total { get; set; }
        public string Estimacion { get; set; }

    }
}