using Riptide;
using Riptide.Demos.DedicatedClient;
using UnityEngine;

/// <summary>
/// Holds all logic for sending local AR heads transforms and blendshape data to the server
/// See original PlayerController class from project template for reference
/// </summary>
public class ArHeadPlayerController : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer arFaceSkinnedMeshRenderer = null;
    [SerializeField] int totalBlendShapes = 0;
    [SerializeField] private float[] currentBlendValues;

    private void Start()
    {
        totalBlendShapes = arFaceSkinnedMeshRenderer.sharedMesh.blendShapeCount;
        currentBlendValues = new float[totalBlendShapes];
    }

    private void Update()
    {
        // Sample inputs every frame and store them until they're sent. This prevents values from being skipped between fixedupdate calls
        for (int i = 0; i < totalBlendShapes; i++)
        {
            currentBlendValues[i] = arFaceSkinnedMeshRenderer.GetBlendShapeWeight(i);
        }
    }

    private void FixedUpdate()
    {
#if UNITY_EDITOR
        //Debug.Log($"First blendshape weight: {currentBlendValues[0]}");
#endif
        if (!NetworkManager.Instance.Client.IsConnected) return;

        SendInput();
    }

    #region Messages
    // TODO: This is where we will send the array of facial blend shapes as well as the heads position
    // and rotation relative to world origin
    private void SendInput()
    {
        Message message = Message.Create(MessageSendMode.Unreliable, ClientToServerId.PlayerInput);
        message.AddVector3(this.gameObject.transform.position);
        message.AddVector3(this.gameObject.transform.localEulerAngles);
        message.AddFloats(currentBlendValues);
        NetworkManager.Instance.Client.Send(message);
    }
    #endregion
}
