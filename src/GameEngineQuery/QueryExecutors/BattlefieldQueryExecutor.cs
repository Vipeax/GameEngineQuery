using System;
using System.Net;
using System.Net.Sockets;
using GameEngineQuery.Constants;
using GameEngineQuery.Constants.Enumerations;
using GameEngineQuery.Exceptions;
using GameEngineQuery.PacketModels;
using System.Linq;

namespace GameEngineQuery.QueryExecutors
{
    public abstract class BattlefieldQueryExecutor<TSI> : SocketQueryExecutor<TSI> where TSI : BattlefieldServerInfo, new()
    {
        protected byte[] ResponsePacket;
        protected int index;
        protected int originalLength;

        protected BattlefieldQueryExecutor(string ipAddress, ushort port) : base(ipAddress, port)
        {
        }

        protected BattlefieldQueryExecutor(EndPoint endPoint) : base(endPoint)
        {
        }

        public override TSI GetServerInfo()
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

            TSI info = new TSI
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
                Address = this.GetNextDataAsString(),
                PunkbusterVersion = this.GetNextDataAsString(),
                JoinQueue = this.GetNextDataAsBoolean(),
                Region = this.GetNextDataAsString()
            };

            return info;
        }

        protected override byte[] HandleGameEngineQuery(byte[] request)
        {
            using (var s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                int packetSize = Values.BattlefieldConstants.ServerInfo.ResponsePacketSize;

                s.SendTimeout = Values.ProjectConstants.SendTimeout;
                s.ReceiveTimeout = Values.ProjectConstants.ReceiveTimeout;

                try
                {
                    s.Connect(endPoint);
                }
                catch (SocketException)
                {
                    throw new BattlefieldQueryException(ExceptionType.CouldNotOpenBattlefieldTcpConnection);
                }

                try
                {
                    s.Send(request);
                }
                catch (SocketException)
                {
                    throw new BattlefieldQueryException(ExceptionType.SocketSend);
                }

                byte[] bfServerInfoResultBytes = new byte[packetSize];

                try
                {
                    s.Receive(bfServerInfoResultBytes);
                }
                catch (SocketException)
                {
                    throw new BattlefieldQueryException(ExceptionType.SocketReceive);
                }

                return bfServerInfoResultBytes;
            }
        }

        protected float[] GetTickets()
        {
            var teamCount = this.GetNextDataAsUshort();
            var ticketData = new float[teamCount];
            
            for (int i = 0; i < teamCount; i++)
            {
                ticketData[i] = this.GetNextDataAsFloat();
            }

            return ticketData;
        }

        private float GetNextDataAsFloat()
        {
            return float.Parse(this.GetNextDataAsString());
        }

        protected uint GetNextDataAsUint()
        {
            return Convert.ToUInt32(this.GetNextDataAsString());
        }

        protected ushort GetNextDataAsUshort()
        {
            return Convert.ToUInt16(this.GetNextDataAsString());
        }

        protected bool GetNextDataAsBoolean()
        {
            return bool.Parse(this.GetNextDataAsString());
        }

        protected T FakeNextData<T>()
        {
            this.GetNextDataAsString();
            return default(T);
        }

        protected string GetNextDataAsString()
        {
            var toSkip = index - (originalLength - this.ResponsePacket.Length);

            var remaining = toSkip > 0 ? this.ResponsePacket.Skip(toSkip).ToArray() : this.ResponsePacket;

            var count = remaining[0];

            this.index += 5 + count;

            var newBuffer = new byte[this.ResponsePacket.Length - this.index];
            
            Array.Copy(this.ResponsePacket, this.index, newBuffer, 0, newBuffer.Length);

            this.index = 0;

            this.ResponsePacket = newBuffer;
            
            var dataString = new string(remaining.Skip(4).Take(count).Select(c => (char) c).ToArray());
            
            return dataString.Trim();
        }
    }
}