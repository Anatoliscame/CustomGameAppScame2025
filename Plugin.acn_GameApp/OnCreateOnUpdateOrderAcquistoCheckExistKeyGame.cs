using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Plugin.acn_GameApp.Core;
using Plugin.acn_GameApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.acn_GameApp
{
    public class OnCreateOnUpdateOrderAcquistoCheckExistKeyGame : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            var service = factory.CreateOrganizationService(context.UserId);
            var trace = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            try
            {
                trace.Trace("Start Plugin OnCreateOnUpdateOrderAcquistoCheckExistKeyGame");
                context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
                if (trace == null)
                    throw new InvalidPluginExecutionException("Failed to retrieve the tracing service.");

                var target = new Entity();

                if (context.MessageName.ToLower() == "create")
                {
                    target = (Entity)context.InputParameters["Target"];
                }
                /*if (context.MessageName.ToLower() == "update")
                {
                    target = context.PostEntityImages.Values?.FirstOrDefault();
                }*/
                ExecuteAcquisto(service, target, trace);

                trace.Trace("End Plugin OnCreateOnUpdateOrderAcquistoCheckExistKeyGame");
            }
            catch (Exception ex)
            {
                trace.Trace($"Error in Plugin OnCreateOnUpdateOrderAcquistoCheckExistKeyGame {ex.Message}. \n\r {ex.StackTrace}");
                throw new InvalidPluginExecutionException(ex.Message);
            }
        }
        public void ExecuteAcquisto(IOrganizationService service, Entity target, ITracingService trace)
        {
            AcquistoHelper _acquistoHelper = new AcquistoHelper();
            KeyGameHelper _keyGameHelper = new KeyGameHelper();
            Entity entityUpdate = new Entity("acn_ordineacquisto");
            entityUpdate.Id = target.Id;
            Guid acquistoIdRetrive = Guid.Empty;
            if (!target.TryGetAttributeValue("acn_acquistoid", out EntityReference acquistoTo) || acquistoTo == null)
            {
                trace.Trace($"acquistoTo is null: {acquistoTo}");

                List<Entity> acquistiInattesa = _acquistoHelper.GetAcquistoInAttesa(service);
                if (acquistiInattesa.Count > 0)
                {
                    Guid acquistoId = acquistiInattesa[0].GetAttributeValue<Guid>("acn_acquistoid");
                    acquistoIdRetrive = acquistoId;
                    entityUpdate["acn_acquistoid"] = new EntityReference("acn_acquisto", acquistoId);
                }
                else
                {
                    int quantitaAcquisto = _acquistoHelper.GetAcquisto(service, target).Entities.Count + 1;

                    Entity nuovoAcquisto = new Entity(Acquisto.LogicalName);
                    nuovoAcquisto["acn_name"] = "acquisto" + quantitaAcquisto.ToString() + DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                    //nuovoAcquisto["acn_account"] = target.GetAttributeValue<EntityReference>("acn_accountid"); // Associa l'account
                    nuovoAcquisto["statuscode"] = new OptionSetValue(746200002); // Stato "In Attesa" (Assumendo che il valore sia 100000000)
                    nuovoAcquisto["acn_code"] = GeneraCodiceAcquisto();
                    Guid acquistoId = service.Create(nuovoAcquisto);
                    acquistoIdRetrive = acquistoId;
                    trace.Trace($"Nuovo Acquisto creato: {acquistoId}");
                }

                entityUpdate["acn_acquistoid"] = new EntityReference(Acquisto.LogicalName, acquistoIdRetrive);
            }
            trace.Trace($"AssignTo {acquistoTo}");

            if (!target.TryGetAttributeValue("acn_videogameid", out EntityReference videogameTo) || videogameTo == null)
            {
                trace.Trace($"videogameToTo is null: {videogameTo}");
                throw new Exception("videogameToTo is not valued");
            }


            List<Entity> keyGameArray = _keyGameHelper.ExistKeyGame(service, videogameTo);

           if (keyGameArray.Count <= 0) { throw new Exception("keyGameArray: Chiavi disponibili con un video gioco non ci sono"); }


            /*Entity eAcquistoTo = service.Retrieve(Acquisto.LogicalName, acquistoIdRetrive.Id, new ColumnSet(new string[] { Acquisto.StatusReason }));
            if (eAcquistoTo.Contains(Acquisto.StatusReason) && eAcquistoTo[Acquisto.StatusReason] is OptionSetValue statusReasonValue)
            {
                if (statusReasonValue.Value != 746200001)// Effetuato
                {
                    _keyGameHelper.UpdateKeyGame(service, keyGameArray, 746200002);// Temporaneamente 

                    //_keyGameHelper.UpdateKeyGame(service, keyGameArray, 746200003);// Inattesa
                }
            }*/
            _keyGameHelper.UpdateKeyGame(service, keyGameArray, 746200002);// Temporaneamente 

            entityUpdate["acn_keygamecode"] = keyGameArray[0].GetAttributeValue<string>("acn_keygame");
            service.Update(entityUpdate);
            trace.Trace($"OrderAcquisto has been updated");

        }

        private string GeneraCodiceAcquisto(int lunghezza = 6)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return  "ACQ-" + new string(Enumerable.Repeat(chars, 6)
                                              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
