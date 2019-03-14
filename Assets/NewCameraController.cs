using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCameraController : MonoBehaviour
{
    public int border = 10;
    public int sensivityXY = 10;
    public int sensivityZoom = 30;
    public float sensivityMidMouse = 1;

    public float minX = -5;
    public float minY = -5;
    public float minZ = -5;
    public float maxX = 5;
    public float maxY = 5;
    public float maxZ = 5;

    public Vector2 currentPosition;
    public Vector2 deltaPosition;
    public Vector3 newDeltaPosition;
    public Vector2 lastPosition;
    // Start is called before the first frame update
    void Start()
    {
        minX += transform.position.x;
        minY += transform.position.y;
        minZ += transform.position.z;
        maxX += transform.position.x;
        maxY += transform.position.y;
        maxZ += transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        // правый край
        if (Input.mousePosition.x >= Screen.width - border)
        {
            // Move the camera
            transform.position += transform.right * Time.deltaTime * sensivityXY;
        }
        // левый край
        if (Input.mousePosition.x <= border)
        {
            // Move the camera
            transform.position -= transform.right * Time.deltaTime * sensivityXY;
        }
        // Верх
        if (Input.mousePosition.y >= Screen.height - border)
        {
            // Move the camera
            transform.position += transform.up * Time.deltaTime * sensivityXY;
        }
        // низ
        if (Input.mousePosition.y <= border)
        {
            // Move the camera
            transform.position -= transform.up * Time.deltaTime * sensivityXY;
        }
        // приближение и удаление
        float mouseWheel = Input.GetAxis("Mouse ScrollWheel");
        transform.position += transform.forward * Time.deltaTime * sensivityZoom * mouseWheel;

        currentPosition = Input.mousePosition;
        deltaPosition = currentPosition - lastPosition;
        lastPosition = currentPosition;

        if (Input.GetMouseButton(2))
        {
            newDeltaPosition = new Vector3(deltaPosition.x, deltaPosition.y, 0);
            transform.position -= newDeltaPosition * Time.deltaTime * sensivityMidMouse;
        }

        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        float clampedY = Mathf.Clamp(transform.position.y, minY, maxY);
        float clampedZ = Mathf.Clamp(transform.position.z, minZ, maxZ);
        transform.position = new Vector3(clampedX, clampedY, clampedZ);
    }
}
