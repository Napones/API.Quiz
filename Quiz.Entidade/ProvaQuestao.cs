using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Entidade
{
    public class ProvaQuestao
    {
        public ProvaQuestao()
        {
            Prova = new Prova();
            Questao = new Questao();
        }

        public long ID { get; set; }
        public Prova Prova { get; set; }
        public Questao Questao { get; set; }

    }
}
