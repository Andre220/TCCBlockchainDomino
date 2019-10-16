using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHelpers : MonoBehaviour
{
    public void DisableUI(GameObject gameObject) 
    {
        gameObject.SetActive(false);
    }

    public void EnableUI(GameObject gameObject)
    {
        gameObject.SetActive(true);
    }
}
