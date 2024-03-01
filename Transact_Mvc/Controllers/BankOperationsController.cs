using Microsoft.AspNetCore.Mvc;
using Transact_Mvc.Models;
using Transact_Mvc.Database;
using Transact_Mvc.Views.BankOperations;


namespace Transact_Mvc.Controllers
{
    public class BankOperationsController : Controller
    {
        private readonly TransactContext _context;

        public BankOperationsController(TransactContext context)
        {
            _context = context;
        }
        public IActionResult DepositSuccess()
        {
            var viewModel = new DepositSuccessViewModel
            {
                SuccessMessage = "Deposit Successful!"
            };

            return View("DepositSuccess", viewModel);
        }
        public IActionResult DepositFail()
        {
            var viewModel = new DepositFail
            {
                DepositFailMessage = "Deposit Failed: You do not have enough money to deposit!"
            };

            return View("DepositFail", viewModel);
        }
        public IActionResult WithdrawFail()
        {
            var viewModel = new WithdrawFail
            {
                WithdrawFailMessage = "Withdraw Failed: amount is more than your existing deposit!"
            };

            return View("WithdrawFail", viewModel);
        }
        public IActionResult WithdrawSuccess()
        {
            var viewModel = new WithdrawSuccessViewModel
            {
                SuccessMessage = "Withdraw Successful!"
            };

            return View("WithdrawSuccess", viewModel);
        }
        public IActionResult DepositFailZero()
        {
            var viewModel = new DepositFailZeroModel
            {
                FailZeroMessage = "Amount should be greater than zero or any negative number!"
            };

            return View("DepositFailZero", viewModel);
        }

        public IActionResult WithdrawFailZero()
        {
            var viewModel = new WithdrawFailZeroModel
            {
                FailZeroMessage = "Amount should be greater than zero or any negative number!"
            };

            return View("WithdrawFailZero", viewModel);
        }
        [HttpGet]
        public IActionResult Deposit()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Deposit(double amount, string userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id.ToString() == userId);
            if (user == null)
            {
                return NotFound($"User with ID '{userId}' not found.");
            }
            if (amount <= 0)
            {
                ModelState.AddModelError("amount", "Amount should be greater than zero or any negative number!");
                return RedirectToAction("DepositFailZero");
            }
            if (amount > user.Balance)
            {
                ModelState.AddModelError("amount", "Amount is greater than your balance!");
                return RedirectToAction("DepositFail");
            }
            try
            {
                var bank = _context.Bank.FirstOrDefault(b => b.UserName == user.UserName);
                if (bank != null)
                {
                    user.Balance -= amount;
                    bank.Balance += amount;
                    _context.Update(user);
                    _context.Update(bank);
                }
                else
                {
                    user.Balance -= amount;
                    var newStatement = new Bank()
                    {
                        Balance = amount,
                        UserName = user.UserName
                    };

                    _context.Add(newStatement);
                }
                _context.SaveChanges();

                var viewModel = new DepositSuccessViewModel
                {
                    SuccessMessage = "Deposit Successful!"
                };
                return View("DepositSuccess", viewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while processing your deposit.");
                return View();
            }
        }
        [HttpGet]
        public IActionResult Withdraw()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Withdraw(double amount, string userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id.ToString() == userId);
            if (user == null)
            {
                return NotFound($"User with ID '{userId}' not found.");
            }
            if (amount <= 0)
            {
                ModelState.AddModelError("amount", "Amount should be greater than zero or any negative number!");
                return RedirectToAction("WithdrawFailZero");
            }
            var bank = _context.Bank.FirstOrDefault(b => b.UserName == user.UserName);
            if (bank.Balance < amount)
            {
                ModelState.AddModelError("amount", "Withdraw Failed: amount is more than your existing deposit!");
                return RedirectToAction("WithdrawFail");
            }
            if (bank.Balance >= amount)
            {
                bank.Balance -= amount;
                user.Balance += amount;
                _context.Update(bank);
                _context.Update(user);
                _context.SaveChanges();
            }
            var viewModel = new WithdrawSuccessViewModel
            {
                SuccessMessage = "Withdraw Successful!"
            };
            return View("WithdrawSuccess", viewModel);
        }
    }
}