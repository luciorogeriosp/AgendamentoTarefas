using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace AgendamentoTarefas
{
    public class CompactacaoPasta
    {
        public delegate void ProcessEndEventHandler(AgendadorTarefasModel sender);
        public event ProcessEndEventHandler OnProcessEnd;

        private void ProcessarEnd(IAsyncResult ar)
        {
            Func<AgendadorTarefasModel, string> function = ar.AsyncState as Func<AgendadorTarefasModel, string>;
            string resultID = function.EndInvoke(ar);
            
            if (OnProcessEnd != null)
            {
                OnProcessEnd(AgendadorTarefasThread.Get(resultID));
            }
        }

        public void Processar(AgendadorTarefasModel o)
        {
            Func<AgendadorTarefasModel, string> function = new Func<AgendadorTarefasModel, string>(ProcessarBegin);
            IAsyncResult result = function.BeginInvoke(o, ProcessarEnd, function);
        }

        private string ProcessarBegin(AgendadorTarefasModel item)
        {
            
            AgendadorTarefasThread.UpdateStatus(item.ID, SituacaoProcesso.EmExecucao);

            // Realizar o processo de compactação da pasta
            //
            //
            //
            //
            //
            //
            //

            Random objRandom = new Random();
            System.Threading.Thread.Sleep(objRandom.Next(1, 5) * 3000);

            AgendadorTarefasThread.UpdateStatus(item.ID, SituacaoProcesso.ExecucaoConcluida);


            return item.ID;
        }
    }
}