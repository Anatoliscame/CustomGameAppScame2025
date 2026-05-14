using Microsoft.Xrm.Sdk;
using Plugin.sc_DigitalProduct.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.sc_DigitalProduct.Helper
{
    public class ProductDetailsHelper
    {
        public ProductDetailsHelper() { }


        public void UpdateNameProductDetails(IOrganizationService service, Entity postImage, string nameTo)
        {
            Entity updateTarget = new Entity(ProductDetails.LogicalName)
            {
                Id = postImage.Id
            };
            updateTarget[ProductDetails.Name] = $"{nameTo}";
            service.Update(updateTarget);
        }
    }
}
