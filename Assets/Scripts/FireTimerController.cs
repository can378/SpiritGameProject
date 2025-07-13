using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTimerController : MonoBehaviour
{
    public Animator animator;
    public GameObject braizer; //화로 \

    private void Start()
    {
        //test
        StartFire(10);
    }
    public void StartFire(float totalDuration)//불 지속시간
    {
        StartCoroutine(FireSequence(totalDuration));
    }

    private IEnumerator FireSequence(float totalDuration)
    {
        // 단계별 비율
        float strongTime = totalDuration * 0.4f;
        float mediumTime = totalDuration * 0.5f;
        float explosionTime = totalDuration * 0.1f; // 펑

        // 전환
        Debug.Log("strong fire🔥");
        animator.Play("StrongFire");
        yield return new WaitForSeconds(strongTime);

        Debug.Log("medium fire🔥");
        animator.Play("MediumFire");
        yield return new WaitForSeconds(mediumTime);

        Debug.Log("explosion fire💥");
        animator.Play("Explosion");
        yield return new WaitForSeconds(explosionTime);



        gameObject.SetActive(false);
    }
}
