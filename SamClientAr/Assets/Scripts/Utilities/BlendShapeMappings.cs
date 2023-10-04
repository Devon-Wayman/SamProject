using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARKit;

[Serializable]
public class Mapping
{
    public ARKitBlendShapeLocation location;
    public string name;
}

[CreateAssetMenu(fileName = "BlendShapeMapping", menuName = "BlendShapeMappings/Mappings", order = 1)]
public class BlendShapeMappings : ScriptableObject
{
    public const string BLENDSHAPE_EXPRESSION = "blendshape2.";

    public float CoefficientScale = 100.0f;

    [SerializeField]
    public List<Mapping> Mappings = new List<Mapping>()
    {
        new Mapping {location = ARKitBlendShapeLocation.EyeBlinkLeft, name = $"{BLENDSHAPE_EXPRESSION}eyeBlink_L"},
        new Mapping {location = ARKitBlendShapeLocation.EyeBlinkRight, name = $"{BLENDSHAPE_EXPRESSION}eyeBlink_R"},
        new Mapping {location = ARKitBlendShapeLocation.EyeSquintLeft, name = $"{BLENDSHAPE_EXPRESSION}eyeSquint_L"},
        new Mapping {location = ARKitBlendShapeLocation.EyeSquintRight, name = $"{BLENDSHAPE_EXPRESSION}eyeSquint_R"},
        new Mapping {location = ARKitBlendShapeLocation.EyeLookDownLeft, name = $"{BLENDSHAPE_EXPRESSION}eyeD_L"},
        new Mapping {location = ARKitBlendShapeLocation.EyeLookDownRight, name = $"{BLENDSHAPE_EXPRESSION}eyeD_R"},
        new Mapping {location = ARKitBlendShapeLocation.EyeLookInLeft, name = $"{BLENDSHAPE_EXPRESSION}eyeLI_L"},
        new Mapping {location = ARKitBlendShapeLocation.EyeLookInRight, name = $"{BLENDSHAPE_EXPRESSION}eyeLI_R"},
        new Mapping {location = ARKitBlendShapeLocation.EyeWideLeft, name = $"{BLENDSHAPE_EXPRESSION}eyeW_L"},
        new Mapping {location = ARKitBlendShapeLocation.EyeWideRight, name = $"{BLENDSHAPE_EXPRESSION}eyeW_R"},
        new Mapping {location = ARKitBlendShapeLocation.EyeWideRight, name = $"{BLENDSHAPE_EXPRESSION}eyeW_R"},
        new Mapping {location = ARKitBlendShapeLocation.JawOpen, name = $"{BLENDSHAPE_EXPRESSION}jawOpen"},
        new Mapping {location = ARKitBlendShapeLocation.JawLeft, name = $"{BLENDSHAPE_EXPRESSION}jawLeft"},
        new Mapping {location = ARKitBlendShapeLocation.JawRight, name = $"{BLENDSHAPE_EXPRESSION}jawRight"},
        new Mapping {location = ARKitBlendShapeLocation.JawForward, name = $"{BLENDSHAPE_EXPRESSION}jawForward"},
        new Mapping {location = ARKitBlendShapeLocation.MouthSmileLeft, name = $"{BLENDSHAPE_EXPRESSION}smile_L"},
        new Mapping {location = ARKitBlendShapeLocation.MouthSmileRight, name = $"{BLENDSHAPE_EXPRESSION}smile_R"},
        new Mapping {location = ARKitBlendShapeLocation.MouthFrownLeft, name = $"{BLENDSHAPE_EXPRESSION}smileF_L"},
        new Mapping {location = ARKitBlendShapeLocation.MouthFrownRight, name = $"{BLENDSHAPE_EXPRESSION}smileF_R"},
        new Mapping {location = ARKitBlendShapeLocation.MouthLeft, name = $"{BLENDSHAPE_EXPRESSION}mouth_L"},
        new Mapping {location = ARKitBlendShapeLocation.MouthRight, name = $"{BLENDSHAPE_EXPRESSION}mouth_R"},
    };
}
