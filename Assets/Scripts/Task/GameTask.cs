using System;
using System.Collections.Generic;
using UnityEngine;

public class GameTask : MonoBehaviour
{
    [SerializeField] private TaskDefinition taskDefinition;
    [SerializeField] private List<TaskObjective> objectives;
    
    public event Action<GameTask> OnCompleted;

    public TaskDefinition TaskDefinition => taskDefinition;
    public bool IsCompleted { get; private set; }

    private void OnEnable()
    {
        foreach (TaskObjective objective in objectives)
        {
            objective.OnCompleted += OnObjectiveCompleted;
        }
    }

    private void OnDisable()
    {
        foreach (TaskObjective objective in objectives)
        {
            objective.OnCompleted -= OnObjectiveCompleted;
        }
    }

    private void OnObjectiveCompleted(TaskObjective objective)
    {
        CheckTaskCompletion();
    }

    private void CheckTaskCompletion()
    {
        foreach (TaskObjective objective in objectives)
        {
            if (!objective.IsCompleted)
                return;
        }

        CompleteTask();
    }

    private void CompleteTask()
    {
        if (IsCompleted)
            return;

        IsCompleted = true;

        Debug.Log($"Task Completed: {taskDefinition.taskName}");

        OnCompleted?.Invoke(this);
    }
}