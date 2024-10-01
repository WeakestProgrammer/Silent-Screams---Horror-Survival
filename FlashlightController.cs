using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    public Camera playerCamera;
    public float offsetDistance = 0.5f;
    public Vector3 positionOffset = new Vector3(0.5f, -0.2f, 0);
    public Vector3 rotationOffset = new Vector3(0, 0, 0);

    private void Start()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

        // Ensure the flashlight is a child of the camera
        transform.SetParent(playerCamera.transform);
    }

    private void Update()
    {
        // Update position
        Vector3 desiredPosition = playerCamera.transform.position +
                                  playerCamera.transform.right * positionOffset.x +
                                  playerCamera.transform.up * positionOffset.y +
                                  playerCamera.transform.forward * (offsetDistance + positionOffset.z);
        transform.position = desiredPosition;

        // Update rotation
        transform.rotation = playerCamera.transform.rotation * Quaternion.Euler(rotationOffset);
    }
}
