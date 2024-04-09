using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Entidade
{
    public class Perfil
    {
        public long ID { get; set; }

        public string Nome { get; set; }

        public string Email { get; set; }

        public string Telefone { get; set; }

        public string CPF { get; set; }

        public string Imagem { get; set; }

        public string Descricao { get; set; }

        public string Login { get; set; }

        public string Senha { get; set; }

        public int TipoUsuario { get; set; }

    }
}
