using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class hideInEditor : MonoBehaviour
{
      
    public GameObject[] hide;
  
    void Update()
    {
        foreach(GameObject go in hide) {
  
        if (Application.isPlaying)
            go.SetActive(true);
        else
            go.SetActive(false);
        }
    }
}
