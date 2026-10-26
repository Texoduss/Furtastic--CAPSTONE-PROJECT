using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARSimDebug : MonoBehaviour
{
    void Update()
    {
        Debug.Log("ARSession.state = " + ARSession.state);
        Debug.Log("Camera pos: " + Camera.main.transform.position);
    }
}
