using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sgop.Models
{
    public class CatalogoConceptosViewModel
    {
        public int idConcepto { get; set; }
        public string idConceptoEncrypt { get; set; }
        public string clave { get; set; }
        public string descripcion { get; set; }
        public string unidad { get; set; }
        public double ?precioUnitario { get; set; }
    }
}