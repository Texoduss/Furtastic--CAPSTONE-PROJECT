using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARManager : MonoBehaviour
{
    
    private static ARManager instance;

    private void Awake() // This method is called when the script instance is being loaded
    {
        if (instance == null)
        {
            instance = this;
            
            
        }
        else if(instance != this)
        {
            Destroy(gameObject);
            
        }
        
    }
}
