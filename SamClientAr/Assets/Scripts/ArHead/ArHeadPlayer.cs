using System.Collections.Generic;
using Riptide;
using Riptide.Demos.DedicatedClient;
using UnityEngine;

public class ArHeadPlayer : MonoBehaviour
{
    public static Dictionary<ushort, ArHeadPlayer> list = new Dictionary<ushort, ArHeadPlayer>();

    [SerializeField] private ushort id;
    [SerializeField] private string username;

    public void Move(Vector3 newPosition, Vector3 forward)
    {
        transform.position = newPosition;
        
        if (id != NetworkManager.Instance.Client.Id) // Don't overwrite local player's forward direction to avoid noticeable rotational snapping
            transform.forward = forward;
    }


    private void OnDestroy()
    {
        list.Remove(id);
    }


    /// <summary>
    /// Spawns in a player object. Since this app only has one connection at all times we will only need
    /// to spawn in the local player prefab. Look into removing all references to the secondary prefabs
    /// </summary>
    /// <param name="id"></param>
    /// <param name="username"></param>
    /// <param name="position"></param>
    public static void Spawn(ushort id, string username, Vector3 position)
    {
        ArHeadPlayer player;
        if (id == NetworkManager.Instance.Client.Id)
            player = Instantiate(NetworkManager.Instance.LocalPlayerPrefab, position, Quaternion.identity).GetComponent<ArHeadPlayer>();
        else
            player = Instantiate(NetworkManager.Instance.PlayerPrefab, position, Quaternion.identity).GetComponent<ArHeadPlayer>();

        player.name = $"Player {id} ({username})";
        player.id = id;
        player.username = username;
        list.Add(player.id, player);
    }

    #region Messages
    // This is the message received from the server when a new player joins for us to spawn in
    // but in the case of this application it is just used to confirm our connectivity and
    // spawn in the default AR Head object
    [MessageHandler((ushort)ServerToClientId.SpawnPlayer)]
    private static void SpawnPlayer(Message message)
    {
        Spawn(message.GetUShort(), message.GetString(), message.GetVector3());
    }

    // This message would have been used to update another connected clients position
    // but we shouldnt need it as communication in this application will be one way and
    // we wonnt need to receive anyone elses inputs
    [MessageHandler((ushort)ServerToClientId.PlayerMovement)]
    private static void PlayerMovement(Message message)
    {
        ushort playerId = message.GetUShort();
        if (list.TryGetValue(playerId, out ArHeadPlayer player))
            player.Move(message.GetVector3(), message.GetVector3());
    }
    #endregion
}
