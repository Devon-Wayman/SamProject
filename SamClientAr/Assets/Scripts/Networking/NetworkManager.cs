using Riptide.Utils;
using System;
using UnityEngine;

namespace Riptide.Demos.DedicatedClient
{
    public enum ClientToServerId : ushort
    {
        PlayerInput,
    }

    public class NetworkManager : DevSingleton<NetworkManager>
    {
        [SerializeField] private ushort port;

        public Client Client { get; private set; }

        private void Start()
        {
            RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);

            Client = new Client();
            Client.Connected += DidConnect;
            Client.ConnectionFailed += FailedToConnect;
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
        }

        public void Connect(string serverIp)
        {
            Debug.Log($"Attempting to connect to server at {serverIp}:{port}");
            Client.Connect($"{serverIp}:{port}");
        }

        private void DidConnect(object sender, EventArgs e)
        {
            Debug.Log("Local client has connected to server");

            // TODO: Set a bool value for the ArHeadPlayerController to reference rather than
            // checking if the client is connected or not directly from the client
            //UIManager.Instance.SendName();
        }

        private void FailedToConnect(object sender, ConnectionFailedEventArgs e)
        {
            Debug.Log("Local client failed to connect to server");
            UIManager.Instance.BackToMain();
        }

        //private void PlayerLeft(object sender, ClientDisconnectedEventArgs e)
        //{
        //    Destroy(ArHeadPlayer.list[e.Id].gameObject);
        //}

        //private void DidDisconnect(object sender, DisconnectedEventArgs e)
        //{
        //    UIManager.Instance.BackToMain();

        //    foreach (ArHeadPlayer player in ArHeadPlayer.list.Values)
        //    {
        //        Destroy(player.gameObject);
        //    }
        //}
    }
}
