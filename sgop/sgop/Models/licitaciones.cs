//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace sgop.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class licitaciones
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public licitaciones()
        {
            this.proyectos = new HashSet<proyectos>();
        }
    
        public int idLicitacion { get; set; }
        public Nullable<int> idEmpresa { get; set; }
        public string noLicitacion { get; set; }
        public string nombreObra { get; set; }
        public Nullable<int> idMunicipio { get; set; }
        public string localidad { get; set; }
        public Nullable<System.DateTime> fechaVisita { get; set; }
        public Nullable<System.DateTime> fechaAclaraciones { get; set; }
        public Nullable<System.DateTime> fechaPropuesta { get; set; }
        public Nullable<System.DateTime> fechaFallo { get; set; }
        public string actaVisita { get; set; }
        public string actaAclaraciones { get; set; }
        public string actaPropuesta { get; set; }
        public string actaFallo { get; set; }
        public Nullable<int> idEstatus { get; set; }
        public Nullable<int> idRequisicion { get; set; }
        public Nullable<System.DateTime> fechaCreacion { get; set; }
        public Nullable<int> usuarioCreacion { get; set; }
        public Nullable<System.DateTime> fechaModificacion { get; set; }
        public Nullable<int> usuarioModificacion { get; set; }
    
        public virtual catalogoEmpresas catalogoEmpresas { get; set; }
        public virtual catalogoEstatus catalogoEstatus { get; set; }
        public virtual catalogoMunicipios catalogoMunicipios { get; set; }
        public virtual usuarios usuarios { get; set; }
        public virtual usuarios usuarios1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<proyectos> proyectos { get; set; }
    }
}