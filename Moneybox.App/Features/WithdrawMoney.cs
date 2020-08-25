using Moneybox.App.DataAccess;
using Moneybox.App.Domain.Services;
using System;

namespace Moneybox.App.Features
{
    public class WithdrawMoney
    {
        private IAccountRepository accountRepository;
        private INotificationService notificationService;

        public WithdrawMoney(IAccountRepository accountRepository, INotificationService notificationService)
        {
            this.accountRepository = accountRepository;
            this.notificationService = notificationService;
        }

        public void Execute(Guid fromAccountId, decimal amount)
        {
            // TODO:

            var from = this.accountRepository.GetAccountById(fromAccountId);
            // Check the withdrawer has enough money to withdraw
            if (from.HasFunds(amount))
            {
                // Process transaction
                from.GiveMoney(amount);
                // Check if withdrawer is nearing zero balance
                if(from.IsNearZeroBalance()) this.notificationService.NotifyFundsLow(from.User.Email);
                this.accountRepository.Update(from);
            }
            else
            {
                throw new InvalidOperationException("Insufficient funds to make transfer");
            }
        }
    }
}
