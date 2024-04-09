using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Entidade
{
    /// <summary>
    /// Retorno para todos os metodos de incluir, editar,  excluir do sistema
    /// </summary>
    [Serializable()]
    public class RetornoAcao<T>
    {
        public RetornoAcao()
        {
            this.Tipo = TipoMensagem.Ok;
        }
        /// <summary>
        /// Codigo de retorno
        /// Menor que 0 ocorreu erro.
        /// </summary>
        [DataMember]
        public int CodigoRetorno { get; set; }

        /// <summary>
        /// Mensagem de retorno
        /// </summary>
        [DataMember]
        public string MensagemRetorno { get; set; }

        /// <summary>
        /// Tipo de mensagem
        /// </summary>
        [DataMember]
        public TipoMensagem Tipo { get; set; }

        [DataMember]
        public T Objeto { get; set; }

        public int CodigoErro { get; set; }
    }

    public class RetornoAcao : RetornoAcao<object>
    {
    }
}
