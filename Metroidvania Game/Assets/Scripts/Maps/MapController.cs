using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public static MapController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GameObject[] maps;

    public GameObject fullMapCam;

    void Start()
    {
        foreach (GameObject map in maps)
        {
            if (PlayerPrefs.GetInt("Map_" + map.name) == 1)
            {
                map.SetActive(true);
            }
        }
        
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (!UIController.instance.fullScreenMap.activeInHierarchy)
            {
                UIController.instance.fullScreenMap.SetActive(true);
                Time.timeScale = 0f;
                fullMapCam.SetActive(true);
            }
            else
            {
                UIController.instance.fullScreenMap.SetActive(false);
                Time.timeScale = 1f;
                fullMapCam.SetActive(false);
            }
        }
    }

    public void ActivateMap(string mapToActivate)
    {
        foreach (GameObject map in maps)
        {
            if (map.name == mapToActivate)
            {
                map.SetActive(true);
                PlayerPrefs.SetInt("Map_" + mapToActivate, 1);
            }
        }
    }

}
