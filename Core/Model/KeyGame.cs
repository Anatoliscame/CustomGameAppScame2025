using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class KeyGame
    {
        public Guid KeyGameId { get; set; }
        public string Name { get; set; }
        public string KeyGameName { get; set; }
        public int StatusPresentKeyGame { get; set; } // usa i codici OptionSet
        public VideoGame VideoGameId { get; set; }

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
