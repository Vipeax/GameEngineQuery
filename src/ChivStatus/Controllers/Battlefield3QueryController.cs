using GameEngineQuery.PacketModels;
using GameEngineQuery.QueryExecutors;
using Microsoft.AspNetCore.Mvc;

namespace ChivStatus.Controllers
{
    [Route("api/bf3")]
    public class Battlefield3QueryController : QueryController<Battlefield3QueryExecutor, Battlefield3ServerInfo, BattlefieldPlayerInfo>
    {
        protected override string KeyFormatStringPrefix => "BF3";
    }
}
