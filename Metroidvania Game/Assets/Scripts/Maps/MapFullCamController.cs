using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapFullCamController : MonoBehaviour
{
    public MapCameraController mapCam;

    public float zoomSpeed;
    private float startSize;
    public float maxZoom, minZoom;

    public float moveModifier = 1f;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();

        startSize = cam.orthographicSize;
    }


    void Update()
    {
        transform.position += new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0f).normalized * cam.orthographicSize * Time.unscaledDeltaTime * moveModifier;


        if (Input.GetKey(KeyCode.E))
        {
            cam.orthographicSize -= zoomSpeed * Time.unscaledDeltaTime;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            cam.orthographicSize += zoomSpeed * Time.unscaledDeltaTime;
        }
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
    }

    private void OnEnable()
    {
        transform.position = mapCam.transform.position;
    }

}
