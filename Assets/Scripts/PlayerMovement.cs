using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float speed;

    public CharacterController chrCntrl;
    Vector3 move = new Vector3(0.0f, 0.0f, 0.0f);

    Vector3 spd;
    float grav = 2* -9.81f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    bool grnd;

    float jmpH = 3.0f;


    public Animator movement;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        //ground check
        grnd = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        //slow walk and movement
        if(Input.GetKey(KeyCode.LeftShift)){
            speed = 3.0f;
        }else{
            speed = 10.0f;
        }

        float ha = Input.GetAxis("Horizontal");
        float va = Input.GetAxis("Vertical");

        move = (transform.right * ha + transform.forward*va)/*.normalized*/ * speed * Time.deltaTime; // try normalising
        chrCntrl.Move(move);

        //jumping
        if(Input.GetButtonDown("Jump") && grnd){
            Debug.Log("skacemo");
            spd.y = Mathf.Sqrt(jmpH * -2 * grav);
        }

        //gravity
        if(grnd && spd.y < 0){
            spd.y = -2.0f;
        }else {
            spd.y += grav * Time.deltaTime;
        }
        chrCntrl.Move(spd * Time.deltaTime);

        animateMotion(ha, va);
    }

    private void animateMotion(float ha, float va){
        if(grnd){
            //hoja
            if(ha == 0 && va == 0){
                movement.SetFloat("Mode", 0.0f, 0.1f, Time.deltaTime);
            }else{
                movement.SetFloat("Mode", 0.5f, 0.1f, Time.deltaTime);
            }
        }else{
            //skacemo oz letimo
            movement.SetFloat("Mode", 1.0f, 0.1f, Time.deltaTime);
        }
    }
}
