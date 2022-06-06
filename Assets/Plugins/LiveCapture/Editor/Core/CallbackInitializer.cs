using UnityEngine.Playables;
using UnityEditor;
using UnityEditor.Timeline;

namespace Unity.LiveCapture.Editor
{
    [InitializeOnLoad]
    static class CallbackInitializer
    {
        static CallbackInitializer()
        {
            EditorApplication.update += OnUpdate;
            Callbacks.SeekOccurred += SeekOccurred;
        }

        static void OnUpdate()
        {
            UpdateServers();
        }

        static void UpdateServers()
        {
            foreach (var server in ServerManager.Instance.Servers)
            {
                server.OnUpdate();
            }
        }

        static void SeekOccurred(ISlate slate, PlayableDirector director)
        {
            if (TimelineEditor.inspectedDirector == director)
            {
                TimelineEditor.Refresh(RefreshReason.WindowNeedsRedraw);
            }
        }
    }
}
