using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Plugin.sc_ProductDetails.Entities;
using Plugin.sc_ProductDetails.Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.sc_ProductDetails.BusinessLogicPlugins
{
    public class OnCreateUpdateOrderAcquistoCheckExistKeyProductDetailsLogic
    {
        public void ExecuteLogic(IOrganizationService service, Entity entity, ITracingService tracingService)
        {
            tracingService?.Trace("Start Plugin OnCreateUpdateOrderAcquistoCheckExistKeyProductDetails");

            tracingService?.Trace("OrderAcquisto (message Create, Update and Delete) is successfully.");

            ExecuteOrderAcquistoCreate(service, entity, tracingService);

            tracingService?.Trace("End Plugin OnCreateUpdateOrderAcquistoCheckExistKeyProductDetails");
        }

        public void ExecuteOrderAcquistoCreate(IOrganizationService service, Entity target, ITracingService trace)
        {
            AcquistoHelper _acquistoHelper = new AcquistoHelper();
            KeyProdottoHelper _keyProductHelper = new KeyProdottoHelper();
            ProdottoDigitaleHelper _prodottoDigitaleHelper = new ProdottoDigitaleHelper();
            Entity entityUpdate = new Entity(OrderAcquisto.LogicalName);
            entityUpdate.Id = target.Id;
            Guid acquistoIdRetrive = Guid.Empty;
            Guid pdIdRetrive = Guid.Empty;
             
            if (!target.TryGetAttributeValue(OrderAcquisto.ProdottoDigitaleId, out EntityReference prodottodigitaleTo))
            {
                trace?.Trace($"ProdottoDigitaleTO is null: {prodottodigitaleTo}");
                return;
            }
            //  get ProdottoDigitaleTO
            Entity getPDToTo = service.Retrieve(prodottodigitaleTo.LogicalName, prodottodigitaleTo.Id, new ColumnSet(true));

            int? typePiattaforma = ((OptionSetValue)getPDToTo.Attributes[ProdottoDigitale.TypePiattaforma]).Value;
            // Key Game di PD Based
            List<Entity> keyProductArray = _keyProductHelper.ExistKeyProduct(service, prodottodigitaleTo, 126400000, typePiattaforma); // Disponibile;
            if (keyProductArray.Count == 0)
            {
                trace?.Trace($"Le chiavi non sono disponibile: {keyProductArray.Count}");
                return;
            }

            if (!target.TryGetAttributeValue(OrderAcquisto.AcquistoId, out EntityReference acquistoTo))
            {
                trace?.Trace($"acquistoTo is null: {acquistoTo}");

                Guid accountId = getPDToTo.GetAttributeValue<EntityReference>(ProdottoDigitale.AccountCliente)?.Id ?? Guid.Empty;
                pdIdRetrive = getPDToTo.GetAttributeValue<Guid>(ProdottoDigitale.ProdottoDigitaleId);
                List<Entity> acquistiInattesa = _acquistoHelper.GetAcquistoInAttesa(service, accountId);
                if (acquistiInattesa.Count > 0)
                {
                    Guid acquistoId = acquistiInattesa[0].GetAttributeValue<Guid>(Acquisto.AcquistoId);
                    acquistoIdRetrive = acquistoId;
                }
                else
                {
                    int quantitaAcquisto = _acquistoHelper.GetAcquisto(service, target).Entities.Count + 1;

                    Guid acquistoId = _acquistoHelper.CreateAcquisto(service, quantitaAcquisto, accountId, 126400001, GeneraCodiceAcquisto());
                    if (acquistoId == Guid.Empty){throw new InvalidPluginExecutionException($"Errore durante la creazione dell'Acquisto.");}
                    acquistoIdRetrive = acquistoId;

                    trace?.Trace($"Nuovo Acquisto creato: {acquistoTo}");
                }

                entityUpdate[OrderAcquisto.AcquistoId] = new EntityReference(Acquisto.LogicalName, acquistoIdRetrive);

            }
            trace?.Trace($"AssignTo {acquistoTo}");

            _keyProductHelper.UpdateKeyProduct(service, keyProductArray[0].Id, 126400004);// Temporaneamente 

            entityUpdate[OrderAcquisto.KeyProdottoDigitale] = keyProductArray[0].GetAttributeValue<string>(KeyProdotto.KeyDigitale);// Padre key

            service.Update(entityUpdate);
        }

        private string GeneraCodiceAcquisto(int lunghezza = 6)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return "ACQ-" + new string(Enumerable.Repeat(chars, 6)
                                              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
} 
