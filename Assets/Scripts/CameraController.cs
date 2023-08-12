using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private bool controlEnable = true;

    private float defaultSize;
    private float currSize;

    private Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = gameObject.GetComponent<Camera>();
        currSize = cam.orthographicSize;
        defaultSize = currSize;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(controlEnable)
            MoveCamera();
    }

    void MoveCamera()
    {
        if (Input.GetMouseButtonDown(1))
        {
                // move the camera around
        }

        if (Input.mouseScrollDelta.y > 0)
        {
            // increase camera size
            cam.orthographicSize -= currSize * 0.1f;
            currSize = cam.orthographicSize;
        }

        if (Input.mouseScrollDelta.y < 0)
        {
            // decrease camera size
            cam.orthographicSize += currSize * 0.1f;
            currSize = cam.orthographicSize;
        }

        if (Input.GetKey(KeyCode.UpArrow))
            transform.position = transform.position + Vector3.up * 10f * Time.deltaTime;

        if (Input.GetKey(KeyCode.DownArrow))
            transform.position = transform.position + Vector3.down * 10f * Time.deltaTime;

        if (Input.GetKey(KeyCode.LeftArrow))
            transform.position = transform.position + Vector3.left * 10f * Time.deltaTime;

        if (Input.GetKey(KeyCode.RightArrow))
            transform.position = transform.position + Vector3.right * 10f * Time.deltaTime;
    }

    public void ResetSize()
    {
        cam.orthographicSize = defaultSize;
        currSize = cam.orthographicSize;
    }
}
