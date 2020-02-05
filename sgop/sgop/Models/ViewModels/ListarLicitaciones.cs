using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace sgop.Models.ViewModels
{
    public class ListarLicitaciones
    {
        public string descripcion_muni { get; set; }
        public string descripcion_estatus { get; set; }
        public string descripcion_empresa { get; set; }

        public int idLicitacion { get; set; }
        public int? idEmpresa { get; set; }
        public string noLicitacion { get; set; }
        public string nombreObra { get; set; }
        public int? idMunicipio { get; set; }
        public string localidad { get; set; }
        [DataType(DataType.Date)]
        // [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "0:yyyy/MM/dd}")]

        public System.DateTime? fechaVisita { get; set; }
        public DateTime? fechaAclaraciones { get; set; }
        public DateTime? fechaPropuesta { get; set; }
        public DateTime? fechaFallo { get; set; }
        public string actaVisita { get; set; }
        public string actaAclaraciones { get; set; }
        public string actaPropuesta { get; set; }
        public string actaFallo { get; set; }
        public int? idEstatus { get; set; }
        public int idRequisicion { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public int usuarioCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public string usuarioModificacion { get; set; }


    }


    public class EditarLicitaciones
    {
        // public string? id_Requisicion { get; set; }
        public double? total_requisicion { get; set; }
        public string propuesta_licitacion { get; set; }
        public string descripcion_muni { get; set; }
        public string descripcion_estatus { get; set; }
        public string descripcion_empresa { get; set; }
        public int? idLicitacion { get; set; }
        public int? idEmpresa { get; set; }
        public string noLicitacion { get; set; }
        public string nombreObra { get; set; }
        public int? idMunicipio { get; set; }
        public string localidad { get; set; }

        [DataType(DataType.Date)]
        // [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "0:yyyy/MM/dd}")]
        public DateTime? fechaVisita { get; set; }
        public string fechaVisitaFomato { get; set; } //Formato Visita
        public DateTime? fechaAclaraciones { get; set; }
        public string fechaAclaracionesFormato { get; set; } //Formato Aclaraciones
        public DateTime? fechaPropuesta { get; set; }
        public string fechaPropuestaFormato { get; set; } //Formato Porpuesta
        public DateTime? fechaFallo { get; set; }
        public string fechaFalloFormato { get; set; }   //Formato Fallo
        public string actaVisita { get; set; }
        public string actaAclaraciones { get; set; }
        public string actaPropuesta { get; set; }
        public string actaFallo { get; set; }
        public int? idEstatus { get; set; }
        public int? idRequisicion { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public int usuarioCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public string usuarioModificacion { get; set; }


    }
}