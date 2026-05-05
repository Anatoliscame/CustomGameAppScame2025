using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.sc_ProductDetails.Entities
{
    public class ProdottoDigitale
    {
        public const string LogicalName = "sc_prodottodigitale";
        public const string ProdottoDigitaleId = "sc_prodottodigitaleid"; // Guid
        public const string Name = "sc_prodottodigitale";
        public const string ProductDetails = "sc_productdetails"; // Lookup
        public const string Codice = "sc_codiceprodotto";
        public const string AccountCliente = "sc_accountcliente";
        public const string StatoProdottoDigitale = "sc_statoprodottodigitale"; // Optionset
        public const string TypeProdottoDigitale = "sc_tipoprodottodigitale";// Optionset 
        public const string TypePiattaforma = "sc_piattaformaprodotttodigitale";// Optionset 
        public const string Key = "sc_key";// String 
        public const string PrezzVendita = "sc_prezzovendita";// Money 

        public const string RequisitinoteAttivazione = "sc_requisitinoteattivazione";//
        public const string StockDisponibile = "sc_stockdisponibile";// Money 
        public const string Descrizione = "sc_descrizione";// Memo 

        public const string ParentProdottoDigitaleId = "sc_parentprodottodigitale";// Non esiste
        
    }
}  
