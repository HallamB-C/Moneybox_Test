using Moneybox.App.DataAccess;
using Moneybox.App.Domain.Services;
using System;

namespace Moneybox.App.Features
{
    public class TransferMoney
    {
        private IAccountRepository accountRepository;
        private INotificationService notificationService;

        public TransferMoney(IAccountRepository accountRepository, INotificationService notificationService)
        {
            this.accountRepository = accountRepository;
            this.notificationService = notificationService;
        }

        public void Execute(Guid fromAccountId, Guid toAccountId, decimal amount)
        {
            var from = this.accountRepository.GetAccountById(fromAccountId);
            var to = this.accountRepository.GetAccountById(toAccountId);

            // Check accounts aren't the same
            if(from.AccountIsDifferentFrom(toAccountId))
            {
                // Check sender has enough money
                if(from.HasFunds(amount))
                {
                    // Check receiver has not hit the pay in limit
                    if (to.IsUnderPayInLimit(amount))
                    {
                        // Process transaction
                        from.GiveMoney(amount);
                        to.ReceiveMoney(amount);
                        this.accountRepository.Update(from);
                        this.accountRepository.Update(to);
                    }
                    else
                    {
                        throw new InvalidOperationException("The account receiving has reached its pay in limit.");
                    }
                }
                else
                {
                    throw new InvalidOperationException("Insufficient funds to make transfer");
                }
                // Check if receiver has neared pay in limit
                if (to.IsNearPayInLimit()) this.notificationService.NotifyApproachingPayInLimit(to.User.Email);
            }
            else
            {
                throw new InvalidOperationException("The account sending and the account receiving are the same.");
            }
        }
    }
}
