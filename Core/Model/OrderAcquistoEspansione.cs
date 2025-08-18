using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class OrderAcquistoEspansione
    {
        public Guid OrderAcquistoEId { get; set; }
        public string Name { get; set; }
        public string KeyGameCode { get; set; }
        public string NameContentVideoGame { get; set; }
        public OrdineAcquisto OrdineAcquisto { get; set; }
    }
}
