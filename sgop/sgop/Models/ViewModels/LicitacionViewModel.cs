using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sgop.Models.ViewModels
{
    public class LicitacionViewModel
    {

        public int idLicitacion { get; set; }
        public int idEmpresa { get; set; }

        public string Empresa { get; set; }

        public string noLicitacion { get; set; }

        public string nombreObra { get; set; }

        public int idMunicipio { get; set; }

        public string Municipio { get; set; }

        public string localidad { get; set; }

        public DateTime fechaVisita { get; set; }

        public DateTime fechaAclaraciones{ get; set; }
        public DateTime fechaPropuesta { get; set; }
        public DateTime fechaFallo { get; set; }

        public string actaVisita { get; set; }
        public string actaAclaraciones { get; set; }
        public string actaPropuesta { get; set; }

        public string actaFallo { get; set; }

        public int idEstatus { get; set; }

        public string Estatus { get; set; }

        public int idRequisicion { get; set; }






    }
}