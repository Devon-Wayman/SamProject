// AR_FOUNDATION_EDITOR_REMOTE: fix for Editor applied
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine.XR.ARSubsystems;
#if (UNITY_IOS || UNITY_EDITOR) && (ARKIT_FACE_TRACKING_INSTALLED || ARKIT_5_0_0_OR_NEWER)
using UnityEngine.XR.ARKit;
#endif

namespace UnityEngine.XR.ARFoundation.Samples
{
    #if UNITY_EDITOR && AR_FOUNDATION_REMOTE_INSTALLED
    using ARKitFaceSubsystem = ARFoundationRemote.Runtime.FaceSubsystem;
    #endif
    
    /// <summary>
    /// Populates the action unit coefficients for an <see cref="ARFace"/>.
    /// </summary>
    /// <remarks>
    /// If this <c>GameObject</c> has a <c>SkinnedMeshRenderer</c>,
    /// this component will generate the blend shape coefficients from the underlying <c>ARFace</c>.
    ///
    /// </remarks>
    [RequireComponent(typeof(ARFace))]
    public class OldManMapper : MonoBehaviour
    {
        [SerializeField] float m_CoefficientScale = 100.0f;

        public float coefficientScale
        {
            get { return m_CoefficientScale; }
            set { m_CoefficientScale = value; }
        }

        [SerializeField] SkinnedMeshRenderer m_SkinnedMeshRenderer;

        public SkinnedMeshRenderer skinnedMeshRenderer
        {
            get
            {
                return m_SkinnedMeshRenderer;
            }
            set
            {
                m_SkinnedMeshRenderer = value;
                CreateFeatureBlendMapping();
            }
        }

        #if (UNITY_IOS || UNITY_EDITOR) && (ARKIT_FACE_TRACKING_INSTALLED || ARKIT_5_0_0_OR_NEWER)
        ARKitFaceSubsystem m_ARKitFaceSubsystem;

        Dictionary<ARKitBlendShapeLocation, int> m_FaceArkitBlendShapeIndexMap;
        #endif

        [SerializeField] ARFace m_Face;

        void Awake()
        {
            //m_Face = GetComponent<ARFace>();
            CreateFeatureBlendMapping();
        }

        void CreateFeatureBlendMapping()
        {
            if (skinnedMeshRenderer == null || skinnedMeshRenderer.sharedMesh == null)
            {
                return;
            }
            
    #if (UNITY_IOS || UNITY_EDITOR) && (ARKIT_FACE_TRACKING_INSTALLED || ARKIT_5_0_0_OR_NEWER)
            const string strPrefix = "blendShape2.";
            m_FaceArkitBlendShapeIndexMap = new Dictionary<ARKitBlendShapeLocation, int>();

            m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.JawOpen        ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "AA");
            m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.BrowDownLeft] = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "BrowsD_L");
            m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.BrowDownRight] = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "BrowsD_R");
            m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.BrowOuterUpLeft] = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "BrowsU_L");
            m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.BrowOuterUpRight] = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "BrowsU_R");
            m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.CheekSquintLeft] = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "CheekSquint_L");
            m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.CheekSquintRight] = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "CheekSquint_R");
#endif
        }

        void SetVisible(bool visible)
        {
            if (skinnedMeshRenderer == null) return;

            skinnedMeshRenderer.enabled = visible;
        }

        void UpdateVisibility()
        {
            var visible = enabled && (m_Face.trackingState == TrackingState.Tracking) && (ARSession.state > ARSessionState.Ready);
            SetVisible(visible);
        }

        void OnEnable()
        {
    #if (UNITY_IOS || UNITY_EDITOR) && (ARKIT_FACE_TRACKING_INSTALLED || ARKIT_5_0_0_OR_NEWER)
            var faceManager = FindObjectOfType<ARFaceManager>();

            if (faceManager != null)
            {
                m_ARKitFaceSubsystem = (ARKitFaceSubsystem)faceManager.subsystem;
            }
    #endif
            UpdateVisibility();
            m_Face.updated += OnUpdated;
            ARSession.stateChanged += OnSystemStateChanged;
        }

        void OnDisable()
        {
            m_Face.updated -= OnUpdated;
            ARSession.stateChanged -= OnSystemStateChanged;
        }

        void OnSystemStateChanged(ARSessionStateChangedEventArgs eventArgs)
        {
            UpdateVisibility();
        }

        void OnUpdated(ARFaceUpdatedEventArgs eventArgs)
        {
            UpdateVisibility();
            UpdateFaceFeatures();
        }

        void UpdateFaceFeatures()
        {
            if (skinnedMeshRenderer == null || !skinnedMeshRenderer.enabled || skinnedMeshRenderer.sharedMesh == null)
            {
                return;
            }

    #if (UNITY_IOS || UNITY_EDITOR) && (ARKIT_FACE_TRACKING_INSTALLED || ARKIT_5_0_0_OR_NEWER)
            using (var blendShapes = m_ARKitFaceSubsystem.GetBlendShapeCoefficients(m_Face.trackableId, Allocator.Temp))
            {
                foreach (var featureCoefficient in blendShapes)
                {
                    int mappedBlendShapeIndex;
                    if (m_FaceArkitBlendShapeIndexMap.TryGetValue(featureCoefficient.blendShapeLocation, out mappedBlendShapeIndex))
                    {
                        if (mappedBlendShapeIndex >= 0)
                        {
                            skinnedMeshRenderer.SetBlendShapeWeight(mappedBlendShapeIndex, featureCoefficient.coefficient * coefficientScale);
                        }
                    }
                }
            }
    #endif
        }
    }
}