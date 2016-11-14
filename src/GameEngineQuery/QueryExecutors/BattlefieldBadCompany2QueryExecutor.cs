using System;
using System.Net;
using GameEngineQuery.Constants.Enumerations;
using GameEngineQuery.Exceptions;
using GameEngineQuery.PacketModels;

namespace GameEngineQuery.QueryExecutors
{
    public class BattlefieldBadCompany2QueryExecutor : BattlefieldQueryExecutor<BattlefieldBadCompany2ServerInfo, BattlefieldPlayerInfo>
    {
        public BattlefieldBadCompany2QueryExecutor(string ipAddress, ushort port) : base(ipAddress, port)
        {
        }

        public BattlefieldBadCompany2QueryExecutor(EndPoint endPoint) : base(endPoint)
        {
        }

        public override BattlefieldBadCompany2ServerInfo GetServerInfo()
        {
            var requestPacket = requestFactory.CreateRequestPacket(RequestPacketType.BattlefieldServerInfo);

            try
            {
                this.ResponsePacket = this.HandleGameEngineQuery(requestPacket);
            }
            catch (Exception exception)
            {
                throw new BattlefieldQueryException(ExceptionType.CouldNotOpenBattlefieldTcpConnection, exception);
            }

            if (this.ResponsePacket[16] != 0x4F || this.ResponsePacket[17] != 0x4B)
            {
                throw new BattlefieldQueryException(ExceptionType.InvalidResponsePacketForBattlefieldServerInfoRequest);
            }

            this.originalLength = this.ResponsePacket.Length;

            BattlefieldBadCompany2ServerInfo info = new BattlefieldBadCompany2ServerInfo
            {
                Padding = this.ResponsePacket[this.index++],
                Padding2 = this.ResponsePacket[this.index++],
                Padding3 = this.ResponsePacket[this.index++],
                Padding4 = this.ResponsePacket[this.index++],
                Padding5 = this.ResponsePacket[this.index++],
                Padding6 = this.ResponsePacket[this.index++],
                Padding7 = this.ResponsePacket[this.index++],
                Padding8 = this.ResponsePacket[this.index++],
                Padding9 = this.ResponsePacket[this.index++],
                Padding10 = this.ResponsePacket[this.index++],
                Padding11 = this.ResponsePacket[this.index++],
                Padding12 = this.ResponsePacket[this.index++],
                Response = this.GetNextDataAsString(),
                Name = this.GetNextDataAsString(),
                Players = this.GetNextDataAsUshort(),
                MaxPlayers = this.GetNextDataAsUshort(),
                GameType = this.GetNextDataAsString(),
                Map = this.GetNextDataAsString(),
                RoundsPlayed = this.GetNextDataAsUshort(),
                RoundsTotal = this.GetNextDataAsUshort(),
                Tickets = this.GetTickets(),
                TargetScore = this.GetNextDataAsUshort(),
                Online = this.FakeNextData<bool>() == false,
                Ranked = this.GetNextDataAsBoolean(),
                Punkbuster = this.GetNextDataAsBoolean(),
                Passworded = this.GetNextDataAsBoolean(),
                Uptime = this.GetNextDataAsUint(),
                RoundTime = this.GetNextDataAsUint(),
                Padding13 = this.GetNextDataAsString(),
                Padding14 = this.GetNextDataAsString(),
                Address = this.GetNextDataAsString(),
                PunkbusterVersion = this.GetNextDataAsString(),
                JoinQueue = this.GetNextDataAsBoolean(),
                Region = this.GetNextDataAsString()
            };

            return info;
        }
    }
}