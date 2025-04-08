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
                    ExecuteAcquistoUpdate(service, target, trace);
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
            AcquistoHelper _acquistoHelper = new AcquistoHelper();
            KeyGameHelper _keyGameHelper = new KeyGameHelper();

            Entity entityUpdate = new Entity("acn_acquisto");
            entityUpdate.Id = target.Id;


                /*if (statuscodeValue.Value == 746200001) // Effetuato
                {
                }*/

                List<Entity> acquistiInattesa = _acquistoHelper.GetAcquistoTargetAndInAttesa(service, target);
                if (acquistiInattesa.Count > 0)
                {
                    throw new Exception("Non e' possibile creare nuovo Acquisto in attesa, perche esiste un'altro non e' Effetuato");
                }
                int quantitaAcquisto = _acquistoHelper.GetAcquisto(service, target).Entities.Count + 1;
                entityUpdate["acn_name"] = "acquisto" + quantitaAcquisto.ToString();
                //entityUpdate["acn_keygamecode"] = keyGameArray[0].GetAttributeValue<string>("acn_keygame");
                service.Update(entityUpdate);
                trace.Trace($"Acquisto has been updated");
            
        }

        public void ExecuteAcquistoPost(IOrganizationService service, Entity targetPost, ITracingService trace)
        {
            AcquistoHelper _acquistoHelper = new AcquistoHelper();
            KeyGameHelper _keyGameHelper = new KeyGameHelper();

            if (targetPost.TryGetAttributeValue("acn_acquistoid", out EntityReference acquistoTo) || acquistoTo != null)
            {
                if (targetPost.Attributes.Contains(Acquisto.StatusReason) && targetPost.Attributes[Acquisto.StatusReason] != null)
                {
                    int? optionSetValue = ((OptionSetValue)targetPost.Attributes[Acquisto.StatusReason]).Value;

                    switch (optionSetValue)
                    {
                        case 746200001:// Effetuato

                           /* List<Entity> keyGameArray = _keyGameHelper.ExistKeyGame(service, videogameTo, 746200000); // Disponibile
                            foreach
                            Entity updateKeyGame = new Entity("acn_keygame");
                            updateKeyGame.Id = */
                            break;

                        case 746200002:// In attesa
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
