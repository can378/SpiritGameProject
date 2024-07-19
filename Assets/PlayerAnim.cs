using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    public List<Animator> animators;//0:front, 1:back, 2:side
    public List<GameObject> playerLayers;//0:front 1:back, 2:side

    Animator nowAnimator;

    Rigidbody2D rigid;
    Vector3 movement;
    //Animator animator;
    private float speed=10f;
    
    float horizontalMove;
    float verticalMove;
    bool isFront;

    float chardir = 1;
    float charscale = 0.7f;

    bool leftPressed;
    bool rightPressed;
    bool upPressed;
    bool downPressed;

    int front=0, back=1,side=2;


    private void Awake()
    {
        //animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        nowAnimator = animators[front];
        isFront=true;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) 
        {
            leftGetKeyDown();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            rightGetKeyDown();
        }
        if(Input.GetKeyDown(KeyCode.W)) 
        { 
            upPressed = true;
            nowAnimator = animators[back];
            playerLayers[front].SetActive(false);
            playerLayers[side].SetActive(false);
            playerLayers[back].SetActive(true);
            isFront = false;

        }
        if(Input.GetKeyDown(KeyCode.S)) 
        { 
            downPressed = true;
            nowAnimator = animators[front];
            playerLayers[back].SetActive(false);
            playerLayers[side].SetActive(false);
            playerLayers[front].SetActive(true);
            isFront = true;
        }


        if (Input.GetKeyUp(KeyCode.A))
        {
            leftGetKeyUp();
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            rightGetKeyUp();
        }
        if (Input.GetKeyUp(KeyCode.W))
        { upPressed = false; }
        if (Input.GetKeyUp(KeyCode.S))
        { downPressed = false; }


        AnimationUpdate();
    }
    public void leftGetKeyDown() 
    { 
        leftPressed = true;
        nowAnimator = animators[side];
        playerLayers[back].SetActive(false);
        playerLayers[side].SetActive(true);
        playerLayers[front].SetActive(false);
        isFront = true;

    }
    public void rightGetKeyDown()
    {  
        rightPressed = true;
        nowAnimator = animators[side];
        playerLayers[back].SetActive(false);
        playerLayers[side].SetActive(true);
        playerLayers[front].SetActive(false);
        isFront = true;
    }
    public void leftGetKeyUp() {  leftPressed = false; }
    public void rightGetKeyUp() {  rightPressed = false; }
    
    private void FixedUpdate()
    {
        Run();
    }
    void Run() 
    {
        if (leftPressed && rightPressed) { horizontalMove = 0; }
        else if (rightPressed) { horizontalMove = 1; }
        else if (leftPressed) { horizontalMove = -1; }
        else if (upPressed && downPressed) { verticalMove = 0; }
        else if (upPressed) { verticalMove = 1; }
        else if(downPressed) { verticalMove = -1; }
        else { horizontalMove = 0; verticalMove = 0; }

        movement.Set(horizontalMove, verticalMove, 0);
        movement = movement.normalized * speed * Time.deltaTime;

        if (isFront)
        {
            if (horizontalMove == 1 || horizontalMove == -1) { chardir = -horizontalMove; }
        }
        else { chardir = 1; }
        
        
        rigid.MovePosition(transform.position + movement);
        transform.localScale=new Vector3(chardir*charscale,charscale,charscale);

    }
    void AnimationUpdate() 
    {

        if (horizontalMove == 0 && verticalMove == 0)
        { nowAnimator.SetBool("isRunning", false); }
        else { nowAnimator.SetBool("isRunning", true); }
            
    }
}
