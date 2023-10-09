using System.Collections.Generic;
using Riptide;
using UnityEngine;

namespace SamServer.Networking
{
    public class Player : MonoBehaviour
    {
        public static Dictionary<ushort, Player> List { get; private set; } = new Dictionary<ushort, Player>();

        public ushort Id { get; private set; }
        public string Username { get; private set; }


        [SerializeField] SkinnedMeshRenderer serverHeadMesh;
        public static int TotalBlendShapes { get; private set; } = 0;
        static float[] LatestBlendValues { get; set; }

        [Header("Eye Related Updates")]
        [SerializeField] bool updateEyes = true;
        [SerializeField] GameObject leftEye = null;
        [SerializeField] GameObject rightEye = null;

        private void Start()
        {
            if (serverHeadMesh == null) return;

            TotalBlendShapes = serverHeadMesh.sharedMesh.blendShapeCount;
            LatestBlendValues = new float[TotalBlendShapes];
            Debug.Log($"Total blendshapes array set to {LatestBlendValues.Length}. Total blendshapes = {TotalBlendShapes}");
        }

        Quaternion latestLeftEyeQuaternion;
        Quaternion latestRightEyeQuaternion;
        private void Update()
        {
            if (NetworkManager.Instance.Server.ClientCount == 0) return;

            this.gameObject.transform.position = latestHeadPosition;
            this.gameObject.transform.eulerAngles = latestHeadRotation;

            if (serverHeadMesh == null) return;

            for (int i = 0; i < LatestBlendValues.Length; i++)
            {
                serverHeadMesh.SetBlendShapeWeight(i, LatestBlendValues[i]);
            }

            if (!updateEyes || leftEye == null || rightEye == null) return;

            // need to multiply the Z axis rotation by -1 as the data being read turns the eye the opposite way im looking
            latestLeftEyeQuaternion = Quaternion.Euler(latestLeftEyeRotation.x, latestLeftEyeRotation.y * -1, latestLeftEyeRotation.z);
            leftEye.transform.rotation = latestLeftEyeQuaternion;

            latestRightEyeQuaternion = Quaternion.Euler(latestRightEyeRotation.x, latestRightEyeRotation.y * -1, latestRightEyeRotation.z);
            rightEye.transform.rotation = latestRightEyeQuaternion;
        }

        private void OnDestroy()
        {
            List.Remove(Id);
        }

        public static void Spawn(ushort id, string username)
        {
            // Spawn the head object at -20 on the Y axis. We will later tween the position to origin. This will create a smoother entrance
            Player player = Instantiate(NetworkManager.Instance.PlayerPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity).GetComponent<Player>();
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
        public static Vector3 latestHeadPosition { get; private set; } = Vector3.zero;
        public static Vector3 latestHeadRotation { get; private set; } = Vector3.zero;
        [MessageHandler((ushort)ClientToServerId.FaceUpdate)]
        private static void HeadUpdateReceived(ushort fromClientId, Message message)
        {
            //Player player = List[fromClientId];
            latestHeadPosition = message.GetVector3();
            latestHeadRotation = message.GetVector3();
            LatestBlendValues = message.GetFloats();
        }

        // Handles incoming message from client containing eye rotation data
        public static Vector3 latestLeftEyeRotation { get; private set; } = Vector3.zero;
        public static Vector3 latestRightEyeRotation { get; private set; } = Vector3.zero;
        [MessageHandler((ushort)(ClientToServerId.EyeUpdate))]
        private static void EyeUpdateReceived(ushort fromClientId, Message message)
        {
            //Player player = List[fromClientId];
            latestLeftEyeRotation = message.GetVector3();
            latestRightEyeRotation = message.GetVector3();
        }
        #endregion
    }
}
