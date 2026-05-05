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
    public class KeyProdottoHelper
    {
        public KeyProdottoHelper() { }

        public List<Entity> ExistKeyProduct(IOrganizationService service, EntityReference productdigitaleTo, int statusKeyProduct, int? typePiattaforma)
        {

            QueryExpression query = new QueryExpression(KeyProdotto.LogicalName)
            {
                ColumnSet = new ColumnSet(KeyProdotto.KeyDigitale),
                Criteria = new FilterExpression()
            };
            query.Criteria.AddCondition(KeyProdotto.ProdottoDigitaleId, ConditionOperator.Equal, productdigitaleTo.Id);
            query.Criteria.AddCondition(KeyProdotto.StatusPresentKey, ConditionOperator.Equal, statusKeyProduct);
            query.Criteria.AddCondition(KeyProdotto.TypePiattaforma, ConditionOperator.Equal, typePiattaforma.Value);
            query.NoLock = true;
            //query.TopCount = 1;
            // query.AddOrder("createdon", OrderType.Descending);
            var result = service.RetrieveMultiple(query);
            if (result.Entities.Count == 0)
            {
                return new List<Entity>();
            }
            return result.Entities.ToList();
        }

        public void UpdateKeyProduct(IOrganizationService service, Guid keyProductArray, int StatusKeyProduct)
        {
            Entity keyProduct = new Entity(KeyProdotto.LogicalName);
            keyProduct.Id = keyProductArray;
            keyProduct[KeyProdotto.StatusPresentKey] = new OptionSetValue(StatusKeyProduct);
            service.Update(keyProduct);
        }
    }
}
