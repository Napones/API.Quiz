using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Entidade
{
    public class Prova
    {
        public Prova()
        {
            Professor = new Professor();
        }

        public long ID { get; set; }
        public string Nome { get; set; }
        public Professor Professor { get; set; }
        public int Avaliacao { get; set; }
    }
}
