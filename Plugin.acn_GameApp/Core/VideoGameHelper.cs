using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.acn_GameApp.Core
{
    public class VideoGameHelper
    {
        public VideoGameHelper(){}

        public List<Entity> GeVideoGames(IOrganizationService service, Entity target)
        {

            QueryExpression query = new QueryExpression("acn_videogame")
            {
                ColumnSet = new ColumnSet(true),
                Criteria = new FilterExpression()
            };
            query.Criteria.AddCondition("acn_videogameid", ConditionOperator.Equal, target.GetAttributeValue<EntityReference>("acn_videogameid").Id);
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
