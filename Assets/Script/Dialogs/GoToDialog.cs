using UnityEngine;

[CreateAssetMenu (fileName = "New Go To Dialog", menuName = "Configs/Dialog Action/GoToDialog")]
public class GoToDialog : DialogAction
{
    [SerializeField] private DialogConfig desiredDialog;
    public override string ExecuteAction()
    {
        GameMaster.Instance.LoadDialog(desiredDialog);

        return "";
    }
}