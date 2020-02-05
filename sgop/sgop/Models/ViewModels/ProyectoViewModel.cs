using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sgop.Models.ViewModels
{
    public class ProyectoViewModel
    {
        public int idProyecto { get; set; }
        public int idLicitacion { get; set; }
        public int idEmpresa { get; set; }

        public string Empresa { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "FechaContrato")]
        [Required]
        public DateTime fechaContrato { get; set; }
        public string polizaCumplimiento { get; set; }
        public string noLicitacion { get; set; }
        public string nombreObra { get; set; }
        public string polizaVicios { get; set; }
        public int plazoDias { get; set; }

        public string polizaAnticipo { get; set; }

        public int idMunicipio { get; set; }

        public string Municipio { get; set; }

        public string localidad { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "FechaInicioContrato")]
        [Required]
        public DateTime fechaInicioContrato { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "FechaFinalContrato")]
        [Required]
        public DateTime fechaFinalContrato { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "FechaInicioReal")]
        [Required]
        public DateTime fechaInicioReal { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "FechaFinalReal")]
        [Required]
        public DateTime fechaFinalReal { get; set; }

        public string actaEntrega { get; set; }
        public int jefeObra { get; set; }

        public int idEstatus { get; set; }


        public string Estatus { get; set; }

        public string FCreacion { get; set; }


    }


    public class EditarProyectoViewModel
    {
        public int idProyecto { get; set; }
        public int idLicitacion { get; set; }
        public int idEmpresa { get; set; }

        public string Empresa { get; set; }

        public DateTime fechaContrato { get; set; }
        public string noLicitacion { get; set; }
        public string nombreObra { get; set; }
        public bool polizaVicios { get; set; }
        public string NpolizaVicios { get; set; }
        public bool polizaCumplimiento { get; set; }
        public string NpolizaCumplimiento { get; set; }
        public bool polizaAnticipo { get; set; }
        public string NpolizaAnticipo { get; set; }
        public int plazoDias { get; set; }

        public int idRequisicion { get; set; }

        public int reqParcial { get; set; }

        public int noEstimacion { get; set; }
        public int idMunicipio { get; set; }

        public string Municipio { get; set; }

        public string localidad { get; set; }


        public DateTime fechaInicioContrato { get; set; }


        public DateTime fechaFinalContrato { get; set; }


        public DateTime fechaInicioReal { get; set; }


        public DateTime fechaFinalReal { get; set; }

        public string actaEntrega { get; set; }
        public int jefeObra { get; set; }

        public int idEstatus { get; set; }


        public string Status { get; set; }

        public string TotalRequisicion { get; set; }





    }
}