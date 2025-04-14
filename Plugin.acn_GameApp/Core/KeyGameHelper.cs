using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.acn_GameApp.Core
{
    public class KeyGameHelper
    {
        public KeyGameHelper() {}

        public List<Entity> ExistKeyGame(IOrganizationService service, EntityReference videogameTo, int statusKeyGame)
        {

            QueryExpression query = new QueryExpression("acn_keygame")
            {
                ColumnSet = new ColumnSet("acn_keygame"),
                Criteria = new FilterExpression()
            };
            query.Criteria.AddCondition("acn_videogame", ConditionOperator.Equal, videogameTo.Id);
            query.Criteria.AddCondition("acn_statuspresentkeygame", ConditionOperator.Equal, statusKeyGame);
            query.NoLock = true;
            query.TopCount = 1;
            // query.AddOrder("createdon", OrderType.Descending);
            var result = service.RetrieveMultiple(query);
            if (result.Entities.Count == 0)
            {
                return new List<Entity>();
            }
            return result.Entities.ToList();
        }

        public void UpdateKeyGame(IOrganizationService service, Guid keyGameArray, int StatusKeyGame)
        {
            Entity keyGame = new Entity("acn_keygame");
            keyGame.Id = keyGameArray;
            keyGame["acn_statuspresentkeygame"] = new OptionSetValue(StatusKeyGame);
            service.Update(keyGame);
        }
    }
}
