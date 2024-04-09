using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Entidade
{
    public class Professor
    {
        public Professor()
        {
            Perfil = new Perfil();
        }

        public long ID { get; set; }
        public bool ProfessorAtivo { get; set; }
        public bool ProfessorVerificado { get; set; }
        public Perfil Perfil { get; set; }

    }
}
