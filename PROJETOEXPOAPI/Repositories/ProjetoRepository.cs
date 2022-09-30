using Microsoft.AspNetCore.Mvc;
using PROJETOEXPOAPI.Contexts;
using PROJETOEXPOAPI.Models;
using System.Diagnostics.CodeAnalysis;

namespace PROJETOEXPOAPI.Repositories
{
    public class ProjetoRepository
    {
        private readonly SqlContext _context;
        public ProjetoRepository(SqlContext context)
        {
            _context = context; 
        }

        public List<Projeto> Listar()
        {
            return _context.Projetos.ToList();
        }

        public Projeto BuscarPorId(int id)
        {
            return _context.Projetos.Find(id);
        }

        public void Cadastrar(Projeto p)
        {
            _context.Projetos.Add(p);

            _context.SaveChanges();
        }

        public void Deletar(int id)
        {
            Projeto p = _context.Projetos.Find(id);

            _context.Projetos.Remove(p);

            _context.SaveChanges();
        }

        public void Alterar(int id, Projeto p)
        {
            Projeto projetoBuscado = _context.Projetos.Find(id);

            if (projetoBuscado != null)
            {
                projetoBuscado.Titulo = p.Titulo;
                projetoBuscado.Status = p.Status;
                projetoBuscado.DatadeInicio = p.DatadeInicio;
                projetoBuscado.Tecnologias = p.Tecnologias;
                projetoBuscado.Requisitos = p.Requisitos;
                projetoBuscado.Area = p.Area;   

                _context.Projetos.Update(projetoBuscado);

                _context.SaveChanges();
            }

        }
    }
}
