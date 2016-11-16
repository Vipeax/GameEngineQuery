using GameEngineQuery.PacketModels;
using GameEngineQuery.QueryExecutors;
using Microsoft.AspNetCore.Mvc;

namespace ChivStatus.Controllers
{
    [Route("api/ut3")]
    [Route("api/cmw")]
    [Route("api/arma3")]
    [Route("api/arma2")]
    [Route("api/tf2")]
    [Route("api/gmod")]
    [Route("api/dayz")]
    [Route("api/csgo")]
    [Route("api/css")]
    [Route("api/cscz")]
    [Route("api/cs16")]
    [Route("api/hldm2")]
    [Route("api/hldm")]
    [Route("api/source")]
    public class SourceEngineQueryController : QueryController<SourceEngineQueryExecutor, A2SInfo, A2SPlayer>
    {
        protected override string KeyFormatStringPrefix => "SE";
    }
}
