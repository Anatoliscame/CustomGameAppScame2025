using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CDKeyScameMVCApp.Models
{
	public class AccountViewModel
	{
        public Guid AccountId { get; set; }
        [Display(Name = "Nome acquisto")]
        public string Name { get; set; }
        public ICollection<AcquistoViewModel> AcquistoS { get; set; } = new List<AcquistoViewModel>();
        public ICollection<VideoGameViewModel> VideoGameS { get; set; } = new List<VideoGameViewModel>();
        public ICollection<OrdineAcquistoViewModel> OrdineAcquistoS { get; set; } = new List<OrdineAcquistoViewModel>();
        
    }
}