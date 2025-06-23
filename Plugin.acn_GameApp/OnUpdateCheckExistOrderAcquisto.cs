using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Plugin.acn_GameApp.Core;
using Plugin.acn_GameApp.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Metadata;
using System.Linq;

namespace Plugin.acn_GameApp
{
    public class OnUpdateCheckExistOrderAcquisto : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            var service = factory.CreateOrganizationService(context.UserId);
            var trace = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            try
            {
                trace.Trace("Start Plugin OnCreateOnUpdateCheckExistOrderAcquisto");
                context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
                if (trace == null)
                    throw new InvalidPluginExecutionException("Failed to retrieve the tracing service.");

                var target = new Entity();

                if (context.MessageName.ToLower() == "create") 
                {
                    target = (Entity)context.InputParameters["Target"];
                    
                }
                if (context.MessageName.ToLower() == "update")
                {
                    target = context.PostEntityImages.Values?.FirstOrDefault();
                    if (target.Attributes.Contains("acn_kestatusacquisto") && target["acn_kestatusacquisto"] != null)
                    {
                        int? optionSetValue = ((OptionSetValue)target.Attributes["acn_kestatusacquisto"]).Value;

                        switch (optionSetValue)
                        {
                            case 746200000: //Effetuato
                                            //if (optionSetValue != 746200001) { throw new ApplicationException("Non sai ancora esprimere la parola 'Mamma'  3+"); }
                                ExecuteAcquistoUpdate(service, target, trace);
                                break;
                            default:
                                return;
                        }
                    }
                }

                trace.Trace("End Plugin OnCreateOnUpdateCheckExistOrderAcquisto");
            }
            catch (Exception ex) 
            {
                trace.Trace($"Error in Plugin OnCreateOnUpdateCheckExistOrderAcquisto {ex.Message}. \n\r {ex.StackTrace}");
                throw new InvalidPluginExecutionException(ex.Message);
            }
        }
        public void ExecuteAcquistoUpdate(IOrganizationService service, Entity target, ITracingService trace)
        {
            OrderAcquistoHelper _oderAcquistoHelper = new OrderAcquistoHelper();
            KeyGameHelper _keyGameHelper = new KeyGameHelper();

            var arrayOrderAcquisto = _oderAcquistoHelper.GetOrderAcquisto(service, target);
            if (arrayOrderAcquisto.Count <= 0) { return; }


            Guid keyGameGuid = Guid.Empty;

            foreach (var crmOrderAcquisto in arrayOrderAcquisto)
            {
                var keygameId = crmOrderAcquisto.GetAttributeValue<AliasedValue>("OrderAcquistoKeyGame.acn_keygameid");
                //var statusKeyGame = crmOrderAcquisto.GetAttributeValue<AliasedValue>("OrderAcquistoKeyGame.acn_statuspresentkeygame");
                keyGameGuid = (Guid)keygameId.Value;

               _keyGameHelper.UpdateKeyGame(service, keyGameGuid, 746200003); // Temporaneamente Acquistato
            }
            trace.Trace($"Acquisto has been updated");
            return;
        }

        public void ExecuteAcquistoPost(IOrganizationService service, Entity targetPost, ITracingService trace)
        {
            AcquistoHelper _acquistoHelper = new AcquistoHelper();
            KeyGameHelper _keyGameHelper = new KeyGameHelper();

            if (targetPost.TryGetAttributeValue("acn_acquistoid", out EntityReference acquistoTo) || acquistoTo != null)
            {
                if (targetPost.Attributes.Contains("acn_kestatusacquisto") && targetPost.Attributes["acn_kestatusacquisto"] != null)
                {
                    int? optionSetValue = ((OptionSetValue)targetPost.Attributes["acn_kestatusacquisto"]).Value;

                    switch (optionSetValue)
                    {
                        case 746200000:// Effetuato

                           /* List<Entity> keyGameArray = _keyGameHelper.ExistKeyGame(service, videogameTo, 746200000); // Disponibile
                            foreach
                            Entity updateKeyGame = new Entity("acn_keygame");
                            updateKeyGame.Id = */
                            break;

                        case 746200001:// In attesa
                            throw new ApplicationException("il valore di stato ordine selezionato 'Completato', acquisto non puo essere eliminato");

                        //case 746200003:// Annullato

                        //    break;

                        default:
                            break;
                    }
                }
            }
        }
    }
}
