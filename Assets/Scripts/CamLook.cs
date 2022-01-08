using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamLook : MonoBehaviour
{

    public GameObject rockt;
    public GameObject shootPoint;
    float timeStamp;
    float shootingCooldown = 1.0f;
    
    private float yaw = 0.0f;
    private float pitch = 0.0f;
    [Range(0, 1000)] public float sensitivity = 300.0f;
    public Transform plyr;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        timeStamp = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        yaw = (sensitivity * Input.GetAxis("Mouse X") * Time.deltaTime);
        
        pitch -= (sensitivity * Input.GetAxis("Mouse Y") * Time.deltaTime);
        pitch = Mathf.Clamp(pitch, -90, 90);

        transform.localRotation = Quaternion.Euler(pitch, 0.0f, 0.0f);
        plyr.Rotate(new Vector3(0.0f, yaw, 0.0f));
        
        //raketa
        if (Input.GetKeyDown(KeyCode.Mouse0) && timeStamp <= Time.time) {
       	Shoot();
       	timeStamp = Time.time + shootingCooldown;
      	}

    }
    
    void Shoot()
    {
    	RaycastHit hit;
    	
    	Quaternion fireRotation = Quaternion.LookRotation(transform.forward);
    	
    	
    	if (Physics.Raycast(transform.position, fireRotation * Vector3.forward, out hit, Mathf.Infinity))
    	{
    		GameObject tempBullet = Instantiate(rockt, shootPoint.transform.position, fireRotation);
    		tempBullet.GetComponent<rocket>().hitPoint = hit.point;
    	}else{
            GameObject tempBullet = Instantiate(rockt, shootPoint.transform.position, fireRotation);
            tempBullet.GetComponent<rocket>().hitPoint = shootPoint.transform.position + (fireRotation * Vector3.forward)*100;
        }
    }
}
