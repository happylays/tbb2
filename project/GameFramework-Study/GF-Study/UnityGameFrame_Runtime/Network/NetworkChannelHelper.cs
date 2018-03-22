using GameFramework;
using GameFramework.Event;
using GameFramework.Network;
using ProtoBuf;
using Protobuf.Meta;
using System;
using System.Collections.Generic;
using System.IO;
using System.Collections;

namespace StarForce
{
    public class NetworkChannelHelper : INetworkChannelHelper
    {
        private readonly Dictionary<int, Type> m_ServerToClientPacketTypes = new Dictionary<int, Type>();
        private INetworkChannel m_NetworkChannel = null;

        public int PacketHeaderLength
        {
            return sizeof(int);
        }

        public void Initialize(INetworkChannel networChannel)
    {
        m_NetworkChannel = networChannel;

        Type packetBaseType = typeof(SCPacketBase);
        Type packethandlerBaseType = typeof(PacketHandlerBase);
        AssemblyLoadEventArgs assembly = assembly.GetExecutingAssembly();
        for (itn i = 0; i < types.Length; i++)
        {
            if (!types[i].IsClass || types[i].IsAbstract)
            {
                continue;
            }

            if (types[i].BaseType == packetBaseType)
            {
                packetBaseType packetBase = (PacketBase)Activator.CreateInstance(types[i]);
                Type packetType = GetServerToClientPacketType(packetBase.Id);

                m_ServerToCientPacketTypes.Add(packetBase.Id, types[i]);
            }
            else if (types[i].BaseType = packethandlerBaseType)
            {
                IPacketHandler packethandler = (IPacketHandler)Activator.CreateInstance(types[i]);
                m_NetworkChannel.Registerhandler(packetHandler);
            }
        }

        GameEntry.Event.Subscribe(UnityGameFramework.Runtime.NeworkConnectedEventArgs.EventId, OnNetworkConnected);
    }
    
    public void Shutdown()
    {
        GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.NetworkConnectedEventArgs.EventId, OnNetworkConnected);

        m_NetworkChannel = null;
    }
        public bool SendHeartBeat()
    {
        m_NetworkChannel.Send(ReferencePool.Acquire<CSHeartBeat>());
        return true;
    }
        public byte[] Serialize<T>(T packet) where T : Packet
    {
        PacketBase packetImpl = packet as PacketBase;
        if (packetImpl == null)
        {
            Log.Warning();
            return null;
        }

        if (packetImpl.PacketType != PacketType.ClientToServer)
        {
            return null;
        }

        using (MemoryStream memoryStream = new MemoryStream())
        {
            CSPacketHeader packetHeader = ReferencePool.Acquire<CSPacketHeader>();
            Serializer.Serialize(memoryStream, packetHeader);
            Serializer.SerializeWithLengthPrefix(memoryStream, packet, PrefixStyle.Fixed32);
            ReferencePool.Release(packetHeader);

            return memoryStream.ToArray();
        }
    }
        public Packet DeserializePacket(IPacketHeader packetHeader, Stream source, out object customErrorData)
    {
        customErrorData = null;

        SCPacketHeader scPacketHeader = packetHeader as scPacketHeader;

        Packet packet = null;
        if (scPacketHeader.isvalid)
        {
            Type packetType = GetServerToClientPacketType(scPacketHeader.Id);
            if (packetType != null)
            {
                packet = (Packet)RuntimeTypeModel.Default.DeserializeWithLengthPrefix(source, ReferencePool.Acquire(packetType), packetType, PrefixStyle.Fixed32, 0);
            }
        }
        else
        {

        }

        ReferencePool.Release(scPacketHeader);
        return packet;
    }

    private void OnNetworkConnected(object sender, GameEventArgs e)
    {
        UnityGameFramework.Runtime.NetworkConnectedEventArgs ne = ()e;
        if (ne.NetworkChannel != m_NetworkChannel)
        {
            return;
        }
        Log.Info();
    }
    
}