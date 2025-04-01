using Microsoft.Xrm.Sdk;
using Plugin.acn_GameApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginTestConsole.Wrapper
{
    class OnCreateOrOnUpdateCheckExistKeyGameWrapper
    {
        public void Execute(IOrganizationService service, string guid)
        {
            var target = GetTarget(service, guid);

            ITracingService tracingService = service as ITracingService;

            OnCreateOrOnUpdateCheckExistKeyGame plugin = new OnCreateOrOnUpdateCheckExistKeyGame();

            plugin.ExecuteAcquisto(service, target, tracingService);

        }

        private Entity GetTarget(IOrganizationService service, string guid)
        {
            var targetEntity = service.Retrieve("acn_acquisto", new Guid(guid), new Microsoft.Xrm.Sdk.Query.ColumnSet(true));
            return targetEntity;
        }
    }
}
