using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.sc_ProductDetails.CorePlugins
{
    public static class Utilities
    {
        public static Entity MergeEntities(Entity preImage, Entity target)
        {
            foreach (var attr in preImage.Attributes)
            {
                if (!target.Contains(attr.Key))
                {
                    target[attr.Key] = attr.Value;
                }
            }
            return target;
        }

        public static string GeneraCodice(int lunghezza = 6)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return "SCAME_CODICE-" + new string(Enumerable.Repeat(chars, 6)
                                              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static List<string> GetPrivateConfigurationValueSplit(string priveConfigTo)
        {
            List<string> priveConfigTo_Array = priveConfigTo.Split(',')
               .Select(u => u.Trim())//per rimuovere gli spazi vuoti sia all'inizio e alla fine
               .Select(u => u.Trim('\'', '"'))
               .Where(u => !string.IsNullOrWhiteSpace(u)) // elimina eventuali elementi vuoti
               .ToList();
            return priveConfigTo_Array;
        }

        public static string GetNameCodiceForProdottoDigitalePrivateConfig(IOrganizationService service, string key)
        {
            QueryExpression queryMessage = new QueryExpression();
            queryMessage.EntityName = "sc_privateconfiguration";
            queryMessage.ColumnSet.AddColumns("sc_description", "sc_value");
            queryMessage.Criteria.AddCondition("sc_key", ConditionOperator.Equal, key);

            var resultQuery = service.RetrieveMultiple(queryMessage);

            if (resultQuery.Entities.Count == 0)
            {
                return string.Empty;
            }
            return resultQuery.Entities[0].GetAttributeValue<string>("sc_value");
        }
    }
}
