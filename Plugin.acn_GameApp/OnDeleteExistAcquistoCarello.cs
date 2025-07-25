using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Plugin.acn_GameApp.Core;
using Plugin.acn_GameApp.Entities;
using System;


namespace Plugin.acn_GameApp
{
    public class OnDeleteExistAcquistoCarello : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            var service = factory.CreateOrganizationService(context.UserId);
            var trace = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            try
            {
                trace.Trace("Start Plugin OnDeleteExistAcquistoCarello");
                //context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
                if (trace == null)
                    throw new InvalidPluginExecutionException("Tracing service is null.");
               
               //if (!context.InputParameters.Contains("Target") || !(context.InputParameters["Target"] is EntityReference))
                //    return;
                if (!context.PreEntityImages.Contains("PreImage"))// || !(context.InputParameters["PreImage"] is EntityReference))
                    return;
                //var targetRef = (EntityReference)context.InputParameters["Target"];

                Entity preImage = context.PreEntityImages["PreImage"];

                if (context.MessageName.ToLower() == "delete")
                {
                    ExecuteAcquistoDelete(service, preImage, trace);
                }
                trace.Trace("End Plugin OnDeleteExistAcquistoCarello");
            }
            catch (Exception ex)
            {
                trace.Trace($"Error in Plugin OnDeleteExistAcquistoCarello {ex.Message}. \n\r {ex.StackTrace}");
                throw new InvalidPluginExecutionException(ex.Message);
            }
        }

        public void ExecuteAcquistoDelete(IOrganizationService service, Entity preImage, ITracingService trace)
        {
            OrderAcquistoHelper _oderAcquistoHelper = new OrderAcquistoHelper();
            KeyGameHelper _keyGameHelper = new KeyGameHelper();
            VideoGameHelper _videoGameHelper = new VideoGameHelper();

            //int? optionSetValue = ((OptionSetValue)targetPost.Attributes["acn_kestatusacquisto"]).Value;

            //Entity target = service.Retrieve("acn_acquisto", targetRef.Id, new ColumnSet(true));

            var arrayOrderAcquisto = _oderAcquistoHelper.GetOrderAcquisto(service, preImage);
            if (arrayOrderAcquisto.Count <= 0) { return; }


            Guid keyGameGuid = Guid.Empty;
            int deletedCount = 0;
            trace?.Trace($"Recupero la lista di OrdineAcquisto: {arrayOrderAcquisto.Count} ");

            foreach (var crmOrderAcquisto in arrayOrderAcquisto)
            {
                if (crmOrderAcquisto == null) continue;
                var keygameId = crmOrderAcquisto.GetAttributeValue<AliasedValue>("OrderAcquistoKeyGame.acn_keygameid");
                if (keygameId == null) continue;
                trace?.Trace($"Viene ciclato Ogni OrdineAcquisto");

                var videogameId = crmOrderAcquisto.GetAttributeValue<EntityReference>(VideoGame.VideogameId);
                Entity getVideoGameTo = service.Retrieve(VideoGame.LogicalName, videogameId.Id, new ColumnSet(true));
                trace?.Trace($"VideoGame di OrdineAcquisto recuperato: {getVideoGameTo.Id.ToString()}");
                int? tipovideogioco = getVideoGameTo.GetAttributeValue<OptionSetValue>(VideoGame.TipoVideogioco)?.Value;
                int? typePiattaforma = ((OptionSetValue)getVideoGameTo.Attributes[VideoGame.TypePiattaforma]).Value;
                trace?.Trace($"Tipo Video Gioco: {tipovideogioco.Value} \n Tipo Piattaforma: {typePiattaforma.Value}");
                if (tipovideogioco.Value == 746200003)
                { //Espansione
                    trace?.Trace($"Hai scelto VideoGame di tipo Espansione");
                    var contentVideoGames = _videoGameHelper.GeVideoGameWithEspansion(service, getVideoGameTo.Id);  // 746200003 -> Disponibile content
                    if (contentVideoGames == null || contentVideoGames.Count <= 0)
                    {
                        return;
                    }
                    trace?.Trace($"Recupero di tutti contenuti di videogame base: N -> {contentVideoGames.Count}");
                    foreach (var content in contentVideoGames)
                    {
                        //trace?.Trace($"");
                        var videoGameId = content.GetAttributeValue<Guid>(VideoGame.VideogameId);
                        if (videoGameId == Guid.Empty) { //continue;
                            throw new InvalidPluginExecutionException($"Non esiste videgame: {videoGameId.ToString()} ");
                        }
                        trace?.Trace($"Il contenuto di VideoGame: {videoGameId}");
                    // Da risolvere -->
                        var arrayKeyGamesContent = _keyGameHelper.ExistKeyGame(service, new EntityReference("acn_videogame", videoGameId), 746200002, typePiattaforma); // Temporaneamente;
                        trace?.Trace($"Un elenco di KeyGame (chiavi di contenuti disponibili): {arrayKeyGamesContent.Count}");
                        if (arrayKeyGamesContent.Count == 0) { //continue;
                            throw new InvalidPluginExecutionException($"La lista di KeyGamesContent: {arrayKeyGamesContent.Count} \n Mentre DLC esiste {videoGameId}, e il numero di DLC sono: {contentVideoGames.Count} ");
                        }

                        _keyGameHelper.UpdateKeyGame(service, arrayKeyGamesContent[0].Id, 746200000);// Disponibile 

                    }
                }
                keyGameGuid = (Guid)keygameId.Value;

                _keyGameHelper.UpdateKeyGame(service, keyGameGuid, 746200000); // Disponibile
                trace?.Trace($"Effetuata un UPDATE di stato acn_keygame genitore");
                service.Delete("acn_ordineacquisto", crmOrderAcquisto.Id);
                deletedCount++;
                //}
            }
            trace.Trace($"{deletedCount} OrderAcquisto eliminati e KeyGame aggiornati.");
            return; 
        }
    }
}
