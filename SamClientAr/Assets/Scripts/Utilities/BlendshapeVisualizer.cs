using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARKit;

[RequireComponent(typeof(ARFace))]
public class BlendshapeVisualizer : MonoBehaviour
{
    [SerializeField] private BlendShapeMappings blendShapeMappings;

    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;

    private ARKitFaceSubsystem arKitFaceSubsystem;

    private Dictionary<ARKitBlendShapeLocation, int> faceArkitBlendShapeIndexMap = new Dictionary<ARKitBlendShapeLocation, int>();

    [SerializeField] private ARFace face;

    private void Awake()
    {
        CreateFeatureBlendMapping();
    }

    private void CreateFeatureBlendMapping()
    {
        if (skinnedMeshRenderer == null || skinnedMeshRenderer.sharedMesh == null)
        {
            return;
        }

        if (blendShapeMappings.Mappings == null || blendShapeMappings.Mappings.Count == 0) return;

        foreach(Mapping mapping in blendShapeMappings.Mappings)
        {
            faceArkitBlendShapeIndexMap[mapping.location] = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(mapping.name);
        }
    }

    void SetVisible(bool visible)
    {
        if (skinnedMeshRenderer == null) return;
        skinnedMeshRenderer.enabled = visible;
    }

    void UpdateVisibility()
    {
        var visible = enabled && (face.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking) && (ARSession.state > ARSessionState.Ready);
        SetVisible(visible);
    }

    private void OnEnable()
    {
        var faceManager = FindObjectOfType<ARFaceManager>();

        if (faceManager != null)
        {
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

    void OnSystemStateChanged(ARSessionStateChangedEventArgs eventArgs)
    {
        UpdateVisibility();
    }

    void OnFaceUpdated(ARFaceUpdatedEventArgs eventArgs)
    {
        UpdateVisibility();
        UpdateFaceFeatures();
    }

    private void UpdateFaceFeatures()
    {
        if (skinnedMeshRenderer == null || !skinnedMeshRenderer.enabled || skinnedMeshRenderer.sharedMesh == null)
        {
            return;
        }

        using (var blendShapes = arKitFaceSubsystem.GetBlendShapeCoefficients(face.trackableId, Unity.Collections.Allocator.Temp))
        {
            foreach (var featureCoefficient in blendShapes)
            {
                int mappedBlendshapeIndex;

                if (faceArkitBlendShapeIndexMap.TryGetValue(featureCoefficient.blendShapeLocation, out mappedBlendshapeIndex))
                {
                    if (mappedBlendshapeIndex >= 0)
                    {
                        skinnedMeshRenderer.SetBlendShapeWeight(mappedBlendshapeIndex, featureCoefficient.coefficient * blendShapeMappings.CoefficientScale);
                    }
                }
            }
        }
    }




}
