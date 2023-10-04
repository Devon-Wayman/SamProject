using Riptide.Utils;
using System;
using UnityEngine;

namespace Riptide.Demos.DedicatedClient
{
    public enum ServerToClientId : ushort
    {
        SpawnPlayer = 1,
        PlayerMovement,
    }
    public enum ClientToServerId : ushort
    {
        PlayerName = 1,
        PlayerInput,
    }

    public class NetworkManager : DevSingleton<NetworkManager>
    {

        [SerializeField] private ushort port;

        [SerializeField] private GameObject localPlayerPrefab;
        [SerializeField] private GameObject playerPrefab;

        public GameObject LocalPlayerPrefab => localPlayerPrefab;
        public GameObject PlayerPrefab => playerPrefab;

        public Client Client { get; private set; }

        private void Start()
        {
            RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);

            Client = new Client();
            Client.Connected += DidConnect;
            Client.ConnectionFailed += FailedToConnect;
            Client.ClientDisconnected += PlayerLeft;
            Client.Disconnected += DidDisconnect;
        }

        private void FixedUpdate()
        {
            Client.Update();
        }

        private void OnApplicationQuit()
        {
            Client.Disconnect();

            Client.Connected -= DidConnect;
            Client.ConnectionFailed -= FailedToConnect;
            Client.ClientDisconnected -= PlayerLeft;
            Client.Disconnected -= DidDisconnect;
        }

        public void Connect(string serverIp)
        {
#if UNITY_EDITOR
            Debug.Log($"Attempting to connect to server at {serverIp}:{port}");
#endif
            Client.Connect($"{serverIp}:{port}");
        }

        private void DidConnect(object sender, EventArgs e)
        {
            UIManager.Instance.SendName();
        }

        private void FailedToConnect(object sender, ConnectionFailedEventArgs e)
        {
            UIManager.Instance.BackToMain();
        }

        private void PlayerLeft(object sender, ClientDisconnectedEventArgs e)
        {
            //Destroy(Player.list[e.Id].gameObject);
            Destroy(ArHeadPlayer.list[e.Id].gameObject);
        }

        private void DidDisconnect(object sender, DisconnectedEventArgs e)
        {
            UIManager.Instance.BackToMain();

            //foreach (Player player in Player.list.Values)
            //    Destroy(player.gameObject);
            foreach (ArHeadPlayer player in ArHeadPlayer.list.Values)
            {
                Destroy(player.gameObject);
            }
        }
    }
}
