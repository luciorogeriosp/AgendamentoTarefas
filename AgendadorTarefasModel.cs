using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgendamentoTarefas
{
    public enum SituacaoProcesso
    {
        AguardandoExecucao = 1,
        EmExecucao = 2,
        ExecucaoConcluida = 3
    }

    public enum TipoProcesso
    {
        CompactacaoPasta = 1
    }

    public class AgendadorTarefasModel
    {
        public string ID { get; set; }
        public SituacaoProcesso Situacao { get; set; }
        public TipoProcesso Tipo { get; set; }
        public object Objeto { get; set; }
    }
}