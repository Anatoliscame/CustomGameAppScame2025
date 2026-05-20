
using Microsoft.Xrm.Sdk;
using Plugin.sc_DigitalProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginTestConsole.Wrapper
{
    class OnPostUpdatePurchaseFinalizeKeysWrapper
    {
        public void Execute(IOrganizationService service, string guid)
        {

            Entity target = GetTarget(service, guid);

            ITracingService tracingService = service as ITracingService;

            OnPostUpdatePurchaseFinalizeKeys plugin = new OnPostUpdatePurchaseFinalizeKeys();

            //plugin.Execute(service, target, tracingService);

        }

        private Entity GetTarget(IOrganizationService service, string guid)
        {
            var targetEntity = service.Retrieve("acn_acquisto", new Guid(guid), new Microsoft.Xrm.Sdk.Query.ColumnSet(true));
            return targetEntity;
        }
    }
}
