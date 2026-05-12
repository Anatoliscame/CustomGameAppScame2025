using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Plugin.acn_GameApp.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.acn_GameApp.Core
{
    public class AcquistoHelper
    {
        public AcquistoHelper() { }

        public EntityCollection GetAcquisto(IOrganizationService service, Entity targetNew)
        {
            QueryExpression acquistiQ = new QueryExpression("acn_acquisto");
            acquistiQ.ColumnSet = new ColumnSet(true);
            acquistiQ.Criteria.AddCondition("acn_acquistoid", ConditionOperator.NotEqual, targetNew.Id);
            return service.RetrieveMultiple(acquistiQ);
        }

        public List<Entity> GetAcquistoTargetAndInAttesa(IOrganizationService service, Entity entity)
        {
            QueryExpression query = new QueryExpression("acn_acquisto")
            {
                ColumnSet = new ColumnSet(true),
                Criteria = new FilterExpression()
            };
            query.Criteria.AddCondition("acn_kestatusacquisto", ConditionOperator.Equal, 746200001); // In Attesa
            query.Criteria.AddCondition("acn_acquistoid", ConditionOperator.NotEqual, entity.Id);
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
            QueryExpression query = new QueryExpression("acn_acquisto")
            {
                ColumnSet = new ColumnSet(true),
                Criteria = new FilterExpression()
            };
            query.Criteria.AddCondition("acn_kestatusacquisto", ConditionOperator.Equal, 746200001); // In Attesa
            query.Criteria.AddCondition("acn_account", ConditionOperator.Equal, accountId);
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
