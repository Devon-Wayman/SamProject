using Riptide;
using SamClient.Networking;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

/// <summary>
/// This class is responsible for spawning eyes that follow a users gaze during an active
/// AR Face tracking session. This class is independent and does not rely on others to send
/// face blenshape and transform information. A different message type will be sent via Riptide
/// so if the model does not have eye data to send, no errors are thrown from inconsistent data exceptions.
///
/// Much like the face, the eye instances will have their transforms sampled each update call and sent out on
/// FixedUpdate to reduce network usage. Will need to see about possibly lerping the values on server side
/// to smooth out rotation and position changes between packets received
/// </summary>
[RequireComponent(typeof(ARFace))]
public class AREyeManager : MonoBehaviour
{
    [SerializeField] GameObject leftEyePrefab;
    [SerializeField] GameObject rightEyePrefab;

    [SerializeField] ARFace arFace;

    [Header("These should be set once eye prefab is instantiated")]
    [SerializeField] private GameObject leftEye;
    [SerializeField] private GameObject rightEye;

    [Header("Determines eye update parameters")]
    [SerializeField]  bool copyRotationFromEyes = false;
    [SerializeField] float eyeUpdateFrequency = 0.05f;
    private float eyeUpdateTimer = 0;

    void ApplyCopyEyeRotation()
    {
        eyeUpdateTimer += Time.deltaTime;

        if (eyeUpdateTimer < eyeUpdateFrequency) return;

        // reset the timer
        eyeUpdateTimer = 0;

        // Check if copyRotationFromEyes is disabled. If so, return and dont update users eye rotation nor
        // execute a network message
        if (!copyRotationFromEyes)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log("Copy rotation from eyes is disabled. Skipping rotation update and packet send");
#endif
            return;
        }

        if (leftEye != null && rightEye != null)
        {
            leftEye.transform.rotation = arFace?.leftEye?.rotation ?? Quaternion.identity;
            rightEye.transform.rotation = arFace?.rightEye?.rotation ?? Quaternion.identity;

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log($"Eye rots {leftEye.transform.rotation}   {rightEye.transform.rotation}");
#endif
        } else
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log("Left/right eye instances are null. Skipping rotation update and packet send");
#endif
        }
    }


    void Start()
    {
        ARFaceManager arFaceManager = FindObjectOfType<ARFaceManager>();

        if (arFaceManager != null && arFaceManager.subsystem.subsystemDescriptor.supportsEyeTracking)
        {
            arFace.updated += OnArFaceUpdate;
        }
    }

    private void Update() => ApplyCopyEyeRotation();

    private void FixedUpdate()
    {
        // Dont bother trying to send a packet to the server if client is not connected
        if (!NetworkManager.Singleton.Client.IsConnected) return;

        if (leftEye == null && rightEye == null) return;

        SendEyeUpdate();
    }

    private void SendEyeUpdate()
    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        Debug.Log("Sending eye update");
#endif

        Message message = Message.Create(MessageSendMode.Unreliable, ClientToServerId.EyeUpdate);
        message.AddVector3(leftEye.transform.eulerAngles);
        message.AddVector3(rightEye.transform.eulerAngles);
        NetworkManager.Singleton.Client.Send(message);
    }

    private void OnArFaceUpdate(ARFaceUpdatedEventArgs args)
    {
        if (arFace.leftEye != null && leftEye == null)
        {
            leftEye = Instantiate(leftEyePrefab, arFace.leftEye);
            leftEye.name = "LeftEye";
            leftEye.SetActive(false);
        }

        if (arFace.rightEye != null && rightEye == null)
        {
            rightEye = Instantiate(rightEyePrefab, arFace.rightEye);
            rightEye.name = "RightEye";
            rightEye.SetActive(false);
        }


        // Show the eyes if they are available
        if (arFace.trackingState == TrackingState.Tracking && ARSession.state > ARSessionState.Ready)
        {
            if (leftEye != null) leftEye.SetActive(true);
            
            if (rightEye != null) rightEye.SetActive(true);
        }
    }

    private void OnDisable()
    {
        arFace.updated -= OnArFaceUpdate;
        leftEye.SetActive(false);
        rightEye.SetActive(false);
    }
}
