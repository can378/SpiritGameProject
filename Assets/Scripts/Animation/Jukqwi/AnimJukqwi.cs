using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimJukqwi : EnemyAnim
{
    public CapsuleCollider2D JukquwiCollider;

    public enum Version {Baby, Adult };
    public Animator[] animators = new Animator[2];
    public GameObject[] animationObject = new GameObject[2];
    public Vector2[] hitBox = new Vector2[2];

    public Version curVersion;

    void Start()
    {
        ChangeVersion(curVersion);
    }

    public void ChangeVersion(Version _version)
    {
        animationObject[(int)curVersion].SetActive(false);

        curVersion = _version;
        animator = animators[(int)curVersion];
        animationObject[(int)curVersion].SetActive(true);
        JukquwiCollider.size = hitBox[(int)curVersion];
    }
}
