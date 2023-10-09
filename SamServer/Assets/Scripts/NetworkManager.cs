using Riptide;
using Riptide.Utils;
#if !UNITY_EDITOR
using System;
#endif
using UnityEngine;

namespace SamServer.Networking
{
    public enum ServerToClientId : ushort
    {
        SpawnPlayer = 1,
        PlayerMovement,
    }
    public enum ClientToServerId : ushort
    {
        PlayerName = 1,
        FaceUpdate,
        EyeUpdate
    }

    public class NetworkManager : DevSingleton<NetworkManager>
    {
        [SerializeField] private ushort port;
        [SerializeField] private ushort maxClientCount;
        [SerializeField] private GameObject playerPrefab;

        public GameObject PlayerPrefab => playerPrefab;

        public Server Server { get; private set; }



        private void Start()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;

#if UNITY_EDITOR
            RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);
#else
            Console.Title = "Server";
            Console.Clear();
            Application.SetStackTraceLogType(UnityEngine.LogType.Log, StackTraceLogType.None);
            RiptideLogger.Initialize(Debug.Log, true);
#endif

            Server = new Server();
            Server.ClientConnected += NewPlayerConnected;
            Server.ClientDisconnected += PlayerLeft;

            Server.Start(port, maxClientCount);
        }

        private void FixedUpdate()
        {
            Server.Update();
        }

        private void OnApplicationQuit()
        {
            Server.Stop();

            Server.ClientConnected -= NewPlayerConnected;
            Server.ClientDisconnected -= PlayerLeft;
        }

        private void NewPlayerConnected(object sender, ServerConnectedEventArgs e)
        {
            DebugDisplay.Instance.ChangeFadeCanvasAlpha(0f);
            foreach (Player player in Player.List.Values)
            {
                if (player.Id != e.Client.Id)
                {
                    player.SendSpawn(e.Client.Id);
                }
            }
        }

        private void PlayerLeft(object sender, ServerDisconnectedEventArgs e)
        {
            DebugDisplay.Instance.ChangeFadeCanvasAlpha(1f, e);
            //Destroy(Player.List[e.Client.Id].gameObject);
        }
    }
}
