using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using PluginTestConsole.Wrapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var service = CrmRepository("DEV");

            OnCreateOrOnUpdateCheckExistKeyGameWrapper wrapper = new OnCreateOrOnUpdateCheckExistKeyGameWrapper();

            string guid = "8ff0fae2-fcf4-ef11-be1f-6045bd95a7f9";

            wrapper.Execute(service, guid);

        }
    }
}
