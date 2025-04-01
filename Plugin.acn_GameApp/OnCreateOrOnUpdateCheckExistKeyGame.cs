using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Plugin.acn_GameApp.Core;
using System;

namespace Plugin.acn_GameApp
{
    public class OnCreateOrOnUpdateCheckExistKeyGame : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            var service = factory.CreateOrganizationService(context.UserId);
            var trace = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            try
            {
                trace.Trace("Start Plugin OnCreateOrOnUpdateBloccaAcquisteRipetitive");
                context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
                if (trace == null)
                    throw new InvalidPluginExecutionException("Failed to retrieve the tracing service.");

                var target = new Entity();

                if (context.MessageName.ToLower() == "create")
                {
                    target = (Entity)context.InputParameters["Target"];
                    ExecuteAcquisto(service, target, trace);
                }

                trace.Trace("End Plugin OnCreateOrOnUpdateCheckExistKeyGame");
            }
            catch (Exception ex)
            {
                trace.Trace($"Error in Plugin OnCreateOrOnUpdateCheckExistKeyGame {ex.Message}. \n\r {ex.StackTrace}");
                throw new InvalidPluginExecutionException(ex.Message);
            }
        }
        public void ExecuteAcquisto(IOrganizationService service, Entity target, ITracingService trace)
        {
            AcquistoHelper _acquistoHelper = new AcquistoHelper();
            if (target.TryGetAttributeValue("statuscode", out OptionSetValue statuscodeValue) && statuscodeValue != null && statuscodeValue.Value != 0)
            {

                if (!target.TryGetAttributeValue("acn_account", out EntityReference accountTo) || accountTo == null)
                {
                    trace.Trace($"accountTo is null: {target}");
                    throw new Exception("accountTo is not valued");
                }
                if (!target.TryGetAttributeValue("acn_videogame", out EntityReference videogameTo) || videogameTo == null)
                {
                    trace.Trace($"videogameToTo is null: {videogameTo}");
                    throw new Exception("videogameToTo is not valued");
                }

                var keyGameArray = _acquistoHelper.ExistKeyGame(service, target, videogameTo);

                if (keyGameArray.Count <= 0) { throw new Exception("keyGameArray: KeyGame non esistono chiavi"); }

                int quantitaAcquisto = _acquistoHelper.GetAcquisto(service, target).Entities.Count + 1;
                target["acn_name"] = "acquisto" + quantitaAcquisto.ToString();
            }
        }
    }
}
