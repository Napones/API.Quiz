using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Entidade
{
    public class ProfessorInstituicao
    {
        public ProfessorInstituicao()
        {
            NaturezaJuridica = new NaturezaJuridica();
            Professor = new Professor();
        }

        public long ID { get; set; }
        public NaturezaJuridica NaturezaJuridica { get; set; }
        public Professor Professor { get; set; }
        public int TipoInstituicao { get; set; }

    }
}
