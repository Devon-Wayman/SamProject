using Riptide;
using SamClient.Networking;
using UnityEngine;

namespace SamClient.PlayerAssets
{
    /// <summary>
    /// Responsible for sending server our latest transform data and blendshape values
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] SkinnedMeshRenderer skinnedFaceMesh = null;
        float[] latestBlendValues;

        int totalBlendshapes = 0;

        private void Start()
        {
            totalBlendshapes = skinnedFaceMesh.sharedMesh.blendShapeCount;
            latestBlendValues = new float[totalBlendshapes];

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log($"{totalBlendshapes} found on mesh. Setting float array length");
#endif
        }

        private void Update()
        {
            if (skinnedFaceMesh == null) return;

            for (int i = 0; i < totalBlendshapes; i++)
            {
                latestBlendValues[i] = skinnedFaceMesh.GetBlendShapeWeight(i);
            }
        }

        private void FixedUpdate()
        {
            if (!NetworkManager.Instance.Client.IsConnected) return;
            SendFaceUpdate();
        }

        #region Messages
        private void SendFaceUpdate()
        {
            Message message = Message.Create(MessageSendMode.Unreliable, ClientToServerId.FaceUpdate);
            message.AddVector3(gameObject.transform.position);
            message.AddVector3(gameObject.transform.eulerAngles); 
            message.AddFloats(latestBlendValues);
            NetworkManager.Instance.Client.Send(message);
        }
        #endregion
    }
}
