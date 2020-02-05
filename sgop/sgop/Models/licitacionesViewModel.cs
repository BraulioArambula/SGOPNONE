using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sgop.Models
{
    public class licitacionesViewModel
    {
        public int idLicitacion { get; set; }
        public int? idRequisicion { get; set; }
        public string nombreObra { get; set; }
        public string localidad { get; set; }
        public string noLicitacion { get; set; }
        public string municipio { get; set; }
        public string estatus { get; set; }
        public string empresa { get; set; }
        public DateTime fecha { get; set; }

        public string fechaCreacion { get; set; }

        public List<int?> lstRequisicionesParciales { get; set; }
        public List<int?> lstEstimaciones { get; set; }

    }
}