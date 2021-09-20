using Domain;
using Microsoft.AspNetCore.Mvc;
using MVCPresentationLayer.Models.Filme;
using Service;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCPresentationLayer.Controllers
{
    public class FilmeController : Controller
    {
        //Readonly -> que ninguém poderá REINSTANCIAR este objeto
        private readonly IGeneroService _generoService;

        public FilmeController(IGeneroService generoSvc)
        {
            this._generoService = generoSvc;
        }


        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> Create()
        {
            DataResponse<Genero> responseGeneros = await _generoService.LerGeneros();
            if (responseGeneros.DeuBoa)
            {
                ViewBag.Generos = responseGeneros.Data;
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(FilmeInsertViewModel viewModel)
        {
            return View();
        }

    }
}
