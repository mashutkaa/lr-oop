using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class AccountFactory
    {
        public static GameAccount CreateAccount(string userName, string accountType = "Standard")
        {
            return accountType switch
            {
                "Premium" => new PremiumAccount(userName),
                "Restriction" => new RestrictionsAccount(userName),
                _ => new GameAccount(userName)
            };
        }
    }

}
