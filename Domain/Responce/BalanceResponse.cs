using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Responce
{
    // Класс для десериализации ответа REST API по балансу пользователя.
    public class BalanceResponse
    {
        public decimal Balance { get; set; }
    }
}
