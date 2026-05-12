using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Newtonsoft.Json;
using Plugin.acn_GameApp.Core;
using Plugin.acn_GameApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Services;
using System.Security.Cryptography;
using System.Text.Json.Serialization;

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
                    nuovoAcquisto[Acquisto.Name] = "acquisto" + quantitaAcquisto.ToString() + DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                    nuovoAcquisto[Acquisto.Account] = new EntityReference("account", accountId); // Associa l'account
                    nuovoAcquisto[Acquisto.KeStatusAcquisto] = new OptionSetValue(746200001); // Stato "In Attesa" (Assumendo che il valore sia 100000000)
                    nuovoAcquisto[Acquisto.Code] = GeneraCodiceAcquisto();
                    Guid acquistoId = service.Create(nuovoAcquisto);
                    acquistoIdRetrive = acquistoId;
                    trace.Trace($"Nuovo Acquisto creato: {acquistoTo}");
                }

                entityUpdate["acn_acquistoid"] = new EntityReference(Acquisto.LogicalName, acquistoIdRetrive);
            }
            trace.Trace($"AssignTo {acquistoTo}");

            int? typePiattaforma = ((OptionSetValue)getVideoGameTo.Attributes["acn_typepiattaforma"]).Value;
            // Key Game di Game Based
            List<Entity> keyGameArray = _keyGameHelper.ExistKeyGame(service, videogameTo, 746200000, typePiattaforma); // Disponibile;
            if (keyGameArray.Count == 0) { return; }

            int? tipovideogioco = getVideoGameTo.GetAttributeValue<OptionSetValue>("acn_tipovideogioco")?.Value;
            //int? tipovideogioco = ((OptionSetValue)getVideoGameTo.Attributes["acn_tipovideogioco"]).Value;
            if (tipovideogioco.Value == 746200003) //Espansione
            {
                var contentVideoGames = _videoGameHelper.GeVideoGameWithEspansion(service, videogameTo.Id);  // 746200003 -> Disponibile content
                if (contentVideoGames == null || contentVideoGames.Count <= 0)
                {
                    return;
                }
                /*var batchRequest = new ExecuteMultipleRequest
                {
                    Requests = new OrganizationRequestCollection(),
                    Settings = new ExecuteMultipleSettings
                    {
                        ContinueOnError = false,
                        ReturnResponses = true
                    }
                };*/
                foreach (var content in contentVideoGames)
                {
                    var contentVideoGameGuid = content.GetAttributeValue<Guid>("acn_videogameid");
                    if (contentVideoGameGuid == Guid.Empty) { continue; }

                    var arrayKeyGamesContent = _keyGameHelper.ExistKeyGame(service, new EntityReference("acn_videogame", contentVideoGameGuid), 746200000, typePiattaforma); // Disponibile;
                    if (arrayKeyGamesContent == null || arrayKeyGamesContent.Count == 0){ continue;}

                    //if (keyGamesContent.GetAttributeValue<string>("acn_keygame").Equals())
                    Entity nuovoOrderAcquistoEspansione = new Entity(OrderAcquistoEspansione.LogicalName);
                    nuovoOrderAcquistoEspansione[OrderAcquistoEspansione.OrderAcquistoEspansioneName] = "Name_"+arrayKeyGamesContent.Count + 1 +"_"+ arrayKeyGamesContent[0].GetAttributeValue<string>("acn_keygame");
                    nuovoOrderAcquistoEspansione[OrderAcquistoEspansione.KeyGameCode] = arrayKeyGamesContent[0].GetAttributeValue<string>("acn_keygame");
                    nuovoOrderAcquistoEspansione[OrderAcquistoEspansione.OrdineAcquisto] = new EntityReference("acn_ordineacquisto", target.Id);
                    nuovoOrderAcquistoEspansione[OrderAcquistoEspansione.NameContentVideogame] = content.GetAttributeValue<string>("acn_key");
                    service.Create(nuovoOrderAcquistoEspansione);
                    /*Entity updateKeyGame = new Entity(KeyGame.LogicalName, arrayKeyGamesContent[0].Id)
                    {
                        [KeyGame.StatusPresentKeyGame] = new OptionSetValue(746200002) // Temporaneamente assegnato
                    };*/
                    
                   // batchRequest.Requests.Add(new UpdateRequest { Target = updateKeyGame });
                
                   _keyGameHelper.UpdateKeyGame(service, arrayKeyGamesContent[0].Id, 746200002);// Temporaneamente 

                   // batchRequest.Requests.Add(new CreateRequest { Target = nuovoOrderAcquistoEspansione });

                }
                /*if (batchRequest.Requests.Count > 0)
                {
                    var response = (ExecuteMultipleResponse)service.Execute(batchRequest);
                    HandleBatchResponse(response, trace);
                    trace?.Trace("Batch di creazione eseguito con successo.");
                }
                else
                {
                    trace?.Trace("Nessun nuovo record da creare (tutti esistenti).");
                }*/
            }

            _keyGameHelper.UpdateKeyGame(service, keyGameArray[0].Id, 746200002);// Temporaneamente 

            entityUpdate["acn_keygamecode"] = keyGameArray[0].GetAttributeValue<string>("acn_keygame");// Padre key

            service.Update(entityUpdate);
            trace.Trace($"OrderAcquisto has been updated");

            if (videogameIdRetrive != Guid.Empty)
            {
                _videoGameHelper.UpdateVideoGame(service, videogameIdRetrive, 746200006); //Scegliere Piattaforma
                trace.Trace($"VideoGame has been updated");
            }
        }
        private void HandleBatchResponse(ExecuteMultipleResponse response, ITracingService tracingService)
        {
            var lstErrors = new List<string>();
            foreach (var responseItem in response.Responses)
            {
                if (responseItem.Fault != null)
                {
                    tracingService?.Trace($"Errore nel batch: {responseItem.Fault.Message}");
                    lstErrors.Add(responseItem.Fault.Message);
                }
            }
            if (lstErrors.Count > 0)
            {
                var jsonLogMessages = JsonConvert.SerializeObject(lstErrors);
                tracingService?.Trace(jsonLogMessages);
                throw new InvalidPluginExecutionException(jsonLogMessages);
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
