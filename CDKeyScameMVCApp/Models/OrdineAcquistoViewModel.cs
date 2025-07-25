using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CDKeyScameMVCApp.Models
{
	public class OrdineAcquistoViewModel
	{
        public Guid OrderAcquistoId { get; set; }
        public string Name { get; set; }
        public AccountViewModel Account { get; set; }
        public AcquistoViewModel Acquisto { get; set; }
        public VideoGameViewModel VideoGameId { get; set; }
        public string KeyGameCode { get; set; }  // Relazionato con KeyGame
        public string OrderName { get; set; }

        public ICollection<OrderAcquistoEspansioneViewModel> OrderAcquistoS { get; set; } = new List<OrderAcquistoEspansioneViewModel>();

    }
}