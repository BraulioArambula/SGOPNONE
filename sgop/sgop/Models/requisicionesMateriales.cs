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
    
    public partial class requisicionesMateriales
    {
        public int idRequisicionMaterial { get; set; }
        public Nullable<int> idRequisicion { get; set; }
        public Nullable<int> idConcepto { get; set; }
        public Nullable<int> idMaterial { get; set; }
        public Nullable<double> total { get; set; }
    
        public virtual catalogoConceptos catalogoConceptos { get; set; }
        public virtual catalogoMateriales catalogoMateriales { get; set; }
    }
}