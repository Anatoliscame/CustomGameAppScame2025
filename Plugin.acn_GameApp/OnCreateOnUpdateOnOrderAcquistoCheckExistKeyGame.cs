using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Plugin.acn_GameApp.Core;
using Plugin.acn_GameApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Services;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.acn_GameApp
{
    public class OnCreateOnUpdateOnOrderAcquistoCheckExistKeyGame : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            var service = factory.CreateOrganizationService(context.UserId);
            var trace = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            try
            {
                trace.Trace("Start Plugin OnCreateOnUpdateOrderAcquistoCheckExistKeyGame");
                context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
                if (trace == null)
                    throw new InvalidPluginExecutionException("Failed to retrieve the tracing service.");

                var target = new Entity();

                if (context.MessageName.ToLower() == "create")
                {
                    target = (Entity)context.InputParameters["Target"];
                    ExecuteOrderAcquistoCreate(service, target, trace);
                }
                if (context.MessageName.ToLower() == "update")
                {
                    target = context.PostEntityImages.Values?.FirstOrDefault();

                }
                 
                trace.Trace("End Plugin OnCreateOnUpdateOrderAcquistoCheckExistKeyGame");
            }
            catch (Exception ex)
            {
                trace.Trace($"Error in Plugin OnCreateOnUpdateOrderAcquistoCheckExistKeyGame {ex.Message}. \n\r {ex.StackTrace}");
                throw new InvalidPluginExecutionException(ex.Message);
            }
        }
        public void ExecuteOrderAcquistoCreate(IOrganizationService service, Entity target, ITracingService trace)
        {
            AcquistoHelper _acquistoHelper = new AcquistoHelper();
            KeyGameHelper _keyGameHelper = new KeyGameHelper();
            VideoGameHelper _videoGameHelper = new VideoGameHelper();
            Entity entityUpdate = new Entity("acn_ordineacquisto");
            entityUpdate.Id = target.Id;
            Guid acquistoIdRetrive = Guid.Empty;
            Guid videogameIdRetrive = Guid.Empty;

            if (!target.TryGetAttributeValue("acn_videogameid", out EntityReference videogameTo))
            {
                trace.Trace($"videogameToTo is null: {videogameTo}");
                return;
            }
            Entity getVideoGameTo = service.Retrieve(videogameTo.LogicalName, videogameTo.Id, new ColumnSet(true));

            if (!target.TryGetAttributeValue("acn_acquistoid", out EntityReference acquistoTo))
            {
                trace.Trace($"acquistoTo is null: {acquistoTo}");
                //List<Entity> getVideoGames = _videoGameHelper.GeVideoGames(service, target);              
                //TypeVideoGame(service, getVideoGameTo);
                //var geEspansion = _videoGameHelper.GeVideoGameWithEspansion(service, getVideoGameTo, 746200003); // 746200003 -> Disponibile
                //if (geEspansion.Count == 0) { return; }
                Guid accountId = getVideoGameTo.GetAttributeValue<EntityReference>("acn_accountid")?.Id ?? Guid.Empty;
                videogameIdRetrive = getVideoGameTo.GetAttributeValue<Guid>("acn_videogameid");
                List<Entity> acquistiInattesa = _acquistoHelper.GetAcquistoInAttesa(service, accountId);
                if (acquistiInattesa.Count > 0)
                {
                    Guid acquistoId = acquistiInattesa[0].GetAttributeValue<Guid>("acn_acquistoid");
                    acquistoIdRetrive = acquistoId;
                }
                else
                {
                    int quantitaAcquisto = _acquistoHelper.GetAcquisto(service, target).Entities.Count + 1;

                    Entity nuovoAcquisto = new Entity(Acquisto.LogicalName);
                    nuovoAcquisto["acn_name"] = "acquisto" + quantitaAcquisto.ToString() + DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                    nuovoAcquisto["acn_account"] = new EntityReference("account", accountId); // Associa l'account
                    nuovoAcquisto["acn_kestatusacquisto"] = new OptionSetValue(746200001); // Stato "In Attesa" (Assumendo che il valore sia 100000000)
                    nuovoAcquisto["acn_code"] = GeneraCodiceAcquisto();
                    Guid acquistoId = service.Create(nuovoAcquisto);
                    acquistoIdRetrive = acquistoId;
                    trace.Trace($"Nuovo Acquisto creato: {acquistoTo}");
                }

                entityUpdate["acn_acquistoid"] = new EntityReference(Acquisto.LogicalName, acquistoIdRetrive);
            }
            trace.Trace($"AssignTo {acquistoTo}");

            int? typePiattaforma = ((OptionSetValue)getVideoGameTo.Attributes["acn_typepiattaforma"]).Value;
             
            List<Entity> keyGameArray = _keyGameHelper.ExistKeyGame(service, videogameTo, 746200000, typePiattaforma); // Disponibile
            Guid keyGameGuid = keyGameArray[0].Id;
            _keyGameHelper.UpdateKeyGame(service, keyGameGuid, 746200002);// Temporaneamente 

            entityUpdate["acn_keygamecode"] = keyGameArray[0].GetAttributeValue<string>("acn_keygame");
            service.Update(entityUpdate);
            trace.Trace($"OrderAcquisto has been updated");

            if (videogameIdRetrive != Guid.Empty)
            {
                _videoGameHelper.UpdateVideoGame(service, videogameIdRetrive, 746200006); //Scegliere Piattaforma
                trace.Trace($"VideoGame has been updated");
            }

        }

        private string GeneraCodiceAcquisto(int lunghezza = 6)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return  "ACQ-" + new string(Enumerable.Repeat(chars, 6)
                                              .Select(s => s[random.Next(s.Length)]).ToArray());
        }


        public void TypeVideoGame(IOrganizationService service, Entity target)
        {
            if (!target.TryGetAttributeValue("acn_tipovideogioco", out OptionSetValue tipovideogioco))
            {
                throw new InvalidPluginExecutionException("Error: the VideoGame record has empty VideoGame Step field.");
            }

            switch (tipovideogioco.Value)
            {
                case 133280000: //Base Game

                    break;

                case 133280001: //DLC
                    break;

                case 133280002: //Remastered

                    break;

                case 133280003: //Espansione

                    break;

                case 133280004: //Altro

                    break;

                default:
                    break;
            }

        }
    }
}
