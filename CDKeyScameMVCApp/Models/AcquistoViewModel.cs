using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace CDKeyScameMVCApp.Models
{
	public class AcquistoViewModel
	{
        public Guid AcquistoId { get; set; }
        public string Name { get; set; }
        //[Required]
        //[StringLength(maximumLength: 25, ErrorMessage = "Titolo e' troppo lungo")]
        public AccountViewModel Account { get; set; }
        public string AccountName { get; set; }
        public string Code { get; set; }
        [DataType(DataType.Date)]
        public DateTime DataAcquisto { get; set; }
        public int? Fattura { get; set; }// Integer in CRM
        public Decimal IVA { get; set; }// Decimal in CRM
        public Decimal Totale { get; set; }// Decimal in CRM

        public enum KeStatusAcquisto
        {
            Effetuato = 746200000,
            In_attesa = 746200001,
            Annulato = 746200002
        }
        //(int)AcquistoViewModel.KeStatusAcquisto.Effetuato)

    }
}