using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IGeneroService
    {
        Task<Response> Cadastrar(Genero g);
        Task<DataResponse<Genero>> LerGeneros();
        Task<SingleResponse<Genero>> GetByID(int id);
        Task<Response> Update(Genero g);
    }
}
