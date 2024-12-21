using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AnimBasic : MonoBehaviour
{
    public Animator animator;
    public Status status;

    void Update()
    {
        if (status.moveVec == Vector2.zero)
        {
            animator.SetBool("isWalk", false);
        }
        else
        {
            animator.SetBool("isWalk", true);
            if (status.moveVec.x < 0)
            {
                animator.transform.localScale = new Vector3(1, 1, 1);
            }
            else if (status.moveVec.x > 0)
            {
                animator.transform.localScale = new Vector3(-1, 1, 1);
            }
            
        }
    }

    /// <summary>
    /// �ش� �������� �ִϸ��̼� ������ �����Ѵ�.
    /// �ٶ󺸴� ������ �ѹ� �����δ�.
    /// 
    /// </summary>
    public void ChangeDirection(Vector3 _Direction)
    {
        if (_Direction.x < 0)
        {
            animator.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (_Direction.x > 0)
        {
            animator.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

}