using Microsoft.AspNetCore.Mvc;
using Transact_Mvc.Database;
using Transact_Mvc.Models;

namespace Transact_Mvc.Controllers
{
    public class MoneyTransferController : Controller
    {
        private readonly TransactContext _context;
        public MoneyTransferController(TransactContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult MoneyTransfer()
        {
            return View();
        }
        [HttpPost]
        public IActionResult MoneyTransfer(MoneyTransferModel transferModel)
        {
            var user_userName = User.Identity.Name;
            var user = _context.Users.FirstOrDefault(u => u.UserName.ToUpper() == user_userName.ToUpper());
            var anotherUser = _context.Users.FirstOrDefault(u => u.UserName.ToUpper() == transferModel.RecipientUsername.ToUpper());
            if (user == null)
            {
                return Json(new { success = false, message = "Failed to transfer money!" });
            }
            if(transferModel.RecipientUsername == user_userName)
            {
                return Json(new { success = false, message = $"Enter another User's UserName!" });
            }
            if(anotherUser == null)
            {
                return Json(new { success = false, message = $"There is no any User by UserName: {transferModel.RecipientUsername}" });
            }
            if(transferModel.Amount <= 0)
            {
                return Json(new { success = false, message = $"Amount should be greater than 0 GEL!" });
            }
            if(user.Balance < transferModel.Amount)
            {
                return Json(new { success = false, message = $"You have not anough money to transfer to another User's account. Your balance is: {user.Balance}GEL and amount is: {transferModel.Amount}GEL." });
            }
            {
                user.Balance -= transferModel.Amount;
                anotherUser.Balance += transferModel.Amount;
                _context.SaveChanges();
                return Json(new { success = true, message = $"You have transfered {transferModel.Amount} GEL to {anotherUser}'s account!" });
            }
        }
    }
}
