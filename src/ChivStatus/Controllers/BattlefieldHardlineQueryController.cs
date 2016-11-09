using GameEngineQuery.PacketModels;
using GameEngineQuery.QueryExecutors;
using Microsoft.AspNetCore.Mvc;

namespace ChivStatus.Controllers
{
    [Route("api/bfh")]
    public class BattlefieldHardlineQueryController : QueryController<BattlefieldHardlineQueryExecutor, BattlefieldHardlineServerInfo>
    {
        protected override string KeyFormatStringPrefix => "BF";
    }
}