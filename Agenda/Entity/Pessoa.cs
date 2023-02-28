using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agenda.Entity
{
    class Pessoa
    {
        // Objeto Id do mongodb
        public ObjectId Id { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public bool Ativo { get; set; }

        public List<Endereco> Enderecos { get; set; }
    }
}
