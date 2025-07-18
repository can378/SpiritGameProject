using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Newtonsoft.Json;


public class AttackAnimation
{
    public int SwingDir;
}

[System.Serializable]
public class WeaponAnimationInfo
{
    public float PreDelay;
    public float Rate;
    public float PostDelay;
    public List<AttackAnimation> Animation;

    // 공격 한번당 걸리는 시간
    public float GetSPA()
    {
        return PreDelay + Rate + PostDelay;
    }

    // 1초당 공격가능 횟수
    public float GetAPS()
    {
        return 1 / GetSPA();
    }
}


public class PlayerAnim : AnimBasic
{
    public TextAsset jsonFile;

    public Dictionary<string, WeaponAnimationInfo> AttackAnimationData;

    public SpriteRenderer[] WeaponSpriteRenderer = new SpriteRenderer[(int)WEAPON_TYPE.END];

    public Sprite[] WeaponSprite;

    void Start()
    {
        AttackAnimationData = JsonConvert.DeserializeObject<Dictionary<string, WeaponAnimationInfo>>(jsonFile.text);
    }

    public void ChangeWeaponSprite(WEAPON_TYPE _Type, int _WeaponIndex)
    {
        if (WeaponSpriteRenderer[(int)_Type] == null || WeaponSprite[_WeaponIndex] == null)
            return;


        WeaponSpriteRenderer[(int)_Type].sprite = WeaponSprite[_WeaponIndex];
    }

    public Vector3 WeaponSpritePos(WEAPON_TYPE _Type)
    {
        return WeaponSpriteRenderer[(int)_Type].transform.position;
    }

    //Player GetInput에서 받아온다.
    /*
    public bool leftPressed;
    public bool rightPressed;
    public bool upPressed;
    public bool downPressed;
    
    private void Start()
    {
        //nowAnimator = animators[front];
        //animLayers[front].SetActive(true);
        //isFront = true;
    }
    */
    /*
        private void Update()
        {

           if (leftPressed) { leftPressed = true; }
           else if (rightPressed) { rightPressed = true; }

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
                leftPressed = true;
            }
            if (!rightPressed)
            {
                rightPressed = true;
            }
            if (!upPressed)
            { upPressed = false; }
            if (!downPressed)
            { downPressed = false; }

             
        AnimationUpdate();
        Run();
    }
    */
    /*
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
    */

    void Run()
    {
        /*
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
        
        if(horizontalMove<0) { chardir = 1; }
        else if(horizontalMove>0) {  chardir = -1; }

        transform.localScale = new Vector3(chardir * charscale, charscale, charscale);
        */
    }
    void AnimationUpdate()
    {
        /*
        if (horizontalMove == 0 && verticalMove == 0)
        { animator.SetBool("isRun", false); }
        else { animator.SetBool("isRun", true); }
        */
    }

}
