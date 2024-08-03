using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AnimBasic : MonoBehaviour
{
    public List<Animator> animators;//0:front, 1:back, 2:side
    public List<GameObject> animLayers;//0:front 1:back, 2:side

    protected Animator nowAnimator;


    public float horizontalMove;
    public float verticalMove;
    protected bool isFront;

    protected float chardir = 1;
    protected float charscale = 0.7f;




    protected int front = 0, back = 1, side = 2;


    
   
    

    
}
