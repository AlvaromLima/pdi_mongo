using Agenda.Entity;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace Agenda
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Projeto Agenda");

            //Conectar o servidor mongodb
            Console.WriteLine("Conectando o servidor");
            var client = new MongoClient("mongodb://localhost:27017");

            //Conectar no banco de dados
            Console.WriteLine("Conectando o banco de dados (caso não exista é criado automaticamente");
            var database = client.GetDatabase("Agenda");

            //Criar a coleção onde vai armazenar nosso objeto Pessoa
            Console.WriteLine("Obtendo a coleção de contatos(criado automaticamente)");
            IMongoCollection<Pessoa> colecao = database.GetCollection<Pessoa>("contatos");

            //InserirPessoa(colecao);

            //AlterarPessoa(colecao);

            //ExcluirPessoa(colecao);

            ListarPessoas(colecao);

            Console.WriteLine("Tecle algo!");
            Console.ReadKey();

        }

        private static void ListarPessoas(IMongoCollection<Pessoa> colecao)
        {
            Console.WriteLine("Listando nossos Contatos");
            //Filtro
            var filtro = Builders<Pessoa>.Filter.Empty;
            var pessoas = colecao.Find<Pessoa>(filtro).ToList();
            pessoas.ForEach(x =>
           {
               Console.WriteLine(string.Concat(
                   "Id: ", x.Id,
                   " Nome: ", x.Nome,
                   " Telefone: ", x.Telefone,
                   " Email: ", x.Email,
                   " Endereço: ", x.Enderecos
                   ));
           });
        }

        private static void ExcluirPessoa(IMongoCollection<Pessoa> colecao)
        {
            Console.WriteLine("Excluindo Pessoa");
            //Filtro
            var filtro = Builders<Pessoa>.Filter.Where(x => x.Id.Equals(ObjectId.Parse("63246e11cd46d0e651d1744f")));
            colecao.DeleteMany(filtro);
        }

        private static void AlterarPessoa(IMongoCollection<Pessoa> colecao)
        {

            Console.WriteLine("Atualizando Pessoa");
            //Filtro - quando empty, altera todo mundo
            var filtro = Builders<Pessoa>.Filter.Empty;
            //Remove ativo da coleção Pessoa
            Console.WriteLine("Remove ativo da coleção Pessoa");
            var removeAtivo = Builders<Pessoa>.Update.Unset(p => p.Ativo);
            colecao.UpdateMany(filtro, removeAtivo);

            Console.ReadKey();

            //Adiciona Ativo na coleção Pessoa
            Console.WriteLine("Adiciona ativo da coleção Pessoa");
            var alteracaoAtivo = Builders<Pessoa>.Update.Set(p => p.Ativo, true);
            colecao.UpdateMany(filtro, alteracaoAtivo);

            //Filtro
            //filtro = Builders<Pessoa>.Filter.Where(x => x.Nome == "Alvaro Lima");
            filtro = Builders<Pessoa>.Filter.Where(x => x.Id.Equals(ObjectId.Parse("63246e11cd46d0e651d1744f")));
            var alteracaoEmail = Builders<Pessoa>.Update.Set(p => p.Email, "alvaromlima@gmail.com");
            colecao.UpdateMany(filtro, alteracaoEmail);

        }

        private static void InserirPessoa(IMongoCollection<Pessoa> colecao)
        {
            Console.WriteLine("Inserindo Pessoa");

            Endereco end1 = new Endereco()
            {
                Cidade = "Florianópolis",
                Estado = "Santa Catarina",
                Pais = "Brasil"
            };

            Endereco end2 = new Endereco()
            {
                Cidade = "São Paulo",
                Estado = "São Paulo",
                Pais = "Brasil"
            };

            List<Endereco> enderecos = new List<Endereco>();
            enderecos.Add(end1);
            enderecos.Add(end2);

            Pessoa p = new Pessoa()
            {
                Nome = "Michele Moraes",
                Telefone = "(48) 99909-2761",
                Email = "michelemoraes@gmail.com",
                Ativo = true,
                Enderecos = enderecos
            };
            colecao.InsertOne(p);
        }

    }
}
