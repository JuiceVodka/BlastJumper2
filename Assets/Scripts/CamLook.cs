using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamLook : MonoBehaviour
{

    private float yaw = 0.0f;
    private float pitch = 0.0f;
    [Range(0, 1000)] public float sensitivity = 300.0f;
    public Transform plyr;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        yaw = (sensitivity * Input.GetAxis("Mouse X") * Time.deltaTime);
        
        pitch -= (sensitivity * Input.GetAxis("Mouse Y") * Time.deltaTime);
        pitch = Mathf.Clamp(pitch, -90, 90);

        transform.localRotation = Quaternion.Euler(pitch, 0.0f, 0.0f);
        plyr.Rotate(new Vector3(0.0f, yaw, 0.0f));

    }
}
