using System.Collections;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private const int STEP_COUNT = 8;
    private static readonly int TimerIndexHash = Animator.StringToHash("TimerIndex");

    public IEnumerator ClockStart(float duration)
    {
        animator.SetTrigger("ReStart");
        float elapsed = 0f;
        int currentIndex = -1;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float normalized = Mathf.Clamp01(elapsed / duration);
            int index = Mathf.FloorToInt(normalized * STEP_COUNT);
            index = Mathf.Clamp(index, 0, STEP_COUNT - 1);

            if (index != currentIndex)
            {
                currentIndex = index;
                animator.SetInteger(TimerIndexHash, currentIndex);
            }

            yield return null;
        }

        animator.SetInteger(TimerIndexHash, STEP_COUNT - 1);
    }
}
