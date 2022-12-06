using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
        public ActionResult Index(FormCollection objetoForm)
        {
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
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                }
            }
            var data = _db.Analistas.ToList();
            return View(data);
        }

        public ActionResult Index2(FormCollection objetoForm)
        {
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

                        string analista1Actual = _db.AnalistasM.Where(c => c.Turno > 0).Select(a => a.NombreCompleto1).FirstOrDefault();
                        string analista2Actual = _db.AnalistasM.Where(c => c.Turno > 0).Select(a => a.NombreCompleto2).FirstOrDefault();
                        string analisita1Siguiente = _db.AnalistasM.Where(c => c.idAnalistasM == idA).Select(a => a.NombreCompleto1).FirstOrDefault();
                        string analisita2Siguiente = _db.AnalistasM.Where(c => c.idAnalistasM == idA).Select(a => a.NombreCompleto2).FirstOrDefault();

                        if (an != null)
                        {
                            an.Turno = 1;
                            an.Fecha = DateTime.Now;
                            _db.SaveChanges();

                            foreach (var item in _db.AnalistasM.ToList())
                            {
                                int idAList = Convert.ToInt32(item.idAnalistasM);
                                if (idAList != idA)
                                {
                                    var idlist = _db.AnalistasM.SingleOrDefault(c => c.idAnalistasM == idAList);
                                    if (idlist != null)
                                    {
                                        idlist.Turno = 0;
                                        //idlist.Fecha = null;
                                        _db.SaveChanges();
                                    }
                                } 
                            }
                        }

                        //dbContextTransaction.Commit();
                        return RedirectToAction("Index2");
                    }

                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                }
            }
            var data = _db.AnalistasM.ToList();
            return View(data);
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
                catch (Exception ex)
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
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                }
            }
        }
        
      
      
    }
}