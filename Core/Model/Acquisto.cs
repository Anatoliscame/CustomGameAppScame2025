using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class Acquisto
    {
        public Guid AcquistoId { get; set; }
        public string Name { get; set; }
        public Account Account { get; set; }
        public string AccountName { get; set; }
        public string Code { get; set; }
        public DateTime DataAcquisto { get; set; }
        public int? Fattura { get; set; }// Integer in CRM
        public Decimal IVA { get; set; }// Decimal in CRM
        public Decimal Totale { get; set; }// Decimal in CRM

        public ICollection<OrdineAcquisto> OrdineAcquistoS { get; set; } = new List<OrdineAcquisto>();

        public enum KeStatusAcquisto
        {
            Effetuato = 746200000,
            In_attesa = 746200001,
            Annulato = 746200002
        }
        //(int)AcquistoViewModel.KeStatusAcquisto.Effetuato)
    }
}
