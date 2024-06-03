using UnityEngine;


public abstract class DialogAction : ScriptableObject
{
    public virtual string ExecuteAction()
    {
        return "";
    }
}

[CreateAssetMenu (fileName = "New Go To Dialog", menuName = "Configs/DialogAction/GoToDialog")]
public abstract class GoToDialog : DialogAction
{
    [SerializeField] private DialogConfig desiredDialog;
    public override string ExecuteAction()
    {
        GameMaster.Instance.LoadDialog(desiredDialog);

        return "";
    }
}