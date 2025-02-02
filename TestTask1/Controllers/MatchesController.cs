using Infrastructure.DbCont;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Request;

namespace TestTask1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchesController : ControllerBase
    {
        private readonly GameServiceDbContext _dbContext;

        public MatchesController(GameServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateMatch([FromBody] CreateMatchRequest request)
        {
            var user = await _dbContext.User.FirstOrDefaultAsync(u => u.UserId == request.Player1Id && !u.IsDeleted);
            if (user == null)
            {
                return NotFound("Пользователь не найден");
            }

            if (user.Balance < request.Stake)
            {
                return BadRequest("Недостаточно средств для создания матча");
            }

            var match = new MatchHistory
            {
                fkPlayer1Id = request.Player1Id,
                Stake = request.Stake,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            _dbContext.MatchHistory.Add(match);
            await _dbContext.SaveChangesAsync();

            return Ok(new { matchId = match.MatchHistoryId });
        }
    }

    
}
