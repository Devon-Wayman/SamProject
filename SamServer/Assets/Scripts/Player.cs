using UnityEngine;

namespace Riptide.Demos.DedicatedServer
{
    public class Player : DevSingleton<Player>
    {
        [SerializeField] SkinnedMeshRenderer skinnedMeshRenderer = null;

        static float[] receivedBlendWeights { get; set; }


        static int TotalBlendShapes = 0;
        static Vector3 receivedPosition = Vector3.zero;
        static Vector3 receivedEulerAngle = Vector3.zero;


        private void Start()
        {
            if (skinnedMeshRenderer == null) return;

            TotalBlendShapes = skinnedMeshRenderer.sharedMesh.blendShapeCount;
            receivedBlendWeights = new float[TotalBlendShapes];
        }

        private void Update()
        {
            if (skinnedMeshRenderer == null) return;

            for (int i = 0; i < TotalBlendShapes; i++)
            {
                skinnedMeshRenderer.SetBlendShapeWeight(i, receivedBlendWeights[i]);
            }
        }

        private void FixedUpdate()
        {
            gameObject.transform.position = receivedPosition;
            gameObject.transform.eulerAngles = receivedEulerAngle;
        }


        /// <summary>
        /// Reads the incoming transform and blendshape data from the connected client
        /// </summary>
        /// <param name="message"></param>
        [MessageHandler((ushort)ClientToServerId.PlayerInput)]
        private static void PlayerInput(Message message)
        {
            receivedPosition = message.GetVector3();
            receivedEulerAngle = message.GetVector3();
            message.GetFloats(TotalBlendShapes, receivedBlendWeights);

            Debug.Log($"Recent position: {receivedPosition}   Recent rotation: {receivedEulerAngle}");
        }
    }
}
