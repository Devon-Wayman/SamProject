using Riptide.Demos.DedicatedClient;
using UnityEngine;

public class ArHeadPlayer : MonoBehaviour
{

    [SerializeField] private ushort id;

    public void Move(Vector3 newPosition, Vector3 forward)
    {
        transform.position = newPosition;
        
        if (id != NetworkManager.Instance.Client.Id) // Don't overwrite local player's forward direction to avoid noticeable rotational snapping
            transform.forward = forward;
    }
}
