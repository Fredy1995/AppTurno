using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Mvc;
using Tuturno.Models;
/// <summary>
/// Autor: Fredy Garate Marín y Andrea Mendez Lopez
/// Fecha: 16 / 11 / 2022
///
/// </summary>
namespace Tuturno.Controllers
{
    public class HomeController : Controller
    {

        public dbTuturnoEntities _db = new dbTuturnoEntities();
        private modelPrincipal data = new modelPrincipal();
        public ActionResult Index(FormCollection objetoForm)
        {
            string ipobtenida = Bitacora();
            ActualizaTurnoAutomatico();
            using (var dbContextTransaction = _db.Database.BeginTransaction())
            {
                try
                {
                    if(objetoForm["btnagregar"] != null)
                    {
                        var an = new Analistas()
                        {
                            NombreCompleto = objetoForm["nombreCompleto"],
                            Turno = 0
                        };

                        _db.Analistas.Add(an);
                        _db.SaveChanges();
                        dbContextTransaction.Commit();
                        return RedirectToAction("Index");
                    }
                    else if (objetoForm["btneliminar"] != null)
                    {
                        int idA = Convert.ToInt32(objetoForm["idAnalista"].ToString());
                        var an = _db.Analistas.SingleOrDefault(c => c.idAnalista == idA);
                        if (an != null)
                        {
                            _db.Analistas.Remove(an);
                            _db.SaveChanges();
                            dbContextTransaction.Commit();
                            return RedirectToAction("Index");
                        }
                    }else if(objetoForm["btnactualizar"] != null)
                    {
                        int idA = Convert.ToInt32(objetoForm["idAnalistaActualizar"].ToString());
                        var an = _db.Analistas.SingleOrDefault(c => c.idAnalista == idA);
                        
                        if (an != null)
                        {
                            an.Turno = 1;
                            an.Fecha = DateTime.Now;
                            _db.SaveChanges();

                            foreach (var item in _db.Analistas.ToList())
                            {
                                int idAList = Convert.ToInt32(item.idAnalista);
                                if (idAList != idA)
                                {
                                    var idlist = _db.Analistas.SingleOrDefault(c => c.idAnalista == idAList);
                                    if (idlist != null)
                                    {
                                        idlist.Turno = 0;
                                        idlist.Fecha = null;
                                        _db.SaveChanges();

                                    }
                                }
                            }
                        }
                        
                        dbContextTransaction.Commit();
                        return RedirectToAction("Index");
                    }
                  
                }
                catch (Exception )
                {
                    dbContextTransaction.Rollback();
                }
            }
            ProximoCumple();
            data.analist = _db.Analistas.ToList();
            TempData["VerTodo"] = IdentificaArea(ipobtenida);
            if (Convert.ToBoolean(TempData["VerTodo"]))
            {
                return View(data);
            }
            else
            {

                return RedirectToAction("tienda");
            }
        }

        public modelPrincipal ProximoCumple()
        {
            int contador = 1,HayCumpleañeros =0;
            int mes = DateTime.Now.Month;
            var cumpleMes = _db.AnalistasC.Where(c => c.Fecha.Value.Month == DateTime.Now.Month).OrderBy(c => c.Fecha).ToList();
            var RestanPorCumplir = cumpleMes.Where(c => c.Fecha.Value.Day > DateTime.Now.Day).OrderBy(c => c.Fecha).ToList();
            if (RestanPorCumplir.Count > 0 )
            { 
                //Se obtiene el nombre y fecha del cumpleñero del Mes Actual
                data.nombre = cumpleMes.Where(c => c.Fecha.Value.Day > DateTime.Now.Day).Select(c => c.NombreCompleto).FirstOrDefault();
                data.fecha = Convert.ToDateTime(cumpleMes.Where(c => c.Fecha.Value.Day > DateTime.Now.Day).Select(c => c.Fecha).FirstOrDefault());
            }
            else
            {
                //Si no hay mas cumpleaños en el mes actual, entonces se obtiene el cumpleañero del mes siguiente
                do
                {
                    var cumpleNextMes = _db.AnalistasC.Where(c => c.Fecha.Value.Month == DateTime.Now.Month + contador).OrderBy(c => c.Fecha).ToList();
                    HayCumpleañeros = cumpleNextMes.Count();
                    data.nombre = cumpleNextMes.Select(c => c.NombreCompleto).FirstOrDefault();
                    data.fecha = Convert.ToDateTime(cumpleNextMes.Select(c => c.Fecha).FirstOrDefault());
                    contador++;
                } while (HayCumpleañeros < 1);
                   
                
               
            }
           
            return data;
        }
        
