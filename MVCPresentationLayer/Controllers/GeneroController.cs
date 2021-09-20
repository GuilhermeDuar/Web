using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Mvc;
using MVCPresentationLayer.Models.Genero;
using Service;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCPresentationLayer.Controllers
{
    public class GeneroController : Controller
    {
        private IGeneroService _generoService;
        private IMapper _mapper;

        public GeneroController(IGeneroService generoService, IMapper mapper)
        {
            this._generoService = generoService;
            this._mapper = mapper;
        }



        public async Task<IActionResult> Index()
        {
            DataResponse<Genero> response = await this._generoService.LerGeneros();
            if (!response.DeuBoa)
            {
                ViewBag.Errors = response.Mensagem;
            }
            List<GeneroQueryViewModel> generos =
                _mapper.Map<List<GeneroQueryViewModel>>(response.Data);

            return View(generos);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Index");
            }

            SingleResponse<Genero> response =await _generoService.GetByID(id.Value);
            if (!response.DeuBoa)
            {
                return RedirectToAction("Index");
            }
            GeneroUpdateViewModel viewModel = _mapper.Map<GeneroUpdateViewModel>(response.Item);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(GeneroUpdateViewModel viewModel)
        {
            Genero g = _mapper.Map<Genero>(viewModel);
            Response r = await _generoService.Update(g);
            if (r.DeuBoa)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Errors = r.Mensagem;
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(GeneroInsertViewModel viewModel)
        {
            Genero genero = _mapper.Map<Genero>(viewModel);

            Response response = await _generoService.Cadastrar(genero);
            if (response.DeuBoa)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Error = response.Mensagem;

            return View();
        }

    }
}
