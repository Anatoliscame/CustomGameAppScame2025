using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
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

        public List<Entity> ExistKeyGame(IOrganizationService service, Entity targetNew, EntityReference videogameTo)
        {

            QueryExpression query = new QueryExpression("acn_keygame")
            {
                ColumnSet = new ColumnSet("acn_keygameid"),
                Criteria = new FilterExpression()
            };
            query.Criteria.AddCondition("acn_videogame", ConditionOperator.Equal, videogameTo.Id);
            query.NoLock = true;
            query.TopCount = 1;

            var result = service.RetrieveMultiple(query);
            if (result.Entities.Count == 0)
            {
                return new List<Entity>();
            }
            return result.Entities.ToList();
        }
    }
}
