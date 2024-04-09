using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Entidade
{
    public class Questao
    {
        public Questao() {

            Materia = new Materia();
        }

        public long ID { get; set; }
        public string Nome { get; set;}
        public string Enunciado { get; set;}

        public string Imagem { get; set;}

        public int Dificuldade { get; set;}

        public bool QuestaoAtiva { get; set;}

        public Materia Materia { get; set;}

    }
}
