using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterEffect : MonoBehaviour
{
    public GameObject firePlan;

    private void OnDisable()
    {
        StopCoroutine(StartAfterEffect());
    }

    public IEnumerator StartAfterEffect()
    {
        firePlan.SetActive(true);
        yield return new WaitForSeconds(3f);
        firePlan.SetActive(false);
        transform.gameObject.SetActive(false);
    }
}
