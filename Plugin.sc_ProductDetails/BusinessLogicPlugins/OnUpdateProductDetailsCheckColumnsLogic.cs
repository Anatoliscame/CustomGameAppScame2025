using Microsoft.Xrm.Sdk;
using Plugin.sc_ProductDetails.CorePlugins;
using Plugin.sc_ProductDetails.Entities;
using Plugin.sc_ProductDetails.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Services;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.sc_ProductDetails.BusinessLogicPlugins
{
    public class OnUpdateProductDetailsCheckColumnsLogic
    {
        public void ExecuteLogic(IOrganizationService service, Entity postImage, ITracingService trace)
        {
            trace?.Trace("ProductDetails (message Create, Update and Delete) is successfully.");
            if (postImage != null)
            {
                ExecuteProductDetails(service, postImage, trace);
            }
            else
            {
                trace?.Trace("Product Details is null");
                return;
            }
        }

        public void ExecuteProductDetails(IOrganizationService service, Entity postImage, ITracingService trace)
        {
            ProdottoDigitaleHelper _prodottoDigitaleHelper = new ProdottoDigitaleHelper();
            ProductDetailsHelper _productDetailsHelper = new ProductDetailsHelper();

            List<Entity> prodottiDigitale = _prodottoDigitaleHelper.GeProdottiDigitaleActived(service, postImage);
            if (prodottiDigitale.Count == 0) 
            { 
                throw new InvalidPluginExecutionException("Non esiste un prodotto digitale attivo."); 
            }
            Guid idProdDigital = prodottiDigitale[0].GetAttributeValue<Guid>(ProdottoDigitale.ProdottoDigitaleId);
            
            string nameTo = GetNameBeforeDash(prodottiDigitale[0].GetAttributeValue<string>(ProdottoDigitale.Name));
            
            decimal percCommissione = postImage.GetAttributeValue<decimal>(ProductDetails.PercentualeCommissioneApp);
            if (percCommissione <= 0)
            {
                throw new InvalidPluginExecutionException("percCommissione non e' valorizzato, inserisci un valore.");
            }

            int? typeproductdetail = postImage.GetAttributeValue<OptionSetValue>(ProductDetails.TypeProductDetail)?.Value;
            if (typeproductdetail == 126400000) //VideoGame
            {
                trace?.Trace($"Valore di Tipo di Prodotto Digitale e' recuperato 'Video Game': {typeproductdetail.Value}");
   
                int? typeexpansion = postImage.Contains(ProductDetails.TypeExpansion)
                    ? postImage.GetAttributeValue<OptionSetValue>(ProductDetails.TypeExpansion)?.Value
                    : null;

                if (typeexpansion == null)
                {
                    trace?.Trace("sc_typeexpansion non valorizzato o non presente nella PostImage.");
                    throw new InvalidPluginExecutionException("Il campo Type Expansion è obbligatorio. Seleziona un valore prima di salvare.");
                }
                _productDetailsHelper.UpdateNameProductDetails(service, postImage, nameTo);

                _prodottoDigitaleHelper.UpdateNameCodiceProdottoDigitale(service, idProdDigital, nameTo, typeexpansion, typeproductdetail);
            }
            if (typeproductdetail == 126400001) //Licenza Software
            {
                trace?.Trace($"Valore di Tipo di Prodotto Digitale e' recuperato 'Licenza Software': {typeproductdetail.Value}");

                int? tipoLicenza = postImage.Contains(ProductDetails.TipoLicenza)
                    ? postImage.GetAttributeValue<OptionSetValue>(ProductDetails.TipoLicenza)?.Value
                    : null;

                if (tipoLicenza == null)
                {
                    trace?.Trace("tipoLicenza non valorizzato o non presente nella PostImage.");
                    throw new InvalidPluginExecutionException("Il campo Tipo di Licenza Software è obbligatorio. Seleziona un valore prima di salvare.");
                }
                _productDetailsHelper.UpdateNameProductDetails(service, postImage, nameTo);

                _prodottoDigitaleHelper.UpdateNameCodiceProdottoDigitale(service, idProdDigital, nameTo, tipoLicenza, typeproductdetail);
            }

            var countryLookup = postImage.GetAttributeValue<EntityReference>(ProductDetails.Country);
            if (countryLookup == null)
            {
                trace?.Trace("Country non valorizzato.");
                return;
                //throw new InvalidPluginExecutionException("Country non valorizzato.");
            }

            //-----------------TEMPORANEO, DA BLOCCARE COUNTRY NON CORRISPONDENTE SU PRIVE CONFIG---------------------//

            VerifyCountryProductDetailsWitchPriveConfig(service, countryLookup.Id, trace);


            ///--------------------------------------------------------------------------///
        }

        private string GetNameBeforeDash(string nameTo)
        {
            string[] nameParts = nameTo.Split('-');

            if (nameParts.Length > 0)
            {
                nameTo = nameParts[0].Trim();
            }

            return nameTo;
        }


        public void VerifyCountryProductDetailsWitchPriveConfig(IOrganizationService service, Guid countrid, ITracingService trace) 
        {
            var countryConfig = Utilities.GetDeserializeCountryConfig(service, trace, "CountryConfig");
            if (countryConfig == null) { return; }

            bool isCountry = Utilities.CheckCountryPrivateConfig(service, countryConfig, countrid);
            if (!isCountry)
            {
                trace?.Trace($"Il paese del prodotto digitale non è tra quelli autorizzati per generare l’approvazione.");
                throw new InvalidPluginExecutionException("Il paese del prodotto digitale non è tra quelli autorizzati per generare l’approvazione.");
            }
        }
    }
} 
