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

        public List<Entity> GeVideoGameWithEspansion(IOrganizationService service, Entity target, int statuscode)
        {

            QueryExpression query = new QueryExpression("acn_videogame")
            {
                ColumnSet = new ColumnSet(true),
                Criteria = new FilterExpression()
            };
            query.Criteria.AddCondition("acn_parentvideogameid", ConditionOperator.Equal, target.GetAttributeValue<EntityReference>("acn_videogameid").Id);
            query.Criteria.AddCondition("acn_videogameid", ConditionOperator.NotEqual);
            query.Criteria.AddCondition("statuscode", ConditionOperator.Equal, statuscode);
            query.NoLock = true;
            var result = service.RetrieveMultiple(query);
            if (result.Entities.Count == 0)
            {
                return new List<Entity>();
            }
            return result.Entities.ToList();
        }

        public void UpdateVideoGame(IOrganizationService service, Guid keyVideoGame, int typePiattaforma)
        {
            Entity entityUpdateVG = new Entity("acn_videogame");
            entityUpdateVG.Id = keyVideoGame;
            entityUpdateVG["acn_typepiattaforma"] = new OptionSetValue(typePiattaforma);
            service.Update(entityUpdateVG); 
        }
    }
}
