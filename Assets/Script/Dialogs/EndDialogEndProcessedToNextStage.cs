using UnityEngine;

[CreateAssetMenu(fileName = "New Unload End Dialog and Go TO Next Stage Action", menuName = "Configs/Dialog Action/Unload And Go To Next Stage Dialog")]
public class EndDialogEndProcessedToNextStage : DialogAction
{
    public override string ExecuteAction()
    {
        GameMaster.Instance.UnloadDialog();
        GameMaster.Instance.ProgressToNextStage();

        return "";
    }
}