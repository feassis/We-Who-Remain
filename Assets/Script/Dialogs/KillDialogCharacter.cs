using UnityEngine;

[CreateAssetMenu(fileName = "New Kill Dialog Character Action", menuName = "Configs/Dialog Action/Kill Dialog Character")]
public class KillDialogCharacter : DialogAction
{
    public override string ExecuteAction()
    {
        GameMaster.Instance.KillDialog();
        return "";
    }
}
