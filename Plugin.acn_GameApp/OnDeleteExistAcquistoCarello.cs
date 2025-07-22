using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Plugin.acn_GameApp.Core;
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
               
                if (!context.InputParameters.Contains("Target") || !(context.InputParameters["Target"] is EntityReference))
                    return;
                 
                var targetRef = (EntityReference)context.InputParameters["Target"];

                if (context.MessageName.ToLower() == "delete")
                {
                    ExecuteAcquistoDelete(service, targetRef, trace);
                }
                trace.Trace("End Plugin OnDeleteExistAcquistoCarello");
            }
            catch (Exception ex)
            {
                trace.Trace($"Error in Plugin OnDeleteExistAcquistoCarello {ex.Message}. \n\r {ex.StackTrace}");
                throw new InvalidPluginExecutionException(ex.Message);
            }
        }

        public void ExecuteAcquistoDelete(IOrganizationService service, EntityReference targetRef, ITracingService trace)
        {
            OrderAcquistoHelper _oderAcquistoHelper = new OrderAcquistoHelper();
            KeyGameHelper _keyGameHelper = new KeyGameHelper();


            //int? optionSetValue = ((OptionSetValue)targetPost.Attributes["acn_kestatusacquisto"]).Value;

            Entity target = service.Retrieve("acn_acquisto", targetRef.Id, new ColumnSet(true));

            var arrayOrderAcquisto = _oderAcquistoHelper.GetOrderAcquisto(service, target);
            if (arrayOrderAcquisto.Count <= 0) { return; }


            Guid keyGameGuid = Guid.Empty;
            int deletedCount = 0;
            foreach (var crmOrderAcquisto in arrayOrderAcquisto)
            {
                if (crmOrderAcquisto == null) continue;
                var keygameId = crmOrderAcquisto.GetAttributeValue<AliasedValue>("OrderAcquistoKeyGame.acn_keygameid");
                if (keygameId == null) continue;

                //EntityReference videoGameidRef = crmOrderAcquisto.Contains("acn_videogameid") ? crmOrderAcquisto.GetAttributeValue<EntityReference>("acn_videogameid") : null;
                //Entity videoGameid = service.Retrieve(videoGameidRef.LogicalName, videoGameidRef.Id, new ColumnSet("acn_tipovideogioco"));
                //int? tipovideogioco = ((OptionSetValue)videoGameid.Attributes["acn_tipovideogioco"]).Value;
                //if (tipovideogioco.Value != 746200003)
                //{ // Espansione

                    keyGameGuid = (Guid)keygameId.Value;

                    _keyGameHelper.UpdateKeyGame(service, keyGameGuid, 746200000); // Disponibile

                    service.Delete("acn_ordineacquisto", crmOrderAcquisto.Id);
                    deletedCount++;
                //}
            }
            trace.Trace($"{deletedCount} OrderAcquisto eliminati e KeyGame aggiornati.");
            return; 
        }
    }
}
