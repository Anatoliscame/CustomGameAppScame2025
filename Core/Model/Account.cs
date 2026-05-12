using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class Account
    {
        public Guid AccountId { get; set; }
        public string Name { get; set; }
        public ICollection<Acquisto> AcquistoS { get; set; } = new List<Acquisto>();
        public ICollection<VideoGame> VideoGameS { get; set; } = new List<VideoGame>();
        public ICollection<OrdineAcquisto> OrdineAcquistoS { get; set; } = new List<OrdineAcquisto>();

    }
}
