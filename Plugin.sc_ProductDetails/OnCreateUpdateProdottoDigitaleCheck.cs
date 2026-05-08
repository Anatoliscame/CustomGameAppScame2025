using Microsoft.Xrm.Sdk;
using Plugin.sc_ProductDetails.BusinessLogicPlugins;
using Plugin.sc_ProductDetails.CorePlugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.sc_ProductDetails
{
    public class OnCreateUpdateProdottoDigitaleCheck : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            try
            {
                var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
                var factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                var service = factory.CreateOrganizationService(context.UserId);
                var trace = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

                OnCreateUpdateProdottoDigitaleCheckLogic bl = new OnCreateUpdateProdottoDigitaleCheckLogic();

                trace.Trace("Start Plugin OnCreateUpdateProdottoDigitaleCheck");

                context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
                if (trace == null)
                    throw new InvalidPluginExecutionException("Failed to retrieve the tracing service.");

                Entity prodottoDigitale =
                            (context.MessageName.ToLower() == "create") ? (Entity)context.InputParameters["Target"] :
                            (context.MessageName.ToLower() == "update") ? Utilities.MergeEntities(context.PreEntityImages["sc_prodottodigitale_pre"], (Entity)context.InputParameters["Target"]) : null;

                if (prodottoDigitale != null)
                {
                    bl.ExecuteLogic(service, prodottoDigitale, context.MessageName, trace);
                }
                else
                {
                    trace?.Trace("prodottoDigitale is null");
                }
                trace?.Trace("End Plugin OnCreateUpdateProdottoDigitaleCheck");
            }
            catch (Exception e)
            {
                throw new InvalidPluginExecutionException(e.Message);
            }

        }

    }
}
