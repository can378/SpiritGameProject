using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class PlayerAnim : AnimBasic
{

    //Player GetInput에서 받아온다.
    public bool leftPressed;
    public bool rightPressed;
    public bool upPressed;
    public bool downPressed;

    private void Start()
    {
        nowAnimator = animators[front];
        //animLayers[front].SetActive(true);
        isFront = true;
    }

    private void Update()
    {
        if (leftPressed) { leftGetKeyDown(); }
        else if (rightPressed) { rightGetKeyDown(); }
        else if (upPressed)
        {
            upPressed = true;
            nowAnimator = animators[back];
            animLayers[front].SetActive(false);
            animLayers[side].SetActive(false);
            animLayers[back].SetActive(true);
            isFront = false;
        }
        else if (downPressed)
        {
            downPressed = true;
            nowAnimator = animators[front];
            animLayers[back].SetActive(false);
            animLayers[side].SetActive(false);
            animLayers[front].SetActive(true);
            isFront = true;
        }


        if (!leftPressed)
        {
            leftGetKeyUp();
        }
        if (!rightPressed)
        {
            rightGetKeyUp();
        }
        if (!upPressed)
        { upPressed = false; }
        if (!downPressed)
        { downPressed = false; }


        AnimationUpdate();

    }

    public void leftGetKeyDown()
    {
        leftPressed = true;
        nowAnimator = animators[side];
        animLayers[back].SetActive(false);
        animLayers[side].SetActive(true);
        animLayers[front].SetActive(false);
        isFront = true;

    }
    public void rightGetKeyDown()
    {
        rightPressed = true;
        nowAnimator = animators[side];
        animLayers[back].SetActive(false);
        animLayers[side].SetActive(true);
        animLayers[front].SetActive(false);
        isFront = true;
    }
    public void leftGetKeyUp() { leftPressed = false; }
    public void rightGetKeyUp() { rightPressed = false; }

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
        else if (downPressed) { verticalMove = -1; }
        else { horizontalMove = 0; verticalMove = 0; }


        if (isFront)
        {
            if (horizontalMove == 1 || horizontalMove == -1) { chardir = -horizontalMove; }
        }
        else { chardir = 1; }

        transform.localScale = new Vector3(chardir * charscale, charscale, charscale);
    }
    void AnimationUpdate()
    {

        if (horizontalMove == 0 && verticalMove == 0)
        { nowAnimator.SetBool("isRunning", false); }
        else { nowAnimator.SetBool("isRunning", true); }

    }

}
