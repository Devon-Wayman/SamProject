using UnityEngine;

namespace Riptide.Demos.DedicatedClient
{
    public class PlayerController : MonoBehaviour
    {
        // TODO: Replace code here with the values and functions we'd want to send to the
        // SAM server to update the head rotation, position, blendshapes, etc



        [SerializeField] private Transform camTransform;
        private bool[] inputs;

        private void Start()
        {
            inputs = new bool[5];
        }

        private void Update()
        {
            // Sample inputs every frame and store them until they're sent. This ensures no inputs are missed because they happened between FixedUpdate calls
            if (Input.GetKey(KeyCode.W))
                inputs[0] = true;

            if (Input.GetKey(KeyCode.S))
                inputs[1] = true;

            if (Input.GetKey(KeyCode.A))
                inputs[2] = true;

            if (Input.GetKey(KeyCode.D))
                inputs[3] = true;

            if (Input.GetKey(KeyCode.Space))
                inputs[4] = true;
        }

        private void FixedUpdate()
        {
            SendInput();

            // Reset input booleans
            for (int i = 0; i < inputs.Length; i++)
            {
                inputs[i] = false;
            }
        }




        #region Messages
        // TODO: This is where we will send the array of facial blend shapes as well as the heads position
        // and rotation relative to world origin
        private void SendInput()
        {
            Message message = Message.Create(MessageSendMode.Unreliable, ClientToServerId.PlayerInput);
            message.AddBools(inputs, false);
            message.AddVector3(camTransform.forward);
            NetworkManager.Instance.Client.Send(message);
        }
        #endregion
    }
}
