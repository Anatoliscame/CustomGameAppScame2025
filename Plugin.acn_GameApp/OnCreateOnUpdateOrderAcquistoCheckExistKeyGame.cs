using Microsoft.Xrm.Sdk;
using Plugin.acn_GameApp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.acn_GameApp
{
    public class OnCreateOnUpdateOrderAcquistoCheckExistKeyGame : IPlugin
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
                }
                if (context.MessageName.ToLower() == "update")
                {
                    target = context.PostEntityImages.Values?.FirstOrDefault();
                }
                ExecuteAcquisto(service, target, trace);

                trace.Trace("End Plugin OnCreateOnUpdateOrderAcquistoCheckExistKeyGame");
            }
            catch (Exception ex)
            {
                trace.Trace($"Error in Plugin OnCreateOnUpdateOrderAcquistoCheckExistKeyGame {ex.Message}. \n\r {ex.StackTrace}");
                throw new InvalidPluginExecutionException(ex.Message);
            }
        }
        public void ExecuteAcquisto(IOrganizationService service, Entity target, ITracingService trace)
        {
            KeyGameHelper _keyGameHelper = new KeyGameHelper();

            if (!target.TryGetAttributeValue("acn_acquisto", out EntityReference acquistoTo) || acquistoTo == null)
            {
                trace.Trace($"acquistoTo is null: {target}");
                throw new Exception("acquistoTo is not valued");
            }
            if (!target.TryGetAttributeValue("acn_videogame", out EntityReference videogameTo) || videogameTo == null)
            {
                trace.Trace($"videogameToTo is null: {videogameTo}");
                throw new Exception("videogameToTo is not valued");
            }

                List<Entity> keyGameArray = _keyGameHelper.ExistKeyGame(service, videogameTo);

                if (keyGameArray.Count <= 0) { throw new Exception("keyGameArray: Chiavi disponibili con un video gioco non ci sono"); }

                _keyGameHelper.CreateKeyGame(service, keyGameArray);
                trace.Trace($"OrderAcquisto has been updated");
        }
    }
}
