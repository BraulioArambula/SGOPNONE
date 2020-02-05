using sgop.Models;
using System;
using System.Collections.Generic;
using System.Linq;

class Rangos
{
    //Metodo para obtener el ID de acuerdo a lo que se envíe
    //Puede recibir: "LICITACIONES", "FACTURAPROPIA", "FACTURACLIENTE", "PROYECTOS", "REQUISICIONES", "EGRESOS", "INGRESOS"
    public int getSiguienteID(string idAObtener)
    {
        using (var db = new sgopEntities())
        {
            List<RangosViewModel> listaID = null;
            rangos rango;
            int idDefinido, idSiguiente = 0, i = 0;
            int idMayor, idMenor;
            int[] idsArr = null;

            //Obtiene el rango de acuerdo al valor que se envía para buscar
            try
            {
                rango = db.rangos.Where(r => r.pertenceA.Equals(idAObtener)).First();
                idDefinido = Convert.ToInt32(rango.rangoInicial);
                idSiguiente = idDefinido;
            }
            catch (Exception)
            {
                return -1;
            }

            if (idAObtener.Equals("LICITACIONES"))
            {
                listaID = (from lic in db.licitaciones
                            orderby lic.idLicitacion ascending
                            select new RangosViewModel
                            {
                                idUsar = lic.idLicitacion
                            }).ToList();
                idsArr = new int[listaID.Count];
                db.Dispose();
            }
            else if (idAObtener.Equals("FACTURAPROPIA"))
            {
                listaID = (from cc in db.controlCobros
                           where cc.clDocumento == "F"
                           orderby cc.noDocumento ascending
                            select new RangosViewModel
                            {
                                idUsar = cc.noDocumento
                            }).ToList();
                idsArr = new int[listaID.Count];
                db.Dispose();
            }
            else if (idAObtener.Equals("INGRESOS"))
            {
                listaID = (from cc in db.controlCobros
                           where cc.clDocumento == "I"
                           orderby cc.noDocumento ascending
                           select new RangosViewModel
                           {
                               idUsar = cc.noDocumento
                           }).ToList();
                idsArr = new int[listaID.Count];
                db.Dispose();
            }
            else if (idAObtener.Equals("FACTURACLIENTE"))
            {
                listaID = (from cp in db.controlPagos
                           where cp.clDocumento == "F"
                           orderby cp.noDocumento ascending
                            select new RangosViewModel
                            {
                                idUsar = cp.noDocumento
                            }).ToList();
                idsArr = new int[listaID.Count];
                db.Dispose();
            }
            else if (idAObtener.Equals("EGRESOS"))
            {
                listaID = (from cp in db.controlPagos
                           where cp.clDocumento == "E"
                           orderby cp.noDocumento ascending
                           select new RangosViewModel
                           {
                               idUsar = cp.noDocumento
                           }).ToList();
                idsArr = new int[listaID.Count];
                db.Dispose();
            }
            else if (idAObtener.Equals("PROYECTOS"))
            {
                listaID = (from proy in db.proyectos
                            orderby proy.idProyecto ascending
                            select new RangosViewModel
                            {
                                idUsar = proy.idProyecto
                            }).ToList();
                idsArr = new int[listaID.Count];
                db.Dispose();
            }
            else if (idAObtener.Equals("REQUISICIONES"))
            { 
                listaID = (from req in db.requisiciones
                           orderby req.idRequisicionRango ascending
                           select new RangosViewModel
                           {
                               idUsar = req.idRequisicionRango
                           }).Distinct().ToList();
                idsArr = new int[listaID.Count];
                db.Dispose();
            }
            else
            {
                db.Dispose();
                return -1;
            }

            if (listaID.Count > 0)
            {
                foreach (var item in listaID)
                {
                    idsArr[i] = (int)item.idUsar;
                    i++;
                }
                idMayor = idsArr.Max();
                idMenor = idsArr.Min();

                if (idDefinido < idMenor || idDefinido > idMayor)
                {
                    idSiguiente = idDefinido;
                }
                else
                {
                    foreach (var item in listaID)
                    {
                        if (item.idUsar == idSiguiente)
                        {
                            idSiguiente += 1;
                        }

                        if (item.idUsar > idSiguiente)
                        {
                            return idSiguiente;
                        }
                    }
                }

                return idSiguiente;

            }
            else
            {
                return idDefinido;
            }
        }
    }
}