        public ActionResult Index2(FormCollection objetoForm)
        {
            string ipobtenida = Bitacora();
            ActualizaTurnoAutomaticoM();
            using (var dbContextTransaction = _db.Database.BeginTransaction())
            {
                try
                {
                    if (objetoForm["btnagregar"] != null)
                    {
                        var an = new AnalistasM()
                        {

                            NombreCompleto1 = objetoForm["analista1"],
                            NombreCompleto2 = objetoForm["analista2"],
                            Turno = 0
                        };

                        _db.AnalistasM.Add(an);
                        _db.SaveChanges();
                        dbContextTransaction.Commit();
                        return RedirectToAction("Index2");
                    }
                    else if (objetoForm["btneliminar"] != null)
                    {
                        int idA = Convert.ToInt32(objetoForm["idAnalistas"].ToString());
                        var an = _db.AnalistasM.SingleOrDefault(c => c.idAnalistasM == idA);
                        if (an != null)
                        {
                            _db.AnalistasM.Remove(an);
                            _db.SaveChanges();
                            dbContextTransaction.Commit();
                            return RedirectToAction("Index2");
                        }
                    }
                    else if (objetoForm["btnactualizar"] != null)
                    {
                        int idA = Convert.ToInt32(objetoForm["idAnalistaActualizar"].ToString());
                        var an = _db.AnalistasM.SingleOrDefault(c => c.idAnalistasM == idA);

                        int IdAnalistaActual = _db.AnalistasM.Where(c => c.Turno > 0).Select(a => a.idAnalistasM).FirstOrDefault();
                        var an2 = _db.AnalistasM.SingleOrDefault(c => c.idAnalistasM == IdAnalistaActual);
                        string analista1Actual = _db.AnalistasM.Where(c => c.Turno > 0).Select(a => a.NombreCompleto1).FirstOrDefault();
                        string analista2Actual = _db.AnalistasM.Where(c => c.Turno > 0).Select(a => a.NombreCompleto2).FirstOrDefault();
                        string analisita1Siguiente = _db.AnalistasM.Where(c => c.idAnalistasM == idA).Select(a => a.NombreCompleto1).FirstOrDefault();
                        string analisita2Siguiente = _db.AnalistasM.Where(c => c.idAnalistasM == idA).Select(a => a.NombreCompleto2).FirstOrDefault();

                        if (an != null)
                        {
                            an.NombreCompleto1 = analista1Actual;
                            an.NombreCompleto2 = analista2Actual;
                            an2.NombreCompleto1 = analisita1Siguiente;
                            an2.NombreCompleto2 = analisita2Siguiente;
                            _db.SaveChanges();
                        }

                        dbContextTransaction.Commit();
                        return RedirectToAction("Index2");
                    }

                }
                catch (Exception )
                {
                    dbContextTransaction.Rollback();
                }
            }
            ProximoCumple();
            //var data = _db.AnalistasM.ToList();
            data.analistm = _db.AnalistasM.ToList();
            TempData["VerTodo"] = IdentificaArea(ipobtenida);
            if (Convert.ToBoolean(TempData["VerTodo"]))
            {
                return View(data);
            }
            else
            {

                return RedirectToAction("tienda");
            }
        }

