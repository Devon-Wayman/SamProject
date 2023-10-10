using Riptide;
using Riptide.Utils;
using SamClient.PlayerAssets;
using SamClient.Utils;
using System;
using UnityEngine;

namespace SamClient.Networking
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
        [SerializeField] private ushort port = 7777;

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

        public void Connect(string ip)
        {
            Debug.Log($"Attempting to connect to {ip} through port {port}");
            Client.Connect($"{ip}:{port}");
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
            Destroy(Player.list[e.Id].gameObject);
        }

        private void DidDisconnect(object sender, DisconnectedEventArgs e)
        {
            UIManager.Instance.BackToMain();
        }
    }
}
