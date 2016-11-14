using GameEngineQuery.PacketModels;
using GameEngineQuery.QueryExecutors;
using Microsoft.AspNetCore.Mvc;

namespace ChivStatus.Controllers
{
    [Route("api/bf4")]
    public class Battlefield4QueryController : QueryController<Battlefield4QueryExecutor, Battlefield4ServerInfo, BattlefieldPlayerInfo>
    {
        protected override string KeyFormatStringPrefix => "BF4";
    }
}