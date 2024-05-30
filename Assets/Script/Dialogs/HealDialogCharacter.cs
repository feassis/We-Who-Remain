using UnityEngine;

[CreateAssetMenu(fileName = "New Heal Dialog Character Action", menuName = "Configs/Dialog Action/Heal Dialog Character")]
public class HealDialogCharacter : DialogAction
{
    [SerializeField] private int healAmount = 1;

    public override string ExecuteAction()
    {
        GameMaster.Instance.HealDialog(healAmount);
        return healAmount.ToString();
    }
}