        public ActionResult Index3(FormCollection objetoForm)
        {
            string ipobtenida = Bitacora();
            using (var dbContextTransaction = _db.Database.BeginTransaction())
            {
                try
                {
                    if (objetoForm["btnagregar"] != null)
                    {
                        var an = new AnalistasC()
                        {

                            NombreCompleto = objetoForm["analista"],
                            Fecha = Convert.ToDateTime(objetoForm["fechaNacimiento"])
                            
                        };

                        _db.AnalistasC.Add(an);
                        _db.SaveChanges();
                        dbContextTransaction.Commit();
                        return RedirectToAction("Index3");
                    }
                    else if (objetoForm["btneliminar"] != null)
                    {
                        int idA = Convert.ToInt32(objetoForm["idAnalistas"].ToString());
                        var an = _db.AnalistasC.SingleOrDefault(c => c.idAnalistaC == idA);
                        if (an != null)
                        {
                            _db.AnalistasC.Remove(an);
                            _db.SaveChanges();
                            dbContextTransaction.Commit();
                            return RedirectToAction("Index3");
                        }
                    }
                }
                catch (Exception )
                {
                    dbContextTransaction.Rollback();
                }
            }
            ProximoCumple();
            //var data = _db.AnalistasC.ToList();
            data.analistasc = _db.AnalistasC.ToList();
            TempData["VerTodo"] = IdentificaArea(ipobtenida);
            if (Convert.ToBoolean(TempData["VerTodo"]))
            {
                return View(data);
            }
            else
            {

                return RedirectToAction("tienda");
            }
        }
        public void ActualizaTurnoAutomaticoM()
        {
            List<AnalistasM> listaA = new List<AnalistasM>();
            listaA = _db.AnalistasM.ToList();

            DateTime dia = DateTime.Now;
            string diaActual = dia.ToString("dddd", new CultureInfo("es-ES"));
            using (var dbContextTransaction = _db.Database.BeginTransaction())
            {
                try
                {
                    if (diaActual.Equals("lunes"))
                    {
                        for (int i = 0; i < listaA.Count; i++)
                        {
                            int idAList = Convert.ToInt32(listaA[i].idAnalistasM);
                           
                            if (listaA[i].Turno > 0 && listaA[i].Turno < 2)
                            {
                                DateTime f = (DateTime)listaA[i].Fecha;
                                if (DateTime.Now.Date > f.Date)
                                {
                                    var idlist = _db.AnalistasM.SingleOrDefault(c => c.idAnalistasM == idAList);
                                    if (idlist != null)
                                    {
                                        idlist.Turno = 0;
                                        _db.SaveChanges();

                                        if (i < listaA.Count - 1)
                                        {
                                            int idsiguiente = Convert.ToInt32(listaA[i + 1].idAnalistasM);
                                            var idlistU = _db.AnalistasM.SingleOrDefault(c => c.idAnalistasM == idsiguiente);
                                            if (idlistU != null)
                                            {
                                                idlistU.Turno = 1;
                                                idlistU.Fecha = DateTime.Now;
                                            }
                                            _db.SaveChanges();
                                        }
                                        else
                                        {

                                            int idIni = Convert.ToInt32(listaA[0].idAnalistasM);
                                            var idUIni = _db.AnalistasM.SingleOrDefault(c => c.idAnalistasM == idIni);
                                            if (idUIni != null)
                                            {
                                                idUIni.Turno = 1;
                                                idUIni.Fecha = DateTime.Now;
                                            }
                                            _db.SaveChanges();
                                        }
                                    }

                                }
                            }
                        }
                        dbContextTransaction.Commit();
                    }

                }
                catch (Exception )
                {
                    dbContextTransaction.Rollback();
                }
            }
        }
        public void ActualizaTurnoAutomatico()
        {
            List<Analistas> listaA = new List<Analistas>();
            listaA = _db.Analistas.ToList();

            DateTime dia = DateTime.Now;
            string diaActual = dia.ToString("dddd", new CultureInfo("es-ES"));
            using (var dbContextTransaction = _db.Database.BeginTransaction())
            {
                try
                {
                    if (!diaActual.Equals("sábado") && !diaActual.Equals("domingo"))
                    {
                        for (int i = 0; i < listaA.Count; i++)
                        {
                            int idAList = Convert.ToInt32(listaA[i].idAnalista);

                            if (listaA[i].Turno > 0 && listaA[i].Turno < 3)
                            {
                                DateTime f = (DateTime)listaA[i].Fecha;
                                if (DateTime.Now.Date > f.Date)
                                {
                                    var idlist = _db.Analistas.SingleOrDefault(c => c.idAnalista == idAList);
                                    if (idlist != null)
                                    {
                                        if (listaA[i].Turno < 2)
                                        {
                                            idlist.Turno = listaA[i].Turno + 1;
                                            idlist.Fecha = DateTime.Now;
                                            _db.SaveChanges();
                                        }
                                        else
                                        {

                                            idlist.Turno = 0;
                                            idlist.Fecha = null;
                                            if (i < listaA.Count - 1)
                                            {
                                                int idsiguiente = Convert.ToInt32(listaA[i + 1].idAnalista);
                                                var idlistU = _db.Analistas.SingleOrDefault(c => c.idAnalista == idsiguiente);
                                                if (idlistU != null)
                                                {
                                                    idlistU.Turno = 1;
                                                    idlistU.Fecha = DateTime.Now;
                                                }
                                                _db.SaveChanges();
                                            }
                                            else
                                            {

                                                int idIni = Convert.ToInt32(listaA[0].idAnalista);
                                                var idUIni = _db.Analistas.SingleOrDefault(c => c.idAnalista == idIni);
                                                if (idUIni != null)
                                                {
                                                    idUIni.Turno = 1;
                                                    idUIni.Fecha = DateTime.Now;
                                                }
                                                _db.SaveChanges();
                                            }

                                        }


                                    }
                                }
                            }



                        }
                        dbContextTransaction.Commit();
                    }

                }
                catch (Exception )
                {
                    dbContextTransaction.Rollback();
                }
            }
        }
        public bool IdentificaArea(string ipobtenida)
        {
            var idArea = _db.Bitacora.Where(c => c.direccionIP == ipobtenida).Select(c => c.idArea).FirstOrDefault();
            if (idArea != null)
            {
                if (idArea == 4)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public string Bitacora()
        {
          
            using (var dbContextTransaction = _db.Database.BeginTransaction())
            {
                var ipobtenida = Request.UserHostAddress;
                var IDip = _db.Bitacora.SingleOrDefault(c => c.direccionIP == ipobtenida );
              
                try
                {
                    if (IDip != null)
                    {
                        //Existe la ip en la bitacora, entonces Actualizo fechaIngreso
                        int IdUser = _db.Bitacora.Where(c => c.direccionIP == ipobtenida).Select(a => a.idUser).FirstOrDefault();
                        var IP = _db.Bitacora.SingleOrDefault(c => c.idUser == IdUser);
                        DateTime fechaRegistrada = Convert.ToDateTime(_db.Bitacora.Where(c => c.direccionIP ==  IP.direccionIP).Select(f => f.fechaIngreso).FirstOrDefault());
                        
                        if (IP != null)
                        {
                            IP.fechaIngreso = DateTime.Now;
                            if (fechaRegistrada.ToString("dd/MM/yyyy") != DateTime.Now.ToString("dd/MM/yyyy"))
                            {
                                IP.VisitasAlDia = 1;
                            }
                            else
                            {
                                IP.VisitasAlDia = IP.VisitasAlDia + 1;
                            }
                            _db.SaveChanges();
                        }
                        dbContextTransaction.Commit();

                    }
                    else 
                    {
                        //No existe IP entonces agregó a la BD
                        var ip = new Bitacora()
                        {
                            idArea = 404, //ip y area no identificada
                            usuario = "Anonimo",
                            direccionIP = ipobtenida,
                            hostname = Dns.GetHostEntry(Request.ServerVariables["REMOTE_HOST"]).HostName,
                            fechaIngreso = DateTime.Now,
                            VisitasAlDia = 1


                        };


                        _db.Bitacora.Add(ip);
                        _db.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                }
                catch (Exception )
                {
                    dbContextTransaction.Rollback();
                }
                return ipobtenida;
            }
        }
        public ActionResult tienda(FormCollection objetoForm, HttpPostedFileBase file)
        {
            string ipobtenida = Bitacora();
            using (var dbContextTransaction = _db.Database.BeginTransaction())
            {
                try
                {
                    if (objetoForm["btnagregar"] != null)
                    {
                        if(objetoForm["descripcion"] != "" && objetoForm["precio"] != "" && objetoForm["file"] != "")
                        {
                            var p = new Productos()
                            {

                                descripcion = objetoForm["descripcion"],
                                precio = Convert.ToDecimal(objetoForm["precio"]),
                                rutaImagen = "../Productos/" + file.FileName

                            };

                            if (file != null && file.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(file.FileName);
                                var path = Path.Combine(Server.MapPath("~/Productos/"), fileName);
                                file.SaveAs(path);
                                _db.Productos.Add(p);
                                _db.SaveChanges();
                                dbContextTransaction.Commit();
                            }
                           
                            return RedirectToAction("tienda");
                        }
                      
                      
                    }
                    else if (objetoForm["btneliminar"] != null)
                    {
                        if(objetoForm["idProducto"] != "")
                        {
                            int idP = Convert.ToInt32(objetoForm["idProducto"].ToString());
                            var rutaImagen = _db.Productos.Where(c => c.idProducto == idP).Select(c => c.rutaImagen).FirstOrDefault();
                            string nombreProdcuto = rutaImagen.Remove(0,13);
                            var p = _db.Productos.SingleOrDefault(c => c.idProducto == idP);
                            if (p != null)
                            {

                                _db.Productos.Remove(p);
                                _db.SaveChanges();

                                var path = Path.Combine(Server.MapPath("../Productos/"), nombreProdcuto);
                                System.IO.File.Delete(path);
                                dbContextTransaction.Commit();
                                return RedirectToAction("tienda");
                            }
                        }
                        
                    }
                }
                catch (Exception )
                {
                    dbContextTransaction.Rollback();
                }
            }
            ProximoCumple();
            data.productos = _db.Productos.ToList();
            TempData["VerTodo"] = IdentificaArea(ipobtenida);
            return View(data);
        }





    }
}