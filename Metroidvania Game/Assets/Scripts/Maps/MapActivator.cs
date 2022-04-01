using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapActivator : MonoBehaviour
{
    public string mapToActivate;


    void Start()
    {
        MapController.instance.ActivateMap(mapToActivate);
    }


}
