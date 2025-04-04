using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Plugin.acn_GameApp.Core;
using System;
using System.Collections.Generic;
using System.IdentityModel.Metadata;
using System.Linq;

namespace Plugin.acn_GameApp
{
    public class OnCreateOnUpdateCheckExistOrderAcquisto : IPlugin
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
                /*if (context.MessageName.ToLower() == "update")
                {
                    target = context.PostEntityImages.Values?.FirstOrDefault();
                }*/

                var targetPost = context.PostEntityImages.Values?.FirstOrDefault();
                ExecuteAcquisto(service, target, trace);

                trace.Trace("End Plugin OnCreateOnUpdateCheckExistOrderAcquisto");
            }
            catch (Exception ex)
            {
                trace.Trace($"Error in Plugin OnCreateOnUpdateCheckExistOrderAcquisto {ex.Message}. \n\r {ex.StackTrace}");
                throw new InvalidPluginExecutionException(ex.Message);
            }
        }
        public void ExecuteAcquisto(IOrganizationService service, Entity target, ITracingService trace)
        {
            AcquistoHelper _acquistoHelper = new AcquistoHelper();
            KeyGameHelper _keyGameHelper = new KeyGameHelper();

            Entity entityUpdate = new Entity("acn_acquisto");
            entityUpdate.Id = target.Id;

            if (target.TryGetAttributeValue("statuscode", out OptionSetValue statuscodeValue) && statuscodeValue != null && statuscodeValue.Value != 0)
            {
                //746200001
                if (!target.TryGetAttributeValue("acn_account", out EntityReference accountTo) || accountTo == null)
                {
                    trace.Trace($"accountTo is null: {target}");
                    throw new Exception("accountTo is not valued");
                }

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
        }
    }
}
