using System;
using System.Web;
using System.Threading;

namespace AgendamentoTarefas
{
    public class AgendadorTarefasModule : IHttpModule
    {
        ParameterizedThreadStart threadDelegate;
        Thread newThread;

        public AgendadorTarefasModule()
        {
        }

        public void Init(HttpApplication application)
        {
            application.BeginRequest +=
                (new EventHandler(this.Application_BeginRequest));
            application.EndRequest +=
                (new EventHandler(this.Application_EndRequest));
        }

        private void Application_BeginRequest(Object source,
             EventArgs e)
        {
            HttpApplication application = (HttpApplication)source;
            HttpContext context = application.Context;

            threadDelegate = new ParameterizedThreadStart(AgendadorTarefasThread.Processar);
            newThread = new Thread(threadDelegate);
            newThread.Start(application.Context);

        }

        private void Application_EndRequest(Object source, EventArgs e)
        {
            HttpApplication application = (HttpApplication)source;
            HttpContext context = application.Context;

            newThread.Abort();
        }

        public void Dispose() 
        { 
        }
    }
}
