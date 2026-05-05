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
    public class AcquistoHelper
    {
        public AcquistoHelper() { }

        public EntityCollection GetAcquisto(IOrganizationService service, Entity targetNew)
        {
            QueryExpression acquistiQ = new QueryExpression(Acquisto.LogicalName);
            acquistiQ.ColumnSet = new ColumnSet(true);
            acquistiQ.Criteria.AddCondition(Acquisto.AcquistoId, ConditionOperator.NotEqual, targetNew.Id);
            return service.RetrieveMultiple(acquistiQ);
        }

        public List<Entity> GetAcquistoTargetAndInAttesa(IOrganizationService service, Entity entity)
        {
            QueryExpression query = new QueryExpression(Acquisto.LogicalName)
            {
                ColumnSet = new ColumnSet(true),
                Criteria = new FilterExpression()
            };
            query.Criteria.AddCondition(Acquisto.KestatusAcquisto, ConditionOperator.Equal, 126400001); // In Attesa
            query.Criteria.AddCondition(Acquisto.AcquistoId, ConditionOperator.NotEqual, entity.Id);
            query.NoLock = true;
            var result = service.RetrieveMultiple(query);
            if (result.Entities.Count == 0)
            {
                return new List<Entity>();
            }
            return result.Entities.ToList();
        }

        public List<Entity> GetAcquistoInAttesa(IOrganizationService service, Guid accountId)
        {
            QueryExpression query = new QueryExpression(Acquisto.LogicalName)
            {
                ColumnSet = new ColumnSet(true),
                Criteria = new FilterExpression()
            };
            query.Criteria.AddCondition(Acquisto.KestatusAcquisto, ConditionOperator.Equal, 126400001); // In Attesa
            query.Criteria.AddCondition(Acquisto.AccountId, ConditionOperator.Equal, accountId);
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
