using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Request
{
    public class CreateMatchRequest
    {
        public long Player1Id { get; set; }
        public decimal Stake { get; set; }
    }
}
