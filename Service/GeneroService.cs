using Domain;
using FluentValidation.Results;
using Service.Interfaces;
using Service.Validations;
using System;
using Service.Extensions;
using DataInfrastructure;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Service
{

    public class GeneroService : IGeneroService
    {
        public async Task<Response> Cadastrar(Genero genero)
        {
            GeneroValidation validation = new GeneroValidation();
            ValidationResult result = validation.Validate(genero);

            Response r = result.ToResponse();
            if (!r.DeuBoa)
            {
                return r;
            }

            try
            {
                using (LocadoraDBContext db = new LocadoraDBContext())
                {
                    //Verifica se o gênero já existe
                    Genero generoCadastrado = await db.Generos.FirstOrDefaultAsync(g => g.Nome == genero.Nome);
                    if (generoCadastrado != null)
                    {
                        //Retorna pois a chave única estouraria este registro
                        return new Response()
                        {
                            DeuBoa = false,
                            Mensagem = "Gênero já cadastrado."
                        };
                    }

                    db.Generos.Add(genero);
                    await db.SaveChangesAsync();
                    return new Response()
                    {
                        DeuBoa = true,
                        Mensagem = "Gênero cadastrado com sucesso."

                    };
                }
            }
            catch (Exception ex)
            {
                return new Response()
                {
                    DeuBoa = false,
                    Mensagem = "Erro no banco de dados, contate o administrador."
                };
            }
        }

        public async Task<DataResponse<Genero>> LerGeneros()
        {
            DataResponse<Genero> response = new DataResponse<Genero>();

            try
            {

                using (LocadoraDBContext db = new LocadoraDBContext())
                {

                    List<Genero> generos = await db.Generos.OrderBy(c => c.ID).ToListAsync();
                    response.Data = generos;
                    response.DeuBoa = true;
                    response.Mensagem = "Gêneros selecionados com sucesso.";
                }
            }
            catch (Exception ex)
            {
                response.DeuBoa = false;
                response.Mensagem = "Erro no banco de dados, contate o adm.";
            }
            return response;
        }

        public async Task<SingleResponse<Genero>> GetByID(int id)
        {
            SingleResponse<Genero> response = new SingleResponse<Genero>();

            try
            {

                using (LocadoraDBContext db = new LocadoraDBContext())
                {
                    Genero genero = await db.Generos.FindAsync(id);
                    if (genero == null)
                    {
                        response.DeuBoa = false;
                        response.Mensagem = "Gênero não encontrado.";
                        return response;
                    }
                    response.DeuBoa = true;
                    response.Mensagem = "Gênero selecionado com sucesso.";
                    response.Item = genero;
                }
            }
            catch (Exception ex)
            {
                response.DeuBoa = false;
                response.Mensagem = "Erro no banco de dados, contate o adm.";
            }
            return response;
        }

        public async Task<Response> Update(Genero g)
        {
            GeneroValidation validation = new GeneroValidation();
            ValidationResult result = validation.Validate(g);

            Response r = result.ToResponse();
            if (!r.DeuBoa)
            {
                return r;
            }

            try
            {

                using (LocadoraDBContext db = new LocadoraDBContext())
                {
                    //Tecnica 1
                    //db.Entry<Genero>(g).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    //db.SaveChanges();

                    //Tecnica 2
                    Genero generoExistente =await db.Generos.FirstOrDefaultAsync(gen => gen.Nome == g.Nome);

                    if(generoExistente != null)
                    {
                        return new Response()
                        {
                            DeuBoa = false,
                            Mensagem = "Este gênero já existe."
                        };
                    }
                    Genero generoBanco = await db .Generos.FindAsync(g.ID);
                    generoBanco.Nome = g.Nome;
                    await db.SaveChangesAsync();
                    return new Response()
                    {
                        DeuBoa = true,
                        Mensagem = "Gênero editado com sucesso."
                    };
                }
            }
            catch (Exception ex)
            {
                r.DeuBoa = false;
                r.Mensagem = "Erro no banco de dados, contate o adm.";
                return r;
            }

        }
    }
}
