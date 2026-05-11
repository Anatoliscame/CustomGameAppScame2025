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
    public class OrderAcquistoHelper
    {
        public OrderAcquistoHelper() { }

        public List<Entity> GetOrderAcquisto(IOrganizationService service, Entity targetUpdateAcquisto)
        {
            QueryExpression query = new QueryExpression(OrderAcquisto.LogicalName)
            {
                ColumnSet = new ColumnSet(OrderAcquisto.OrdineAcquistoId, OrderAcquisto.KeyProdottoDigitale, OrderAcquisto.AcquistoId, OrderAcquisto.ProdottoDigitaleId),
                Criteria = new FilterExpression()
            };
            query.Criteria.AddCondition(OrderAcquisto.AcquistoId, ConditionOperator.Equal, targetUpdateAcquisto.Id);
            query.NoLock = true;
            //query.TopCount = 1;

            // Collega l'entità KeyGame alla query (join)
            LinkEntity keyProductLink = new LinkEntity
            {
                LinkFromEntityName = OrderAcquisto.LogicalName,
                LinkFromAttributeName = OrderAcquisto.KeyProdottoDigitale,
                LinkToEntityName = KeyProdotto.LogicalName,
                LinkToAttributeName = KeyProdotto.KeyDigitale,
                JoinOperator = JoinOperator.Inner,
                Columns = new ColumnSet(KeyProdotto.KeyProdottoId, KeyProdotto.StatusPresentKey),
                EntityAlias = "OrderAcquistoKeyProduct"
            };
            keyProductLink.LinkCriteria.AddCondition(KeyProdotto.StatusPresentKey, ConditionOperator.Equal, 126400004); //Temporaneamente

            query.LinkEntities.Add(keyProductLink);// KeyProduct

            var result = service.RetrieveMultiple(query);
            if (result.Entities.Count == 0)
            {
                return new List<Entity>();
            }
            return result.Entities.ToList();
        }

        public List<Entity> GetOrderAcquistoEspansion(IOrganizationService service, Guid target)
        {
            QueryExpression query = new QueryExpression(OrderAcquistoEspansione.LogicalName)
            {
                ColumnSet = new ColumnSet(false),
                Criteria = new FilterExpression()
            };
            query.Criteria.AddCondition(OrderAcquistoEspansione.OrdineAcquisto, ConditionOperator.Equal, target);
            query.NoLock = true;
            var result = service.RetrieveMultiple(query);
            if (result.Entities.Count == 0)
            {
                return new List<Entity>();
            }
            return result.Entities.ToList();
        }
    }
}
