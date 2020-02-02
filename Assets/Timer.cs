using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    public float time;
    public bool repeat = true;
    public UnityEvent OnTimer;
    protected virtual void Start()
    {
        StartCoroutine(timer());
    }

    protected IEnumerator timer ()
    {
        yield return new WaitForSeconds(time);

        OnTimer.Invoke();

        if(repeat)
        {
            timer();
        }
    }
}
