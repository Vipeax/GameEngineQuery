using GameEngineQuery.QueryExecutors;
using Microsoft.AspNetCore.Mvc;

namespace ChivStatus.Controllers
{
    [Route("api/source")]
    public class SourceEngineQueryController : QueryController<SourceEngineQueryExecutor>
    {
        protected override string KeyFormatStringPrefix => "SE";
    }
}
