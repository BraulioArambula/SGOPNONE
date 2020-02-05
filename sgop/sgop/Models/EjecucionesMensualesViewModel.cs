using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sgop.Models
{
    public class EjecucionesMensualesViewModel
    {
        public int idEjecucion { get; set; }
        public string periodo { get; set; }
        public int? idRequisicion { get; set; }
        public int? idConcepto { get; set; }
        public string clave { get; set; }
        public double? cantidad { get; set; }
        public int periodoMes { get; set; }
        public int periodoAnio { get; set; }
    }
}