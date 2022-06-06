using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.PlayerLoop;

namespace Unity.LiveCapture
{
    /// <summary>
    /// An empty struct that defines the custom player loop update used for synchronization.
    /// </summary>
    /// <remarks>
    /// The synchronizer update takes place immediately following Update, before
    /// the animation system update.
    /// </remarks>
    struct SynchronizerUpdate
    {
    }

    /// <summary>
    /// A component used to synchronize playback of content from <see cref="ITimedDataSource"/> instances.
    /// </summary>
    [ExecuteAlways]
    [AddComponentMenu("Live Capture/Timecode Synchronizer")]
    [HelpURL(Documentation.baseURL + "ref-component-timecode-synchronizer" + Documentation.endURL)]
    public class SynchronizerComponent : MonoBehaviour
    {
        [SerializeField]
        bool m_DisplayTimecode;

        [SerializeField]
        Synchronizer m_Impl = new Synchronizer();

        Coroutine m_CalibrationCoro;

        /// <summary>
        /// The synchronizer instance.
        /// </summary>
        internal Synchronizer Impl => m_Impl;

        /// <summary>
        /// The synchronizer instance.
        /// </summary>
        public ISynchronizer Synchronizer => m_Impl;

#if UNITY_EDITOR
        PlayableGraph m_DummyPlayableGraph;
#endif

        void OnEnable()
        {
            // Do the update prior to the animation update in PreLateUpdate. This is necessary to avoid adding a frame of
            // latency with live capture devices that drive the actors via the playables graph.
            PlayerLoopExtensions.RegisterUpdate<PreLateUpdate, SynchronizerUpdate>(OnSynchronizerUpdate, 0);

#if UNITY_EDITOR
            // Playing a PlayableGraph tricks the Editor into keeping the update loop
            // alive.
            m_DummyPlayableGraph = PlayableGraph.Create("Synchronizer Editor Update");
            ScriptPlayableOutput.Create(m_DummyPlayableGraph, "output");
            m_DummyPlayableGraph.Play();
#endif
        }

        void OnDisable()
        {
            StopCalibration();
            Impl.Pause();
            PlayerLoopExtensions.DeregisterUpdate<SynchronizerUpdate>(OnSynchronizerUpdate);
#if UNITY_EDITOR
            if (m_DummyPlayableGraph.IsValid())
            {
                m_DummyPlayableGraph.Destroy();
            }
#endif
        }

        void OnSynchronizerUpdate()
        {
            Impl.Update();
        }

        void OnGUI()
        {
            if (m_DisplayTimecode)
            {
                GUI.Label(new Rect(10, 10, 100, 20), Impl.CurrentTimecode.ToString());
            }
        }

        /// <summary>
        /// Starts calibration of the synchronized group.
        /// </summary>
        /// <remarks>
        /// Calibration is used to automatically determine optimal delays and buffer sizes for each data
        /// source in the synchronized group so they will have valid data to present for each synchronized frame.
        /// </remarks>
        /// <param name="calibrator">The calibrator to use. If <see langword="null"/>, the default calibrator is used.</param>
        public void StartCalibration(ISynchronizationCalibrator calibrator = null)
        {
            if (!isActiveAndEnabled) return;

            if (calibrator == null)
            {
                calibrator = new DefaultSyncCalibrator();
            }

            m_CalibrationCoro = StartCoroutine(Impl.CalibrationWith(calibrator));
        }

        /// <summary>
        /// Cancels the synchronizer calibration if it is currently in progress.
        /// </summary>
        public void StopCalibration()
        {
            if (m_CalibrationCoro != null)
            {
                StopCoroutine(m_CalibrationCoro);
                m_CalibrationCoro = null;
            }
        }
    }
}
