using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Plugin.sc_ProductDetails.CorePlugins;
using Plugin.sc_ProductDetails.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.sc_ProductDetails.Helper
{
    public class ProdottoDigitaleHelper
    {
        public ProdottoDigitaleHelper() { }

        public List<Entity> GeNamesProdottiDigitaleActived(IOrganizationService service, string nameTo, int? typePD)
        {

            QueryExpression query = new QueryExpression(ProdottoDigitale.LogicalName)
            {
                ColumnSet = new ColumnSet(true),
                Criteria = new FilterExpression()
            };
            query.Criteria.AddCondition(ProdottoDigitale.Name, ConditionOperator.Equal, nameTo);
            query.Criteria.AddCondition(ProdottoDigitale.TypeProdottoDigitale, ConditionOperator.Equal, typePD);
            query.Criteria.AddCondition(ProdottoDigitale.StateCode, ConditionOperator.Equal, 0);//Active
            query.NoLock = true;
            query.TopCount = 1;
            var result = service.RetrieveMultiple(query);
            if (result.Entities.Count == 0)
            { 
                return new List<Entity>();
            }
            return result.Entities.ToList();
        }

        public List<Entity> GeProdottiDigitaleActived(IOrganizationService service, Entity target)
        {

            QueryExpression query = new QueryExpression(ProdottoDigitale.LogicalName)
            {
                ColumnSet = new ColumnSet(true),
                Criteria = new FilterExpression()
            };
            query.Criteria.AddCondition(ProdottoDigitale.StateCode, ConditionOperator.Equal, 0);//Active
            query.Criteria.AddCondition(ProdottoDigitale.ProductDetails, ConditionOperator.Equal, target.Id);
            query.NoLock = true;
            query.TopCount = 1;
            var result = service.RetrieveMultiple(query);
            if (result.Entities.Count == 0)
            {
                return new List<Entity>();
            }
            return result.Entities.ToList();
        }

        public void UpdateKeyProdottoDigitale(IOrganizationService service, Guid keyProdottoDigitale, int typePiattaforma)
        {
            Entity entityUpdatePD = new Entity(ProdottoDigitale.LogicalName);
            entityUpdatePD.Id = keyProdottoDigitale;
            entityUpdatePD[ProdottoDigitale.TypePiattaforma] = new OptionSetValue(typePiattaforma);
            service.Update(entityUpdatePD);
        }

        public void UpdateNameCodiceProdottoDigitale(IOrganizationService service, Guid idProdDigital, string nameTo, int? value)
        {
            Entity updateProdottoDigitale = new Entity(ProdottoDigitale.LogicalName)
            {
                Id = idProdDigital
            };

            updateProdottoDigitale.Attributes[ProdottoDigitale.Name] = $"{nameTo}";
            updateProdottoDigitale.Attributes[ProdottoDigitale.Codice] = $"{Utilities.GeneraCodice()}" + " - " + $"{GetNameVerifyTypeExpansion(value)}";

            service.Update(updateProdottoDigitale);
        }
        public string GetNameVerifyTypeExpansion(int? value) 
        {
            string name = string.Empty;

            if (value == 126400000) // Base Game
            {
                name = "Base Game";
            }
            if (value == 126400001) // DLC
            {
                name = "DLC";
            }
            if (value == 126400002) // Remastered
            {
                name = "Remastered";
            }
            if (value == 126400003) // Espansione
            {
                name = "Espansione";
            }
            return name;
        }
    }
}
 