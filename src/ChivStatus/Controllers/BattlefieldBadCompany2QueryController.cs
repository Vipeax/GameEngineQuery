using GameEngineQuery.PacketModels;
using GameEngineQuery.QueryExecutors;
using Microsoft.AspNetCore.Mvc;

namespace ChivStatus.Controllers
{
    [Route("api/bfbc2")]
    public class BattlefieldBadCompany2QueryController : QueryController<BattlefieldBadCompany2QueryExecutor, BattlefieldBadCompany2ServerInfo>
    {
        protected override string KeyFormatStringPrefix => "BF";
    }
}