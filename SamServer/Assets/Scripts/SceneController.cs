using UnityEngine;




public enum SpotColors
{
    RED,
    BLUE,
    GREEN,
    WHITE
}


/// <summary>
/// Handles incoming commands from client device (At this time expecting ipad with keyboard to work
/// so we can just use a standard keyboard button press for different functions)
/// </summary>
public class SceneController : MonoBehaviour
{
    [Header("Spotlights")]
    [SerializeField] GameObject spotlightParent = null;
    Light[] spotlights = null;
    [SerializeField] SpotColors sessionStartColor = SpotColors.BLUE;

    Color[] availableColors = new Color[] { Color.red, Color.blue, Color.green, Color.white };

    private void Awake()
    {
        spotlights = spotlightParent.GetComponentsInChildren<Light>();

        // Startin color set to blue for testing
        foreach (var light in spotlights)
        {
            light.color = availableColors[(int)sessionStartColor];
        }
    }


    /// <summary>
    /// Switch spotlight colors without fade transition
    /// </summary>
    /// <param name="requestedColor">Color to switch spotlights to</param>
    public void SwitchSpotColor(Color requestedColor)
    {

    }

    /// <summary>
    /// Fade spotlight colors between two values with a fade transition
    /// </summary>
    /// <param name="requestedColor">Color to fade spotlights to</param>
    public void FadeSpotColor(Color requestedColor)
    {

    }
}
