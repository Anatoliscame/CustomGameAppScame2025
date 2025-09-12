using CDKeyScameMVCApp.Models;
using Core.Model;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CDKeyScameMVCApp.Helpers
{
	public static class Mapping
	{
        public static VideoGameViewModel ToVideoGameModel(this VideoGame videogame)
        {
            /*ICollection<OrdineAcquistoViewModel> ordineAcquistoSModel = new List<OrdineAcquistoViewModel>();
            foreach (var item in videogame.OrdineAcquistoS)
            {
                ordineAcquistoSModel.Add(item?.VisOrdineAcquisto());
            }*/

            return new VideoGameViewModel
            {
                VideoGameId = videogame.VideoGameId,
                Key = videogame.Key,
                Name = videogame.Name,
                Account = videogame.Account != null ? new AccountViewModel
                {
                    AccountId = videogame.Account.AccountId,
                    Name = videogame.Account.Name
                }
                : null,
                DataUscita = videogame.DataUscita,
                Prezzo = videogame.Prezzo,
                tipoVideoGioco = (int?)videogame.tipoVideoGioco,
                tipoPiattaforma = (int?)videogame.tipoPiattaforma,
                StateCode = (int?)videogame.StateCode

            };
        }
    }
}