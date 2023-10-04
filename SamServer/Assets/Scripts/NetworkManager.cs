using System.Net;
using System.Net.Sockets;
using Riptide.Utils;
#if !UNITY_EDITOR
using System;
#endif
using UnityEngine;

namespace Riptide.Demos.DedicatedServer
{
    //public enum ServerToClientId : ushort
    //{
    //    SpawnPlayer = 1,
    //    PlayerMovement,
    //}
    public enum ClientToServerId : ushort
    {
        PlayerInput,
    }

    public class NetworkManager : DevSingleton<NetworkManager>
    {
        string serversIp = string.Empty;


        [SerializeField] private ushort port;
        [SerializeField] private ushort maxClientCount;

        public Server Server { get; private set; }


        private void Start()
        {
            SetServersIp();

            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 30;

#if UNITY_EDITOR
            RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);
#else
            Console.Title = "Server";
            Console.Clear();
            Application.SetStackTraceLogType(UnityEngine.LogType.Log, StackTraceLogType.None);
            RiptideLogger.Initialize(Debug.Log, true);
#endif

            Server = new Server();
            Server.ClientConnected += ClientConnected;
            Server.ClientDisconnected += ClientDisconnected;

            Server.Start(port, maxClientCount);
        }

        private void SetServersIp()
        {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        Debug.Log(ip.ToString());
                        serversIp = ip.ToString();
                    }
                }
        }

        private void FixedUpdate()
        {
            Server.Update();
        }

        private void OnApplicationQuit()
        {
            Server.Stop();
            Server.ClientConnected -= ClientConnected;
            Server.ClientDisconnected -= ClientDisconnected;
        }


        /// <summary>
        /// Executed when the single client connects to the  server. Enables the head mesh renderer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClientConnected(object sender, ServerConnectedEventArgs e)
        {
            Debug.Log("CLIENT HAS CONNECTED");
            Player.Instance.gameObject.SetActive(true);
        }

        /// <summary>
        /// Executed when the single client disconnects. For now just disabling the head mesh but can incorporate an outro to play 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClientDisconnected(object sender, ServerDisconnectedEventArgs e)
        {
            Debug.Log("CLIENT HAS DISCONNECTED");
            Player.Instance.gameObject.SetActive(false);
        }


        // print the servers available ip addresses when no one is connected
        private void OnGUI()
        {
            if (!(Server.ClientCount == 0)) return;
            GUI.Label(new Rect(10, 10, 200, 20), $"Server address: {serversIp}!");
        }
    }
}
