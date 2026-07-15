using System;
using System.Collections;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private float gameTime;
    private float currentTime;
    
    private Coroutine timerCoroutine;

    private void OnEnable()
    {
        GameManager.gameEvents.AddEvent(GameEvents.EventType.OnGameStart, StartTimer);
    }

    public void StartTimer()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }

        timerCoroutine = StartCoroutine(StartTimerLoop());
    }

    IEnumerator StartTimerLoop()
    {
        currentTime = gameTime;
        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            yield return null;
        }
    }
}
