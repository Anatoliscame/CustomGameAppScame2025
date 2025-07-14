using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.acn_GameApp.Core
{
    public class OrderAcquistoHelper
    {
        public OrderAcquistoHelper() { }

        public List<Entity> GetOrderAcquisto(IOrganizationService service, Entity targetUpdateAcquisto)
        {
            QueryExpression query = new QueryExpression("acn_ordineacquisto")
            {
                ColumnSet = new ColumnSet("acn_ordineacquistoid", "acn_keygamecode", "acn_acquistoid", "acn_videogameid"),
                Criteria = new FilterExpression()
            };
            query.Criteria.AddCondition("acn_acquistoid", ConditionOperator.Equal, targetUpdateAcquisto.Id);
            query.NoLock = true;
            //query.TopCount = 1;

            // Collega l'entità KeyGame alla query (join)
            LinkEntity keyGameeLink = new LinkEntity
            {
                LinkFromEntityName = "acn_ordineacquisto",
                LinkFromAttributeName = "acn_keygamecode",
                LinkToEntityName = "acn_keygame",
                LinkToAttributeName = "acn_keygame",
                JoinOperator = JoinOperator.Inner,
                Columns = new ColumnSet("acn_keygameid", "acn_statuspresentkeygame"),
                EntityAlias = "OrderAcquistoKeyGame"
            };
            keyGameeLink.LinkCriteria.AddCondition("acn_statuspresentkeygame", ConditionOperator.Equal, 746200002); //Temporaneamente


            query.LinkEntities.Add(keyGameeLink);// KeyGame

            var result = service.RetrieveMultiple(query);
            if (result.Entities.Count == 0)
            {
                return new List<Entity>();
            }
            return result.Entities.ToList();
        }
    }
}
