using Domain.Request;
using Infrastructure;
using Infrastructure.DbCont;
using Infrastructure.Repositiry;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace TestTask1.Controllers
{

        [Route("api/[controller]")]
        [ApiController]
        public class TransactionsController : ControllerBase
        {
            private readonly GameServiceDbContext _dbContext;

            public TransactionsController(GameServiceDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            [HttpPost("transfer")]
            public async Task<IActionResult> TransferMoney([FromBody] TransferRequest request)
            {

                var fromUser = await _dbContext.User.FirstOrDefaultAsync(u => u.UserId == request.FromUserId && !u.IsDeleted);
                var toUser = await _dbContext.User.FirstOrDefaultAsync(u => u.UserId == request.ToUserId && !u.IsDeleted);

                if (fromUser == null || toUser == null)
                {
                    return NotFound("Пользователь не найден");
                }

                if (fromUser.Balance < request.Amount)
                {
                    return BadRequest("Недостаточно средств");
                }

                fromUser.Balance -= request.Amount;
                toUser.Balance += request.Amount;

                var transaction = new GameTransactions
                {
                    fkFromUserId = fromUser.UserId,
                    fkToUserId = toUser.UserId,
                    Amount = request.Amount,
                    Reason = "Manual transfer",
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };

                _dbContext.GameTransactions.Add(transaction);
                await _dbContext.SaveChangesAsync();

                return Ok("Перевод выполнен успешно");
            }
        }

}
