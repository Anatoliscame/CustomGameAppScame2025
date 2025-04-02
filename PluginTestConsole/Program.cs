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

            OnCreateOnUpdateCheckExistOrderAcquistoWrapper wrapper = new OnCreateOnUpdateCheckExistOrderAcquistoWrapper();

            string guid = "c3869d87-140f-f011-9989-000d3abdaa8d";

            wrapper.Execute(service, guid);

        }
    }
}
