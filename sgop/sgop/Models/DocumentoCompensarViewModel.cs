using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sgop.Models
{
    public class DocumentoCompensarViewModel
    {
        public int? idRequisicionParcial { get; set; }
        public int noDocumento { get; set; }
        public int idRequisicion { get; set; }
        public string clDocumento { get; set; }
        public double importe { get; set; }
        public string fechaDocumento { get; set; }
        public int docCompensacion { get; set; }
        public string fechaCompensacion { get; set; }
        public int usuarioCreacion { get; set; }
        public int usuarioCompensacion { get; set; }
    }
}