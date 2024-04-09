using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Entidade
{
    public class Alternativa
    {
        public Alternativa()
        {
            Questao = new Questao();
        }

        public long ID { get; set; }

        public string Resposta { get; set; }

        public bool Correta { get; set;}

        public Questao Questao { get; set;}
    }
}
