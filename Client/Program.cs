using Core.BusinessLogic;
using Microsoft.Xrm.Tooling.Connector;
using RepositoryCRM;
using RepositoryCRM.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class Program
    {
        static void Main(string[] args)
        {

            var connectionString = System.Configuration.ConfigurationManager.AppSettings["CRM_CustomeAppScameCDKeysVersion2"];
            CrmServiceClient crmServiceClient = new CrmServiceClient(connectionString);

            if (!crmServiceClient.IsReady)
            {
                throw new Exception("cannot instantiate service");
            }


            IBusinessLayer bl = new MainBusinessLayer(new RepositoryVideoGame());

            var videogames = bl.GetVideoGames(crmServiceClient); // Assicurati che il metodo esista nel BL

            // Stampa ogni videogame su console
            foreach (var vg in videogames)
            {
                Console.WriteLine($"{vg.VideoGameId} - {vg.Key} - {vg.Name} - " +
                                  $"{vg.Account?.AccountId} - {vg.DataUscita.ToShortDateString()} - " +
                                  $"{vg.Prezzo} - {vg.tipoVideoGioco} - {vg.tipoPiattaforma} - {vg.StateCode}");
            }

            Console.WriteLine($"Totale videogame: {videogames.Count}");
            Console.WriteLine("Premi un tasto per uscire...");
            Console.ReadKey();
        }
    }
}
