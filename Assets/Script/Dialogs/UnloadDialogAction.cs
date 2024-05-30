using UnityEngine;

[CreateAssetMenu(fileName = "New Unload Dialog Action Action", menuName = "Configs/Dialog Action/Unload Dialog")]
public class UnloadDialogAction : DialogAction
{
    public override string ExecuteAction()
    {
        GameMaster.Instance.UnloadDialog();

        return "";
    }
}