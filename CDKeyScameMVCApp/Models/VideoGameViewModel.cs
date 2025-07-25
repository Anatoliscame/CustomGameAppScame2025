using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CDKeyScameMVCApp.Models
{
	public class VideoGameViewModel
	{
        public Guid VideoGameId { get; set; }
        public string Key { get; set; } // acn_key (Name CRM)
        public string Name { get; set; } // (Name CRM)
        public AccountViewModel Account { get; set; }
        [DataType(DataType.Date)]
        public DateTime DataUscita { get; set; }
        //public VideoGameViewModel ParentVideoGameId { get; set; } ?? DA PENSARE
        public string PG { get; set; }
        public Decimal Prezzo { get; set; }

        public enum Genere
        {
            Action = 746200000,
            Adventure = 746200001,
            Horror = 746200002
        }

        public enum TipoVideoGioco
        {
            Base_Game = 746200000,
            DLC = 746200001,
            Remastered = 746200002,
            Espansione = 746200003,
            Altro = 746200004
        }

        public enum TypePiattaforma
        {
            Steam = 746200000,
            EA = 746200001,
            Ubisoft = 746200002,
            Psn = 746200003,
            Microsoft_Xbox = 746200004,
            Epic_Games = 746200005,
            Scegliere_Piattaforma = 746200006
        }

    }
}