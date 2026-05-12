using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using Plugin.acn_GameApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginTestConsole.Wrapper
{
    class OnCreateOnUpdateCheckExistOrderAcquistoWrapper
    {
        public void Execute(IOrganizationService service, string guid)
        {

            Entity target = GetTarget(service, guid);

            ITracingService tracingService = service as ITracingService;

            OnUpdateCheckExistOrderAcquisto plugin = new OnUpdateCheckExistOrderAcquisto();

            plugin.ExecuteAcquistoUpdate(service, target, tracingService);

        }

        private Entity GetTarget(IOrganizationService service, string guid)
        {
            var targetEntity = service.Retrieve("acn_acquisto", new Guid(guid), new Microsoft.Xrm.Sdk.Query.ColumnSet(true));
            return targetEntity;
        }
    }
}
