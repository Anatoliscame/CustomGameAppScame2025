using Microsoft.Xrm.Sdk;
using Plugin.sc_ProductDetails.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.sc_ProductDetails.BusinessLogicPlugins
{
    public class OnCreateUpdateOrderAcquistoCheckExistKeyProductDetailsLogic
    {
        public void ExecuteLogic(IOrganizationService service, Entity entity, ITracingService tracingService)
        {
            tracingService?.Trace("Start Plugin OnCreateUpdateOrderAcquistoCheckExistKeyProductDetails");

            tracingService?.Trace("OrderAcquisto (message Create, Update and Delete) is successfully.");

            ExecuteOrderAcquistoCreate(service, entity, tracingService);

            tracingService?.Trace("End Plugin OnCreateUpdateOrderAcquistoCheckExistKeyProductDetails");
        }

        public void ExecuteOrderAcquistoCreate(IOrganizationService service, Entity target, ITracingService tracingService)
        {
            AcquistoHelper _acquistoHelper = new AcquistoHelper();
            KeyProdottoHelper _keyProductHelper = new KeyProdottoHelper();
            ProdottoDigitaleHelper _prodottoDigitaleHelper = new ProdottoDigitaleHelper();
            Entity entityUpdate = new Entity("acn_ordineacquisto");
            entityUpdate.Id = target.Id;
            Guid acquistoIdRetrive = Guid.Empty;
            Guid videogameIdRetrive = Guid.Empty;
        }
    }
}
