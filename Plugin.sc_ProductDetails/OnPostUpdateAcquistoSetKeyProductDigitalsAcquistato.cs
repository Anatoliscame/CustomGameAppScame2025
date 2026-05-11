using Microsoft.Xrm.Sdk;
using Plugin.sc_ProductDetails.BusinessLogicPlugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.sc_ProductDetails
{
    public class OnPostUpdateAcquistoSetKeyProductDigitalsAcquistato : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            try
            {
                var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
                var factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                var service = factory.CreateOrganizationService(context.UserId);
                var trace = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

                OnPostUpdateAcquistoSetKeyProductDigitalsAcquistatoLogic bl = new OnPostUpdateAcquistoSetKeyProductDigitalsAcquistatoLogic();

                if (trace == null)
                    throw new InvalidPluginExecutionException("Failed to retrieve the tracing service.");

                trace.Trace("Start Plugin OnPostUpdateAcquistoSetKeyProductDigitalsAcquistato");

                var target = new Entity();

                if (context.MessageName.ToLower() == "update")
                {
                    target = context.PostEntityImages.Contains("sc_acquisto_post") ? context.PostEntityImages["sc_acquisto_post"] : null;

                    bl.ExecuteLogic(service, target, trace);
                }

                trace.Trace("End Plugin OnPostUpdateAcquistoSetKeyProductDigitalsAcquistato");

            }
            catch (Exception e)
            {
                throw new InvalidPluginExecutionException(e.Message);
            }

        }

    }
}
