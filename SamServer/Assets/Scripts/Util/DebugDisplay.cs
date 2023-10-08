using TMPro;
using UnityEngine;
using Riptide.Demos.DedicatedServer;

public class DebugDispla : MonoBehaviour
{
    [SerializeField] bool showDebug = false;
    [SerializeField] GameObject debugPanel = null;
    [SerializeField] TMP_Text debugText = null;

    private void Start()
    {
        debugPanel.SetActive(showDebug);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            showDebug = !showDebug;
            debugPanel.SetActive(showDebug);
        }

        if (showDebug) UpdateDebugText();
    }

    private void UpdateDebugText()
    {
        if (NetworkManager.Instance.Server.ClientCount == 0){
            debugText.text = $"Server running: {NetworkManager.Instance.Server.IsRunning}\nServer port: {NetworkManager.Instance.Server.Port}\nClient connected: False\nServer TOT: {NetworkManager.Instance.Server.TimeoutTime}";
        } else
        {
            debugText.text = $"Server running: {NetworkManager.Instance.Server.IsRunning}\nServer port: {NetworkManager.Instance.Server.Port}\nClient connected: True\nServer TOT: {NetworkManager.Instance.Server.TimeoutTime}\n\nClient head position: {Player.latestHeadPosition}\nClient head rotation: {Player.latestHeadRotation}\nClient RTT: {NetworkManager.Instance.Server.Clients[0].RTT}ms";
        }
    }
}
