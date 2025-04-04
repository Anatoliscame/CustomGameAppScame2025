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

        public List<Entity> ExistKeyGame(IOrganizationService service, EntityReference videogameTo)
        {

            QueryExpression query = new QueryExpression("acn_keygame")
            {
                ColumnSet = new ColumnSet("acn_keygame"),
                Criteria = new FilterExpression()
            };
            query.Criteria.AddCondition("acn_videogame", ConditionOperator.Equal, videogameTo.Id);
            query.Criteria.AddCondition("acn_statuspresentkeygame", ConditionOperator.Equal, 746200000); // Disponibile
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

        public void UpdateKeyGame(IOrganizationService service, List<Entity> keyGameArray, int StatusKeyGame)
        {
            Entity keyGame = new Entity("acn_keygame");
            keyGame.Id = keyGameArray[0].Id;
            keyGame["acn_statuspresentkeygame"] = new OptionSetValue(StatusKeyGame);
            service.Update(keyGame);
        }
    }
}
