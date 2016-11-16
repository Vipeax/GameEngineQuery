using GameEngineQuery.PacketModels;
using GameEngineQuery.QueryExecutors;
using Microsoft.AspNetCore.Mvc;

namespace ChivStatus.Controllers
{
    [Route("api/codmw2")]
    [Route("api/codmw")]
    [Route("api/cod4")]
    [Route("api/cod3")]
    [Route("api/cod2")]
    [Route("api/sof2")]
    [Route("api/sof")]
    [Route("api/wet")]
    [Route("api/rtcw")]
    [Route("api/quake3")]
    public class Quake3EngineQueryController : QueryController<Quake3EngineQueryExecutor, Quake3ServerInfo, Quake3PlayerInfo>
    {
        protected override string KeyFormatStringPrefix => "q3";
    }
}