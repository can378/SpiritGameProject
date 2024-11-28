using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AnimBasic : MonoBehaviour
{
    //public List<Animator> animators;//0:front, 1:back, 2:side
    //public List<GameObject> animLayers;//0:front 1:back, 2:side
    //protected int front = 0, back = 1, side = 2;
    public Animator animator;

    [HideInInspector]
    public float horizontalMove { get; set; }
    [HideInInspector]
    public float verticalMove { get; set; }
    //protected bool isFront;

    protected float chardir = 1;
    protected float charscale = 0.3f;




    

}
