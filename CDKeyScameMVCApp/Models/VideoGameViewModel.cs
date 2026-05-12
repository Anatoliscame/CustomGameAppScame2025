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

       // [Required]
        public string Key { get; set; } // acn_key (Name CRM)
       // [Required]
        public string Name { get; set; } // (Name CRM)
        public AccountViewModel Account { get; set; }
        //[DataType(DataType.Date)]
        public DateTime DataUscita { get; set; }
        //public VideoGameViewModel ParentVideoGameId { get; set; } ?? DA PENSARE
        public string PG { get; set; }
        public Decimal Prezzo { get; set; }

        public ICollection<KeyGameViewModel> KeyGameS { get; set; } = new List<KeyGameViewModel>();
        public ICollection<OrdineAcquistoViewModel> OrdineAcquistoS { get; set; } = new List<OrdineAcquistoViewModel>();
        public enum Genere
        {
            Action = 746200000,
            Adventure = 746200001,
            Horror = 746200002
        }

        public int? tipoVideoGioco { get; set; }

        public int? tipoPiattaforma { get; set; }

        public int? StateCode { get; set; }
        public string VisVideoGameView()
        {
            return $"{VideoGameId} - {Key} - {Account?.AccountId} - {DataUscita.ToShortDateString()} - {PG} - {Prezzo} - {tipoVideoGioco} - {tipoPiattaforma} - {StateCode}";
        }
    }
}