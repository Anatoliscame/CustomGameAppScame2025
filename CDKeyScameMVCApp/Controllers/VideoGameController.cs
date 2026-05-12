using CDKeyScameMVCApp.Helpers;
using CDKeyScameMVCApp.Models;
using Core.BusinessLogic;
using Core.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;


namespace CDKeyScameMVCApp.Controllers
{
	public class VideoGameController : Controller
    {
        private readonly IBusinessLayer _BL;

        public VideoGameController(IBusinessLayer bl)
        {
            _BL = bl;
        }

        // GET: /VideoGame
        [HttpGet]
        public IActionResult IndexVideoGame()
        {
            List<VideoGame> videogames = _BL.GetVideoGames(); // metodo nel Business Layer

            List<VideoGameViewModel> videogameViewModel = new List<VideoGameViewModel>();
            foreach (var videogame in videogames)
            {
                videogameViewModel.Add(videogame.ToVideoGameModel());
            }

            /*var videogameViewModel = videogames
                .Select(vg => vg.ToVideoGameModel())
                .ToList();*/

            return View(videogameViewModel);
        }
    }
}