using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float speed;
    public bool crouched = false;
    float mass = 1f;
    Vector3 impactVector = Vector3.zero;
    float verticalVelocity = 0f;
    float negSinus;

    public CharacterController chrCntrl;
    Vector3 move = new Vector3(0.0f, 0.0f, 0.0f);

    Vector3 spd;
    float grav = 2* -9.81f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    bool grnd;
    bool grndPrev;

    float jmpH = 3.0f;


    public Animator movement;
    public Animator shooting;

    float timeStamp;
    float shootingCooldown = 0.7f;

    public AudioSource asrc;
    public AudioClip shoot;
    public AudioClip crunch;
    // Start is called before the first frame update
    void Start()
    {
        timeStamp = Time.time;
    }

    // Update is called once per frame
    void Update()
    {   
    	grndPrev = grnd;
        //ground check
        grnd = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        
        if (grndPrev == false && grnd == true) {
            impactVector = Vector3.zero;
        }
        

        //slow walk and movement
        if(Input.GetKey(KeyCode.LeftShift)){
            movement.SetBool("Slow", true);
            speed = 3.0f;
        }else{
            speed = 10.0f;
            movement.SetBool("Slow", false);
        }

        float ha = Input.GetAxis("Horizontal");
        float va = Input.GetAxis("Vertical");

        move = (transform.right * ha + transform.forward*va)/*.normalized*/ * speed * Time.deltaTime; // try normalising
        chrCntrl.Move(move);

        //jumping
        if(Input.GetButtonDown("Jump") && grnd){
            spd.y = Mathf.Sqrt(jmpH * -2 * grav);
        }

        //gravity
        if(grnd && spd.y < 0){
            if(spd.y < -20){
                asrc.PlayOneShot(crunch, 0.5f*GlobalTimer.sliderValue);
            }
            spd.y = -2.0f;
        }else {
            spd.y += grav * Time.deltaTime;
        }
        
        //Debug.Log("impVec= " + impactVector);
        /*if (impactVector[2] > 0) {
            impactVector[2] = impactVector[2] * (-1);
        }*/
        
        chrCntrl.Move((spd + impactVector) * Time.deltaTime);
        

        animateMotion(ha, va);

        if(Input.GetMouseButtonDown(0) && timeStamp <= Time.time){
            shooting.SetTrigger("Active");
            timeStamp = Time.time + shootingCooldown;
            asrc.PlayOneShot(shoot, 0.3f*GlobalTimer.sliderValue);
        }

        if(grnd){
            chrCntrl.stepOffset = 0.5f;
        }else{
            chrCntrl.stepOffset = 0.0f;
        }
    }

    private void animateMotion(float ha, float va){
        if(grnd){
            //hoja
            if(ha == 0 && va == 0){
                movement.SetFloat("Mode", 0.0f, 0.1f, Time.deltaTime);
            }else if(va < 0){
                movement.SetFloat("Mode", 0.33f, 0.1f, Time.deltaTime);
            }else{
                movement.SetFloat("Mode", 0.66f, 0.1f, Time.deltaTime);
            }
        }else{
            //skacemo oz letimo
            movement.SetFloat("Mode", 1.0f, 0.1f, Time.deltaTime);
        }
    }
    
    public void AddImpact(float impactForce, Vector3 direction){
        impactVector = Vector3.zero;
        verticalVelocity = impactForce;
        impactVector = direction;
        impactVector.Normalize();
        impactVector.y = 0f;
//           impactVector *= impactForce * Mathf.Sin(Vector3.Angle(transform.up, direction)) / (0.5f * mass); 
    	if (Mathf.Sin(Vector3.Angle(transform.up, direction)) > 0) {
    	   negSinus = -1 * Mathf.Sin(Vector3.Angle(transform.up, direction));
    	} 
    	else {
    	   negSinus = Mathf.Sin(Vector3.Angle(transform.up, direction));
    	}
        impactVector *= impactForce * negSinus / (0.5f * mass); 

        //Debug.Log("inside sinus = " + Mathf.Sin(Vector3.Angle(transform.up, direction)));
        spd.y = impactForce / mass;
        
    }
}
