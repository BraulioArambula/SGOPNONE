using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sgop.Models
{
    public class EstimacionViewModel
    {
        public string IdEstimacion { get; set; }
        public string NoEstimacion { get; set; }
        public string IdRequisicion { get; set; }
        public string IdConcepto { get; set; }
        public string Cantidad { get; set; }
    }
}