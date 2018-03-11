using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;

namespace AgendamentoTarefas
{
    public class AgendadorTarefasThread
    {
        // Contexto da aplicação
        private static HttpContext _context;

        /// <summary>
        /// Método de execução do processo principal.
        /// </summary>
        /// <param name="context">Contexto da aplicação</param>
        public static void Processar(object context)
        {
            // Atribui a variável local.
            _context = context as HttpContext;

            while (true)
            {
                // Chama o método que identifica se existe algum processo para rodar, inicia um processo independente, e marca o processo com o "Em Execução".
                VerificaFilaProcessos();

                //  Aguarda por um certo intervalo de tempo
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(3));
            }
        }

        /// <summary>
        /// Captura os dados de um processo
        /// </summary>
        /// <param name="Id">Identificador do processo.</param>
        /// <returns></returns>
        public static AgendadorTarefasModel Get(string Id)
        {
            List<AgendadorTarefasModel> listaAgendadorTarefas = null;
            AgendadorTarefasModel item = new AgendadorTarefasModel();

            if (_context.Application["AGENDADOR_TAREFAS_LISTA"] != null)
            {
                listaAgendadorTarefas = _context.Application["AGENDADOR_TAREFAS_LISTA"] as List<AgendadorTarefasModel>;
                item = listaAgendadorTarefas.Where(w => w.ID == Id).FirstOrDefault();
            }

            return item;
        }

        /// <summary>
        /// Captura todos os processos.
        /// </summary>
        /// <returns></returns>
        public static List<AgendadorTarefasModel> All()
        {
            List<AgendadorTarefasModel> listaAgendadorTarefas = new List<AgendadorTarefasModel>();

            if (_context.Application["AGENDADOR_TAREFAS_LISTA"] != null)
            {
                listaAgendadorTarefas = _context.Application["AGENDADOR_TAREFAS_LISTA"] as List<AgendadorTarefasModel>;
            }

            return listaAgendadorTarefas;
        }

        /// <summary>
        /// Adiciona um novo processo à fila de execução.
        /// </summary>
        /// <param name="tipo">Tipo do processo</param>
        /// <param name="o">Objeto a ser processado</param>
        /// <returns></returns>
        public static string Add(TipoProcesso tipo, Object o)
        {
            AgendadorTarefasModel item = new AgendadorTarefasModel();

            if (_context.Application["AGENDADOR_TAREFAS_LISTA"] == null)
            {
                _context.Application["AGENDADOR_TAREFAS_LISTA"] = new List<AgendadorTarefasModel>();
            }

            List<AgendadorTarefasModel>  listaAgendadorTarefas = _context.Application["AGENDADOR_TAREFAS_LISTA"] as List<AgendadorTarefasModel>;

            item.ID = Guid.NewGuid().ToString("n");
            item.Situacao = SituacaoProcesso.AguardandoExecucao;
            item.Tipo = tipo;
            item.Objeto = o;

            listaAgendadorTarefas.Add(item);

            _context.Application["AGENDADOR_TAREFAS_LISTA"] = listaAgendadorTarefas;

            return item.ID;
        }

        /// <summary>
        /// Apaga um processo da fila.
        /// </summary>
        /// <param name="Id"></param>
        public static void Delete(string Id)
        {
             List<AgendadorTarefasModel> listaAgendadorTarefas = null;
            AgendadorTarefasModel item = new AgendadorTarefasModel();

            if (_context.Application["AGENDADOR_TAREFAS_LISTA"] != null)
            {
                listaAgendadorTarefas = _context.Application["AGENDADOR_TAREFAS_LISTA"] as List<AgendadorTarefasModel>;
                item = listaAgendadorTarefas.Where(w => w.ID == Id).FirstOrDefault();

                if (item != null)
                {
                    listaAgendadorTarefas.Remove(item);

                    _context.Application["AGENDADOR_TAREFAS_LISTA"] = listaAgendadorTarefas;
                }
            }
        }

        /// <summary>
        /// Atualiza o status do processo.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="situacao"></param>
        public static void UpdateStatus(string Id, SituacaoProcesso situacao)
        {
            List<AgendadorTarefasModel> listaAgendadorTarefas = null;
            AgendadorTarefasModel item = new AgendadorTarefasModel();

            if (_context.Application["AGENDADOR_TAREFAS_LISTA"] != null)
            {
                listaAgendadorTarefas = _context.Application["AGENDADOR_TAREFAS_LISTA"] as List<AgendadorTarefasModel>;
                item = listaAgendadorTarefas.Where(w => w.ID == Id).FirstOrDefault();

                if (item != null)
                {
                    item.Situacao = situacao;
                }
            }
        }

        /// <summary>
        /// Verifica a fila de processos.
        /// </summary>
        public static void VerificaFilaProcessos()
        {
            try
            {
                List<AgendadorTarefasModel> listaAgendadorTarefas = null;

                if (_context.Application["AGENDADOR_TAREFAS_LISTA"] != null)
                {
                    listaAgendadorTarefas = _context.Application["AGENDADOR_TAREFAS_LISTA"] as List<AgendadorTarefasModel>;
                    listaAgendadorTarefas = listaAgendadorTarefas.Where(w => w.Situacao == SituacaoProcesso.AguardandoExecucao).ToList();

                    if (listaAgendadorTarefas.Count > 0)
                    {
                        foreach (AgendadorTarefasModel itemProcesso in listaAgendadorTarefas)
                        {
                            switch (itemProcesso.Tipo)
                            {
                                case TipoProcesso.CompactacaoPasta :
                                    CompactacaoPasta oCompactacaoPasta = new CompactacaoPasta();
                                    oCompactacaoPasta.OnProcessEnd += new CompactacaoPasta.ProcessEndEventHandler(oCompactacaoPasta_OnProcessEnd);
                                    oCompactacaoPasta.Processar(itemProcesso);

                                    break;
                            }
                        }
                    }
                }

                
            }
            catch (Exception ex)
            {
                // Implementar Log de Eventos
            }
        }

        private static void oCompactacaoPasta_OnProcessEnd(AgendadorTarefasModel sender)
        {
            // Ações customizadas referentes ao Processo de Compactação de Pastas.
        }
    }
}