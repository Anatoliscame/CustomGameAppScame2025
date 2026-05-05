using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Plugin.sc_ProductDetails.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.sc_ProductDetails.Helper
{
    public class ProdottoDigitaleHelper
    {
        public ProdottoDigitaleHelper() { }

        public List<Entity> GeProdottoDigitale(IOrganizationService service, Entity target)
        {

            QueryExpression query = new QueryExpression(ProdottoDigitale.LogicalName)
            {
                ColumnSet = new ColumnSet(true),
                Criteria = new FilterExpression()
            };
            query.Criteria.AddCondition(ProdottoDigitale.ProdottoDigitaleId, ConditionOperator.Equal, target.GetAttributeValue<EntityReference>(ProdottoDigitale.ProdottoDigitaleId).Id);
            query.NoLock = true;
            query.TopCount = 1;
            var result = service.RetrieveMultiple(query);
            if (result.Entities.Count == 0)
            {
                return new List<Entity>();
            }
            return result.Entities.ToList();
        }

        public void UpdateKeyProdottoDigitale(IOrganizationService service, Guid keyProdottoDigitale, int typePiattaforma)
        {
            Entity entityUpdatePD = new Entity(ProdottoDigitale.LogicalName);
            entityUpdatePD.Id = keyProdottoDigitale;
            entityUpdatePD[ProdottoDigitale.TypePiattaforma] = new OptionSetValue(typePiattaforma);
            service.Update(entityUpdatePD);
        }
    }
}
 