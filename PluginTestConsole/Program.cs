using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using PluginTestConsole.Wrapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Description;

namespace PluginTestConsole
{
    class Program
    {
        public static IOrganizationService CrmRepository(string connectionStringName)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("CRM ConnectionString empty or null!");

            // Connect to the CRM web service using a connection string.
            ServiceClient conn = new ServiceClient(connectionString);

            return conn;
        }


        static void Main(string[] args)
        {
            // var service = CrmRepository("CRM_CREDENTIALS");

            var connectionString = System.Configuration.ConfigurationManager.AppSettings["CRM_CustomeAppScameCDKeysVersion2"];
            CrmServiceClient crmServiceClient = new CrmServiceClient(connectionString);
             // string connectionString = ConfigurationManager.ConnectionStrings["CRM_CREDENTIALS"].ConnectionString;

             //CrmServiceClient crmServiceClient = new CrmServiceClient(connectionString);

            if (!crmServiceClient.IsReady)
            {
                throw new Exception("cannot instantiate service");
            }


            OnCreateOnUpdateCheckExistOrderAcquistoWrapper wrapper = new OnCreateOnUpdateCheckExistOrderAcquistoWrapper();

            string guid = "45d11387-e916-f011-998a-000d3abdaa8d";
            
            wrapper.Execute(crmServiceClient, guid);

        }
    }
}
