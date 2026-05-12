using Microsoft.Xrm.Sdk;
using Plugin.sc_ProductDetails.BusinessLogicPlugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.sc_ProductDetails
{
    public class OnCreateUpdateOrderAcquistoCheckExistKeyProductDetails : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            try
            {
                var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
                var factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                var service = factory.CreateOrganizationService(context.UserId);
                var trace = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

                OnCreateUpdateOrderAcquistoCheckExistKeyProductDetailsLogic bl = new OnCreateUpdateOrderAcquistoCheckExistKeyProductDetailsLogic();

                trace.Trace("Start Plugin OnCreateUpdateOrderAcquistoCheckExistKeyProductDetails");
                context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
                if (trace == null)
                    throw new InvalidPluginExecutionException("Failed to retrieve the tracing service.");

                var target = new Entity();

                if (context.MessageName.ToLower() == "create")
                {
                    target = (Entity)context.InputParameters["Target"];
                    bl.ExecuteLogic(service, target, trace);

                    trace?.Trace("End Plugin OnCreateUpdateOrderAcquistoCheckExistKeyProductDetails");
                }

            }
            catch (Exception e)
            {
                throw new InvalidPluginExecutionException(e.Message);
            }
        }
    }
}
