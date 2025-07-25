using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.acn_GameApp.Entities;

namespace Plugin.acn_GameApp.Core
{
    public class VideoGameHelper
    {
        public VideoGameHelper(){}

        public List<Entity> GeVideoGames(IOrganizationService service, Entity target)
        {

            QueryExpression query = new QueryExpression(VideoGame.LogicalName)
            {
                ColumnSet = new ColumnSet(true),
                Criteria = new FilterExpression()
            };
            query.Criteria.AddCondition(VideoGame.VideogameId, ConditionOperator.Equal, target.GetAttributeValue<EntityReference>("acn_videogameid").Id);
            query.NoLock = true;
            query.TopCount = 1;
            var result = service.RetrieveMultiple(query);
            if (result.Entities.Count == 0)
            {
                return new List<Entity>();
            }
            return result.Entities.ToList();
        }

        public List<Entity> GeVideoGameWithEspansion(IOrganizationService service, Guid videogameID)
        {

            QueryExpression query = new QueryExpression(VideoGame.LogicalName)
            {
                ColumnSet = new ColumnSet(true),
                Criteria = new FilterExpression()
            };
            query.Criteria.AddCondition(VideoGame.ParentVideogameId, ConditionOperator.Equal, videogameID);
            query.Criteria.AddCondition(VideoGame.TipoVideogioco, ConditionOperator.Equal, 746200001);
            //query.Criteria.AddCondition("acn_typepiattaforma", ConditionOperator.Equal, typePiattaforma);
            //query.Criteria.AddCondition("acn_videogameid", ConditionOperator.NotEqual, videogameID.Id);
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
            Entity entityUpdateVG = new Entity(VideoGame.LogicalName);
            entityUpdateVG.Id = keyVideoGame;
            entityUpdateVG[VideoGame.TypePiattaforma] = new OptionSetValue(typePiattaforma);
            service.Update(entityUpdateVG); 
        }
    }
}
