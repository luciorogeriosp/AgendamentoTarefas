using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AgendamentoTarefas.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult VisualizaProcessos()
        {
            List<AgendadorTarefasModel> listaAgendadorTarefas = AgendadorTarefasThread.All();

            return View(listaAgendadorTarefas);
        }

        [HttpPost]
        public ActionResult ProcessarDiretorio(string diretorio)
        {
            string id = AgendadorTarefasThread.Add(TipoProcesso.CompactacaoPasta, diretorio);

            return Json(id, JsonRequestBehavior.AllowGet);
        }
    }
}
