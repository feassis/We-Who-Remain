using UnityEngine;

[CreateAssetMenu(fileName = "New Damage Dialog Character Action", menuName = "Configs/Dialog Action/Damage Dialog Character")]
public class DamageDialogCharacter : DialogAction
{
    [SerializeField] private int damageAmount = 1;

    public override string ExecuteAction()
    {
        GameMaster.Instance.DamageDialog(damageAmount);
        return damageAmount.ToString();
    }
}
