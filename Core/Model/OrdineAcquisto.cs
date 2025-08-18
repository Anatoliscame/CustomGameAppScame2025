using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class OrdineAcquisto
    {
        public Guid OrderAcquistoId { get; set; }
        public string Name { get; set; }
        public Account Account { get; set; }
        public Acquisto Acquisto { get; set; }
        public VideoGame VideoGameId { get; set; }
        public string KeyGameCode { get; set; }  // Relazionato con KeyGame
        public string OrderName { get; set; }

        public ICollection<OrderAcquistoEspansione> OrderAcquistoS { get; set; } = new List<OrderAcquistoEspansione>();

    }
}
