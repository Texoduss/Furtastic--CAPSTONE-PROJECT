using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARHealthCheck : MonoBehaviour
{
    void Update()
    {
        Debug.Log("ARSession.state = " + ARSession.state);
    }
}
