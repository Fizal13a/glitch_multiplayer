using System;
using UnityEngine;

public abstract class TaskObjective : MonoBehaviour
{
    public bool IsCompleted { get; private set; }

    public event Action<TaskObjective> OnCompleted;

    protected void CompleteObjective()
    {
        if (IsCompleted)
            return;

        IsCompleted = true;
        OnCompleted?.Invoke(this);
    }
}