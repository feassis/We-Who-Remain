using UnityEngine;


public abstract class DialogAction : ScriptableObject
{
    public virtual string ExecuteAction()
    {
        return "";
    }
}
