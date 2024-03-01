using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Transact_Mvc.Models
{
    public class MoneyTransferModel
    {
        [Required(ErrorMessage = "Amount is required.")]
        public double Amount { get; set; }

        [Required(ErrorMessage = "Recipient username is required.")]
        public string RecipientUsername { get; set; }
    }
}

