using Core.Model;
using Core.RepositoryInterface;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryCRM.DAL
{
    public class RepositoryVideoGame : IRepositoryVideoGame
    {
        public RepositoryVideoGame()
        {
        }

        public VideoGame Add(VideoGame item)
        {
            throw new NotImplementedException();
        }

        public string CercaVideoGame(string titolo)
        {
            throw new NotImplementedException();
        }

        public bool Delete(VideoGame item)
        {
            throw new NotImplementedException();
        }

        public List<VideoGame> GetAll()
        {
            throw new NotImplementedException();
        }
        public List<VideoGame> GetAllVideoGame(IOrganizationService _service)
        {
            // Query per prendere tutti i videogame
            List<VideoGame> videogames = new List<VideoGame>();

            QueryExpression query = new QueryExpression("acn_videogame")
            {
                ColumnSet = new ColumnSet(true) // tutte le colonne
            };

            // Recupero i record
            EntityCollection results = _service.RetrieveMultiple(query);

            if (results.Entities.Count == 0)
            {
                return videogames; // lista vuota
            }
            // Ciclo su tutti gli Entity
            foreach (var entity in results.Entities)
            {
                VideoGame.VideoGameState? state = entity.Contains("statecode")
                                  ? (VideoGame.VideoGameState?)((OptionSetValue)entity["statecode"]).Value
                                  : null;

                VideoGame.TipoVideoGioco tipoVideoGiocoCRM = VideoGame.TipoVideoGioco.Altro; // default
                if (entity.Contains("acn_tipovideogioco") && entity["acn_tipovideogioco"] is OptionSetValue osv)
                {
                    tipoVideoGiocoCRM = (VideoGame.TipoVideoGioco)osv.Value;
                }

                VideoGame.TypePiattaforma tipoPiattaformaCRM = VideoGame.TypePiattaforma.Scegliere_Piattaforma; // default
                if (entity.Contains("acn_typepiattaforma") && entity["acn_typepiattaforma"] is OptionSetValue tPiatt)
                {
                    tipoPiattaformaCRM = (VideoGame.TypePiattaforma)tPiatt.Value;
                }

                VideoGame vg = new VideoGame
                {
                    VideoGameId = entity.Id,
                    Key = entity.GetAttributeValue<string>("acn_key"),
                    Name = entity.GetAttributeValue<string>("acn_name"),
                    Account = entity.Contains("acn_accountid") ? new Account { AccountId = ((EntityReference)entity["acn_accountid"]).Id } : null,
                    DataUscita = entity.GetAttributeValue<DateTime?>("acn_datauscita") ?? DateTime.MinValue,
                    Prezzo = entity.GetAttributeValue<decimal?>("acn_prezzo") ?? 0m,
                    tipoVideoGioco = tipoVideoGiocoCRM,
                    tipoPiattaforma = tipoPiattaformaCRM,
                    StateCode = state
                };

                videogames.Add(vg);

            }
            return videogames;

        }

        public VideoGame GetById(int id)
        {
            throw new NotImplementedException();
        }

        public string RicercaUnVideoGame(string titolo)
        {
            throw new NotImplementedException();
        }

        public VideoGame Update(VideoGame item)
        {
            throw new NotImplementedException();
        }
    }
}
