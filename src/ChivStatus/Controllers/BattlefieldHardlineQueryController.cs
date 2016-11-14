using GameEngineQuery.PacketModels;
using GameEngineQuery.QueryExecutors;
using Microsoft.AspNetCore.Mvc;

namespace ChivStatus.Controllers
{
    [Route("api/bfh")]
    public class BattlefieldHardlineQueryController : QueryController<BattlefieldHardlineQueryExecutor, BattlefieldHardlineServerInfo, BattlefieldPlayerInfo>
    {
        protected override string KeyFormatStringPrefix => "BFH";
    }
}