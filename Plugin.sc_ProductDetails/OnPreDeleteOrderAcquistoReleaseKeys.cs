using Microsoft.Xrm.Sdk;
using Plugin.sc_ProductDetails.BusinessLogicPlugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Services;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.sc_ProductDetails
{
    public class OnPreDeleteOrderAcquistoReleaseKeys : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            try
            {
                var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
                var factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                var service = factory.CreateOrganizationService(context.UserId);
                var trace = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

                OnPreDeleteOrderAcquistoReleaseKeysLogic bl = new OnPreDeleteOrderAcquistoReleaseKeysLogic();

                trace.Trace("Start Plugin OnPreDeleteOrderAcquistoReleaseKeys");
                context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
                if (trace == null)
                    throw new InvalidPluginExecutionException("Failed to retrieve the tracing service.");


                Entity preImage = context.PreEntityImages.Contains("sc_acquisto_pre") ? context.PreEntityImages["sc_acquisto_pre"] : null;

                if (context.MessageName.ToLower() == "delete")
                {
                    bl.ExecuteLogic(service, preImage, trace);
                }
                trace?.Trace("End Plugin OnPreDeleteOrderAcquistoReleaseKeys");


            }
            catch (Exception e)
            {
                throw new InvalidPluginExecutionException(e.Message);
            }

        }
    }
}
