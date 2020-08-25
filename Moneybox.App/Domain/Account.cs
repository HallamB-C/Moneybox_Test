using System;

namespace Moneybox.App
{
    public class Account
    {
        public const decimal PayInLimit = 4000m;

        public Guid Id { get; set; }

        public User User { get; set; }

        public decimal Balance { get; set; }

        public decimal Withdrawn { get; set; }

        public decimal PaidIn { get; set; }

        // Account checks
        public bool AccountIsDifferentFrom(Guid otherId)
        {
            if(Id.Equals(otherId))
            {
                return false;
            }
            return true;
        }

        public bool HasFunds(decimal amount)
        {
            if(Balance > amount)
            {
                return true;
            }
            return false;
        }

        public bool IsUnderPayInLimit(decimal amount)
        {
            if(PaidIn + amount > PayInLimit)
            {
                return false;
            }
            return true;
        }

        public bool IsNearPayInLimit()
        {
            if(PaidIn > 3500m)
            {
                return true;
            }
            return false;
        }

        public bool IsNearZeroBalance()
        {
            if(Balance < 500m)
            {
                return true;
            }
            return false;
        }

        public decimal CheckBalance()
        {
            return Balance;
        }

        // Handle moving money
        public decimal WithdrawFunds(decimal amount)
        {
            if (HasFunds(amount))
            {
                this.Balance -= amount;
                return amount;
            }
            else
            {
                return 0m;
            }
        }

        public decimal DepositFunds(decimal amount)
        {
            if (IsUnderPayInLimit(amount))
            {
                this.Balance += amount;
                return Balance;
            }
            else
            {
                return 0m;
            }
        }

        public decimal ReceiveMoney(decimal amount)
        {
            DepositFunds(amount);
            UpdatePaidIn(amount);
            return UpdateBalanceAfterDeposit(amount);
        }
        // Could be better named
        public decimal GiveMoney(decimal amount)
        {
            WithdrawFunds(amount);
            UpdateWithdrawn(amount);
            return UpdateBalanceAfterWithdrawal(amount);
        }

        // Updates
        public decimal UpdateWithdrawn(decimal amount)
        {
            this.Withdrawn -= amount;
            return this.Withdrawn;
        }

        public decimal UpdatePaidIn(decimal amount)
        {
                this.PaidIn += amount;
                return this.PaidIn;
        }

        public decimal UpdateBalanceAfterWithdrawal(decimal amount)
        {
            this.Balance -= amount;
            return Balance;
        }

        public decimal UpdateBalanceAfterDeposit(decimal amount)
        {
            this.Balance += amount;
            return Balance;
        }
    }
}
