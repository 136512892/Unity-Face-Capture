using UnityEngine;
using Unity.LiveCapture.CompanionApp;

public class LiveCaptureExample : MonoBehaviour
{
    LiveCaptureServer server;
    private void Awake()
    {
        server = Resources.Load<LiveCaptureServer>("Live Capture Server");
        server.Init();
        server.StartServer();
    }
    private void Update()
    {
        server.OnUpdate();
    }
    private void OnDestroy()
    {
        server.StopServer();
    }
}