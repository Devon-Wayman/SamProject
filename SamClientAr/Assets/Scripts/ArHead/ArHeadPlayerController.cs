using Riptide;
using Riptide.Demos.DedicatedClient;
using UnityEngine;

/// <summary>
/// Holds all logic for sending local AR heads transforms and blendshape data to the server
/// See original PlayerController class from project template for reference
/// </summary>
public class ArHeadPlayerController : MonoBehaviour
{
    //private bool[] inputs;
    [SerializeField] private Mesh arHeadMesh = null;
    [SerializeField] private float[] arFaceBlendValues;

    private void Start()
    {
        // TODO: Set float array length to amount of blendshapes on shared mesh
        //inputs = new bool[5];
        arFaceBlendValues = new float[arHeadMesh.blendShapeCount];
    }

    private void Update()
    {
        // Sample inputs every frame and store them until they're sent. This prevents values from being skipped between fixedupdate calls
        //for (int i = 0; i < arHeadMesh.GetBlendShapeIndex.va; i++)
        //{

        //}
    }

    private void FixedUpdate()
    {
        SendInput();

        // Reset input booleans
        //for (int i = 0; i < inputs.Length; i++)
        //{
        //    inputs[i] = false;
        //}
    }




    #region Messages
    // TODO: This is where we will send the array of facial blend shapes as well as the heads position
    // and rotation relative to world origin
    private void SendInput()
    {
        Message message = Message.Create(MessageSendMode.Unreliable, ClientToServerId.PlayerInput);
        //message.AddBools(inputs, false);
        message.AddVector3(this.gameObject.transform.position);
        message.AddVector3(this.gameObject.transform.localEulerAngles);
        NetworkManager.Instance.Client.Send(message);
    }
    #endregion
}
