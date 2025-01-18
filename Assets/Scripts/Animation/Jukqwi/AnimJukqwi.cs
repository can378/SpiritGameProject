using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimJukqwi : EnemyAnim
{
    public enum Version {Baby, Monster };
    public Animator[] animators = new Animator[2];

    public Version curVersion;

    void Start()
    {
        ChangeVersion(curVersion);
    }

    public void ChangeVersion(Version _version)
    {
        if(animator != null)
            animator.gameObject.SetActive(false);


        curVersion = _version;
        animator = animators[(int)curVersion];
        animator.gameObject.SetActive(true);
    }
}
