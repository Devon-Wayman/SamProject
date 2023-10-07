using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARKit;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARFace))]
public class BlendShapeVisualizer : MonoBehaviour
{
    [SerializeField] BlendShapeMappings blendShapeMappings;

    [SerializeField] SkinnedMeshRenderer skinnedMeshRenderer;

    private ARKitFaceSubsystem arKitFaceSubsystem;

    private Dictionary<ARKitBlendShapeLocation, int> faceArKitBlendShapeIndexMap = new Dictionary<ARKitBlendShapeLocation, int>();

    [SerializeField] ARFace face;

    private void Awake()
    {
        face = GetComponent<ARFace>();
        CreateFeatureBlendMapping();
    }

    private void CreateFeatureBlendMapping()
    {
        if (skinnedMeshRenderer == null || skinnedMeshRenderer.sharedMesh == null) return;

        if (blendShapeMappings.Mappings == null || blendShapeMappings.Mappings.Count == 0)
        {
            Debug.LogError("Mappings must be configured before using BlendShapeModifier");
            return;
        }

        foreach (Mapping mapping in blendShapeMappings.Mappings)
        {
            faceArKitBlendShapeIndexMap[mapping.location] = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(mapping.name);
        }
    }

    void SetVisible(bool visible)
    {
        if (skinnedMeshRenderer == null) return;
        skinnedMeshRenderer.enabled = visible;
    }

    void UpdateVisibility()
    {
        bool visible = enabled && (face.trackingState == TrackingState.Tracking) && (ARSession.state > ARSessionState.Ready);
        SetVisible(visible);
    }

    private void OnEnable()
    {
        ARFaceManager faceManager = FindObjectOfType<ARFaceManager>();

        if (faceManager != null)
        {
            Debug.Log("Found AR Face Manager!");
            arKitFaceSubsystem = (ARKitFaceSubsystem)faceManager.subsystem;
        }

        UpdateVisibility();

        face.updated += OnFaceUpdated;
        ARSession.stateChanged += OnSystemStateChanged;
    }

    private void OnDisable()
    {
        face.updated -= OnFaceUpdated;
        ARSession.stateChanged -= OnSystemStateChanged;
    }



    private void OnSystemStateChanged(ARSessionStateChangedEventArgs args)
    {
        UpdateVisibility();
    }

    private void OnFaceUpdated(ARFaceUpdatedEventArgs args)
    {
        UpdateVisibility();
        UpdateFaceFeatures();
    }

    private void UpdateFaceFeatures()
    {
        if (skinnedMeshRenderer == null || !skinnedMeshRenderer.enabled || skinnedMeshRenderer.sharedMesh == null) return;

        using (var blendShapes = arKitFaceSubsystem.GetBlendShapeCoefficients(face.trackableId, Unity.Collections.Allocator.Temp))
        {
            foreach(var featureCoefficient in blendShapes)
            {
                int mappedBlendShapeIndex;
                if (faceArKitBlendShapeIndexMap.TryGetValue(featureCoefficient.blendShapeLocation, out mappedBlendShapeIndex))
                {
                    if (mappedBlendShapeIndex >= 0)
                    {
                        skinnedMeshRenderer.SetBlendShapeWeight(mappedBlendShapeIndex, featureCoefficient.coefficient * blendShapeMappings.CoefficientScale);
                    }
                }
            }
        }
    }
}
