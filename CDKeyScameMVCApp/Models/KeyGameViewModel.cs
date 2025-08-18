using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CDKeyScameMVCApp.Models
{
	public class KeyGameViewModel
	{
        public Guid KeyGameId { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Codice Chiave")]
        public string KeyGameName { get; set; }
        [Display(Name = "Stato")]
        public int StatusPresentKeyGame { get; set; } // usa i codici OptionSet
        public VideoGameViewModel VideoGameId { get; set; }

        public enum StatusPresentKeyGameEnum
        {
            Disponibile = 746200000,
            Indisponibile = 746200001,
            Temporaneamente = 746200002,
            Temporaneamente_Acquistato = 746200003
        }

        public enum TypePiattaformaEnum
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