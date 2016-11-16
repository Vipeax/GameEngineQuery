using System;
using GameEngineQuery.PacketModels;
using GameEngineQuery.QueryExecutors;
using Hangfire.Server;

namespace ChivStatus.BackgroundProcesses
{
    public class BackgroundServerWatcher : IBackgroundProcess
    {
        public void Execute(BackgroundProcessContext context)
        {
            this.GetValue();
            context.Wait(new TimeSpan(0, 0, 5));
        }

        private void GetValue()
        {
            IQueryExecutor<A2SInfo, A2SPlayer> queryExecutor = new SourceEngineQueryExecutor("66.150.121.148", 27019);
            queryExecutor.GetServerInfo();
        }
    }
}