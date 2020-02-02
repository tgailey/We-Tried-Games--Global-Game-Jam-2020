using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    public float time;
    public bool repeat = true;
    public UnityEvent OnTimer;
    private void Start()
    {
        StartCoroutine(timer());
    }

    private IEnumerator timer ()
    {
        yield return new WaitForSeconds(time);

        OnTimer.Invoke();

        if(repeat)
        {
            timer();
        }
    }
}
