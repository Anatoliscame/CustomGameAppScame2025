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
            if (prodottiDigitale.Count == 0) { throw new InvalidPluginExecutionException("Non esiste un prodotto digitale attivo."); }
            Guid idProdDigital = prodottiDigitale[0].GetAttributeValue<Guid>(ProdottoDigitale.ProdottoDigitaleId);
            string nameTo = GetNameBeforeDash(prodottiDigitale[0].GetAttributeValue<string>(ProdottoDigitale.Name));

            int? typeproductdetail = postImage.GetAttributeValue<OptionSetValue>(ProductDetails.TypeProductDetail)?.Value;
            //VideoGame
            if (typeproductdetail == 126400000)
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
    }
}
