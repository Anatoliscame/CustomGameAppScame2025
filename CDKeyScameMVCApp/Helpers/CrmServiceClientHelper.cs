using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace CDKeyScameMVCApp.Helpers
{
	public class CrmServiceClientHelper
	{
        private static CrmServiceClient _service;

        public static IOrganizationService GetService()
        {
            if (_service == null || !_service.IsReady)
            {
                var connStr = ConfigurationManager.ConnectionStrings["CRM_CustomeAppScameCDKeysVersion2"].ConnectionString;
                _service = new CrmServiceClient(connStr);
            }
            return _service;
        }
    }
}