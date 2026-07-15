using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameTaskManager : MonoBehaviour
{
    [Header("Tasks")]
    [SerializeField] private List<GameTask> availableTasks;
    [SerializeField] private int tasksPerRound = 3;

    private readonly List<GameTask> activeTasks = new();

    public IReadOnlyList<GameTask> ActiveTasks => activeTasks;

    public event Action OnAllTasksCompleted;

    private void OnEnable()
    {
        GameManager.gameEvents.AddEvent(GameEvents.EventType.OnGameStart, GenerateTasks);
    }

    private void GenerateTasks()
    {
        activeTasks.Clear();

        List<GameTask> shuffledTasks = availableTasks
            .OrderBy(task => UnityEngine.Random.value)
            .ToList();

        activeTasks.AddRange(
            shuffledTasks.Take(tasksPerRound)
        );

        foreach (GameTask task in availableTasks)
        {
            bool isActive = activeTasks.Contains(task);
            task.gameObject.SetActive(isActive);
        }

        SubscribeToTasks();
    }

    private void SubscribeToTasks()
    {
        foreach (GameTask task in activeTasks)
        {
            task.OnCompleted += OnTaskCompleted;
        }
    }

    private void OnTaskCompleted(GameTask completedTask)
    {
        Debug.Log(
            $"Task Completed: {completedTask.TaskDefinition.taskName}"
        );

        CheckGameCompletion();
    }

    private void CheckGameCompletion()
    {
        if (activeTasks.Any(task => !task.IsCompleted))
            return;

        CompleteAllTasks();
    }

    private void CompleteAllTasks()
    {
        Debug.Log("ALL TASKS COMPLETED");

        OnAllTasksCompleted?.Invoke();
    }

    private void OnDestroy()
    {
        foreach (GameTask task in activeTasks)
        {
            task.OnCompleted -= OnTaskCompleted;
        }
    }
}