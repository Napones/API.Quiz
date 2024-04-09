using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Entidade
{
    public class Instituicao
    {
        public Instituicao()
        {
            NaturezaJuridica = new NaturezaJuridica();
        }

        public long ID { get; set; }

        public string Nome { get; set;}

        public string Mantenedora { get; set; }

        public string CNPJ { get; set; }

        public string RepresentanteLegal { get; set; }

        public NaturezaJuridica NaturezaJuridica { get; set; }

    }
}
