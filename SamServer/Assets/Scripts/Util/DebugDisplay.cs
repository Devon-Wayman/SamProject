using Riptide.Demos.DedicatedServer;
using UnityEngine;

public class DebugDispla : MonoBehaviour
{
    [SerializeField] bool showDebug = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            showDebug = !showDebug;
        }
    }


    private void OnGUI()
    {
        if (!showDebug) return;

        GUI.Label(new Rect(10, 10, 300, 300), $"Clients connected: {NetworkManager.Instance.Server.ClientCount}\nLast position: {Player.latestPosition}\nLatest rotation: {Player.latestRotation}\nBlendshapes: {Player.TotalBlendShapes}");
    }
}
