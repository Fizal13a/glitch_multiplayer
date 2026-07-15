using UnityEngine;

[CreateAssetMenu(fileName = "TaskDefinition", menuName = "Task/TaskDefinition")]
public class TaskDefinition : ScriptableObject
{
    public string taskName;
    [TextArea] public string description;
}
