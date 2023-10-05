using System.Collections.Generic;
using UnityEngine;

namespace Riptide.Demos.DedicatedServer
{
    public class Player : MonoBehaviour
    {
        public static Dictionary<ushort, Player> List { get; private set; } = new Dictionary<ushort, Player>();

        public ushort Id { get; private set; }
        public string Username { get; private set; }


        [SerializeField] SkinnedMeshRenderer serverHeadMesh;
        public static int TotalBlendShapes { get; private set; } = 0;
        static float[] LatestBlendValues { get; set; }

        private void Start()
        {
            if (serverHeadMesh == null) return;

            TotalBlendShapes = serverHeadMesh.sharedMesh.blendShapeCount;
            LatestBlendValues = new float[TotalBlendShapes];
        }

        private void Update()
        {
            if (NetworkManager.Instance.Server.ClientCount == 0) return;

            this.gameObject.transform.position = latestPosition;
            this.gameObject.transform.localEulerAngles = latestRotation;
        }
        private void FixedUpdate()
        {
            if (NetworkManager.Instance.Server.ClientCount == 0) return;

            if (serverHeadMesh == null)return;

            // Set the local head's blend values to the latest received from the connected client
            for (int i = 0; i < TotalBlendShapes; i++)
            {
                serverHeadMesh.SetBlendShapeWeight(i, LatestBlendValues[i]);
            }
        }

        private void OnDestroy()
        {
            List.Remove(Id);
        }

        public static void Spawn(ushort id, string username)
        {
            Player player = Instantiate(NetworkManager.Instance.PlayerPrefab, new Vector3(0f, 1f, 0f), Quaternion.identity).GetComponent<Player>();
            player.name = $"Player {id} ({(username == "" ? "Guest" : username)})";
            player.Id = id;
            player.Username = username;

            player.SendSpawn();
            List.Add(player.Id, player);
        }

        #region Messages
        /// <summary>Sends a player's info to the given client.</summary>
        /// <param name="toClient">The client to send the message to.</param>
        public void SendSpawn(ushort toClient)
        {
            NetworkManager.Instance.Server.Send(GetSpawnData(Message.Create(MessageSendMode.Reliable, ServerToClientId.SpawnPlayer)), toClient);
        }
        /// <summary>Sends a player's info to all clients.</summary>
        private void SendSpawn()
        {
            NetworkManager.Instance.Server.SendToAll(GetSpawnData(Message.Create(MessageSendMode.Reliable, ServerToClientId.SpawnPlayer)));
        }

        private Message GetSpawnData(Message message)
        {
            message.AddUShort(Id);
            message.AddString(Username);
            message.AddVector3(transform.position);
            return message;
        }


        [MessageHandler((ushort)ClientToServerId.PlayerName)]
        private static void PlayerName(ushort fromClientId, Message message)
        {
            Spawn(fromClientId, message.GetString());
        }


        // Handles incoming message from client containing latest head transforms and blend values
        public static Vector3 latestPosition { get; private set; } = Vector3.zero;
        public static Vector3 latestRotation { get; private set; } = Vector3.zero;
        [MessageHandler((ushort)ClientToServerId.FaceUpdate)]
        private static void PlayerInput(ushort fromClientId, Message message)
        {
            Player player = List[fromClientId];
            latestPosition = message.GetVector3();
            latestRotation = message.GetVector3();
            //LatestBlendValues = message.GetFloats(TotalBlendShapes);
        }
        #endregion
    }
}
