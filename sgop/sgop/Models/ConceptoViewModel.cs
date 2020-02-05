using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sgop.Models
{
    public class ConceptoViewModel
    {
        public int IdConcepto { get; set; }
        public string Clave { get; set; }
        public string Descripcion { get; set; }
        public string Unidad { get; set; }
        public string PrecioUnitario { get; set; }
    }
}