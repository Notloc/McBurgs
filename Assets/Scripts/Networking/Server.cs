using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Net;
using Unity.Networking.Transport;
using Unity.Collections;

public class Server : MonoBehaviour
{
    [SerializeField] ushort portNumber;
    [Space]
    public UdpNetworkDriver m_Driver;
    public NetworkEndPoint m_EndPoint;

    private NativeList<NetworkConnection> m_Connections;

    private void Start()
    {
        m_Driver = new UdpNetworkDriver(new INetworkParameter[0]);
        m_EndPoint = NetworkEndPoint.Parse("0.0.0.0", portNumber);

        if (m_Driver.Bind(m_EndPoint) != 0)
            Debug.Log("Server failed to bind to port " + portNumber);
        else
            m_Driver.Listen();

        m_Connections = new NativeList<NetworkConnection>(8, Allocator.Persistent);
    }

    private void OnDestroy()
    {
        // Properly deallocate the non-GC managed memory
        m_Driver.Dispose();
        m_Connections.Dispose();
    }

    private void Update()
    {
        m_Driver.ScheduleUpdate().Complete(); // Force syncronous

        // Remove old connections
        for (int i = 0; i < m_Connections.Length; i++)
        {
            if (!m_Connections[i].IsCreated)
            {
                m_Connections.RemoveAtSwapBack(i);
                --i;
            }
        }

        // Accept new connections
        NetworkConnection c;
        while ((c = m_Driver.Accept()) != default(NetworkConnection))
        {
            m_Connections.Add(c);
            Debug.Log("Accepted a connection");
        }


        DataStreamReader stream;
        for (int i=0; i<m_Connections.Length; i++)
        {
            if (!m_Connections[i].IsCreated)
                continue;


            NetworkEvent.Type cmd;
            while ((cmd = m_Driver.PopEventForConnection(m_Connections[i], out stream)) != NetworkEvent.Type.Empty)
            {
                if (cmd == NetworkEvent.Type.Data)
                {
                    var readerContext = default(DataStreamReader.Context);
                    uint number = stream.ReadUInt(ref readerContext);
                    Debug.Log("Got " + number + " from the Client adding + 2 to it.");

                    number += 2;

                    using (var writer = new DataStreamWriter(4, Allocator.Temp))
                    {
                        writer.Write(number);
                        m_Driver.Send(NetworkPipeline.Null, m_Connections[i], writer);
                    }

                }
                else if (cmd == NetworkEvent.Type.Disconnect)
                {
                    Debug.Log("Client disconnected from server");
                    m_Connections[i] = default(NetworkConnection);
                }

            }
        }
    }
}
