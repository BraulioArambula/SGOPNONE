using sgop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sgop.Controllers.Paginacion
{
    public class PagedData<T> where T : class
    {
        public IEnumerable<T> Data { get; set; }
        public int totalPages { get; set; }
        public int currentPage { get; set; }
        public string busqueda { get; set; }
        public string buscarPor { get; set; }
        public FormCollection conceptosAgregados { get; set; }
    }
}