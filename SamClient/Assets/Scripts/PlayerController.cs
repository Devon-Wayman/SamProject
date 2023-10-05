﻿using UnityEngine;

namespace Riptide.Demos.DedicatedClient
{
    /// <summary>
    /// Responsible for sending server our latest transform data and blendshape values
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] SkinnedMeshRenderer skinnedFaceMesh = null;
        [SerializeField] float[] latestBlendValues;

        int totalBlendshapes = 0;

        private void Start()
        {
            if (skinnedFaceMesh == null)
            {
                latestBlendValues = new float[0];
            }
            else
            {
                totalBlendshapes = skinnedFaceMesh.sharedMesh.blendShapeCount;
                latestBlendValues = new float[totalBlendshapes];
            }
            
            Debug.Log($"{totalBlendshapes} found on mesh. Setting float array length");
        }

        private void Update()
        {
            if (skinnedFaceMesh == null) return;

            // Sample current blendshape values
            for (int i = 0; i < totalBlendshapes; i++)
            {
                latestBlendValues[i] = skinnedFaceMesh.GetBlendShapeWeight(i);
            }
        }

        private void FixedUpdate()
        {
            SendFaceUpdate();
        }

        #region Messages
        private void SendFaceUpdate()
        {
            Message message = Message.Create(MessageSendMode.Unreliable, ClientToServerId.FaceUpdate);
            message.AddVector3(gameObject.transform.position);
            message.AddVector3(gameObject.transform.eulerAngles); 
            message.AddFloats(latestBlendValues);
            NetworkManager.Singleton.Client.Send(message);
        }
        #endregion
    }
}