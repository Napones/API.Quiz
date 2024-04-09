using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Entidade
{
    public class Gabarito
    {
        public Gabarito()
        {
            Prova = new Prova();
            Questao = new Questao();
        }

        public long ID { get; set; }
        public int Acertos { get; set; }
        public int Erros { get; set; }
        public decimal Nota { get; set; }
        public Prova Prova { get; set; }
        public Questao Questao { get; set; }

    }
}
