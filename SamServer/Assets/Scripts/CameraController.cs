using UnityEngine;

/// <summary>
/// Use for repositioning camera. Does not allow for rotation change, only translation on all three axis
/// </summary>
public class CameraController : MonoBehaviour
{
    [SerializeField] Vector3 startPosition;

    private void Start()
    {
        startPosition = gameObject.transform.position;
    }

    [SerializeField] float latestMouseX;
    [SerializeField] float latestMouseY;
    private void Update()
    {
        latestMouseX = Input.GetAxis("Mouse X");
        latestMouseY = Input.GetAxis("Mouse Y");

        Vector3 newCamPosition = gameObject.transform.localPosition;
        newCamPosition.y += latestMouseY;
        newCamPosition.x += latestMouseX;

        transform.localPosition = new Vector3(newCamPosition.x,newCamPosition.y, transform.localPosition.z);

    }
}
