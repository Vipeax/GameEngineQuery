using GameEngineQuery.PacketModels;
using GameEngineQuery.QueryExecutors;
using Microsoft.AspNetCore.Mvc;

namespace ChivStatus.Controllers
{
    [Route("api/source")]
    public class SourceEngineQueryController : QueryController<SourceEngineQueryExecutor, A2SInfo>
    {
        protected override string KeyFormatStringPrefix => "SE";
    }
}
