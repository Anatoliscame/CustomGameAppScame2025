using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class VideoGame
    {
        public Guid VideoGameId { get; set; }
        public string Key { get; set; } // acn_key (Name CRM)
        public string Name { get; set; } // (Name CRM)
        public Account Account { get; set; }
        public DateTime DataUscita { get; set; }
        //public VideoGameViewModel ParentVideoGameId { get; set; } ?? DA PENSARE
        public string PG { get; set; }
        public Decimal Prezzo { get; set; }

        //[AttributeLogicalName("statecode")]
        public VideoGameState? StateCode { get; set; }

        public ICollection<KeyGame> KeyGameS { get; set; } = new List<KeyGame>();
        public ICollection<OrdineAcquisto> OrdineAcquistoS { get; set; } = new List<OrdineAcquisto>();
        public enum Genere
        {
            Action = 746200000,
            Adventure = 746200001,
            Horror = 746200002
        }

        public TipoVideoGioco tipoVideoGioco { get; set; }
        public enum TipoVideoGioco
        {
            Base_Game = 746200000,
            DLC = 746200001,
            Remastered = 746200002,
            Espansione = 746200003,
            Altro = 746200004
        }

        public TypePiattaforma tipoPiattaforma { get; set; }

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

        public enum VideoGameState
        {
            Active = 0,
            Inactive = 1
        }
        public string VisVideoGame()
        {
            return $"{VideoGameId} - {Key} - {Account?.AccountId} - {DataUscita.ToShortDateString()} - {PG} - {Prezzo} - {tipoVideoGioco} - {tipoPiattaforma}";
        }
    }
}
