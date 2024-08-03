using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAnim : AnimBasic
{
    public List<Animator> babyAnimator;
    public List<Animator> adultAnimator;
    public List<GameObject> babyLayer;
    public List<GameObject> adultLayer;


    public void changeAnimToAdult() 
    {
        animLayers[front].SetActive(false);
        animLayers[back].SetActive(false);
        animLayers[side].SetActive(false);
        animators = adultAnimator;
        animLayers = adultLayer;

    }
    public void changeAnimToBaby() 
    {
        animLayers[front].SetActive(false);
        animLayers[back].SetActive(false);
        animLayers[side].SetActive(false);
        animators = babyAnimator;
        animLayers = babyLayer;
    }

    private void Update()
    {
        applyAnim();
        LRInversion();
    }
    void applyAnim() 
    {
        //animLayers[front].SetActive(false);
        //animLayers[back].SetActive(false);
        //animLayers[side].SetActive(false);

        if (verticalMove == -1)
        {
            //nowAnimator = animators[front];
            animLayers[front].SetActive(true);
            animLayers[back].SetActive(false);
            animLayers[side].SetActive(false);
        }
        else if (verticalMove == 1)
        {
            //nowAnimator = animators[back];
            animLayers[front].SetActive(false);
            animLayers[back].SetActive(true);
            animLayers[side].SetActive(false);
        }
        else if (horizontalMove != 0)
        {
            //nowAnimator = animators[side];
            animLayers[front].SetActive(false);
            animLayers[back].SetActive(false);
            animLayers[side].SetActive(true);
        }
        else 
        {
            //nowAnimator = animators[front];
            animLayers[front].SetActive(true);
            animLayers[back].SetActive(false);
            animLayers[side].SetActive(false);
        }
    
    }

    void LRInversion()
    {
        charscale = 1;
        if (horizontalMove == 1 || horizontalMove == -1) { chardir = -horizontalMove; }
        else { chardir = 1; }

        transform.localScale = new Vector3(chardir * charscale, charscale, charscale);
    }
}
