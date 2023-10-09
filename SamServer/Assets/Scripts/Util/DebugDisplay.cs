using TMPro;
using UnityEngine;
using System.Net;
using System.Collections.Generic;
using System;
using SamServer.Networking;
using Riptide;

public class DebugDisplay : DevSingleton<DebugDisplay>
{
    bool showDebug = false;
    [SerializeField] GameObject debugPanel = null;
    [SerializeField] TMP_Text debugText = null;

    List<string> serverIps = new List<string>();

    [Header("Fade Viewer")]
    [SerializeField] CanvasGroup fadeCanvas = null;

    private void Start()
    {
        fadeCanvas.alpha = 1f;
        debugPanel.SetActive(showDebug);
        GetLocalAddresses();
        UpdateCursorLock();
    }

    private void GetLocalAddresses()
    {
        IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork) serverIps.Add(ip.ToString());
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            showDebug = !showDebug;

            UpdateCursorLock();

            debugPanel.SetActive(showDebug);
        }

        if (showDebug) UpdateDebugText();
    }

    private void UpdateCursorLock()
    {
        if (showDebug)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        } else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void UpdateDebugText()
    {
        if (NetworkManager.Instance.Server.ClientCount == 0){
            debugText.text = $"Server running: {NetworkManager.Instance.Server.IsRunning}\nServer port: {NetworkManager.Instance.Server.Port}\nServer TOT: {NetworkManager.Instance.Server.TimeoutTime}\nServer Addresses:\n{String.Join("\n  ", serverIps)}\n\nClient connected: False";
        } else
        {
            debugText.text = $"Server running: {NetworkManager.Instance.Server.IsRunning}\nServer port: {NetworkManager.Instance.Server.Port}\nServer TOT: {NetworkManager.Instance.Server.TimeoutTime}\n\nClient connected: True\nClient Not Connected: {NetworkManager.Instance.Server.Clients[0].IsNotConnected}\nClient head position: {Player.latestHeadPosition}\nClient head rotation: {Player.latestHeadRotation}\nClient RTT: {NetworkManager.Instance.Server.Clients[0].RTT}ms";
        }
    }


    public void ChangeFadeCanvasAlpha(float requestedAlpha)
    {
        LeanTween.alphaCanvas(fadeCanvas, requestedAlpha, 1f);
    }

    public void ChangeFadeCanvasAlpha(float requestedAlpha, ServerDisconnectedEventArgs e)
    {
        LeanTween.alphaCanvas(fadeCanvas, requestedAlpha, 1f).setOnComplete(delegate () {
            Destroy(Player.List[e.Client.Id].gameObject);
        });
    }
}
