using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace CDKeyScameMVCApp.Models
{
	public class OrderAcquistoEspansioneViewModel
	{
        public Guid OrderAcquistoEId { get; set; }
        [Display(Name = "Nome")]
        public string Name { get; set; }
        [Display(Name = "Key code")]
        public string KeyGameCode { get; set; }
        public string NameContentVideoGame { get; set; }
        public OrdineAcquistoViewModel OrdineAcquisto { get; set; }

    }
}