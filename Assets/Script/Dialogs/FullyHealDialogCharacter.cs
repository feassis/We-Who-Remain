using UnityEngine;

[CreateAssetMenu(fileName = "New Fully Heal Dialog Character Action", menuName = "Configs/Dialog Action/Fully Heal Dialog Character")]
public class FullyHealDialogCharacter : DialogAction
{
    public override string ExecuteAction()
    {
        GameMaster.Instance.FullyHealDialog();
        return "";
    }
}