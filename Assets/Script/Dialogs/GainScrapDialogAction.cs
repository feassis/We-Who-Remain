using UnityEngine;

[CreateAssetMenu(fileName = "New GainScrapAction Action", menuName = "Configs/Dialog Action/Gain Scrap")]
public partial class GainScrapDialogAction : DialogAction
{
    [SerializeField] private int minScrap;
    [SerializeField] private int maxScrap;

    public override string ExecuteAction()
    {
        var foodToGain = Random.Range(minScrap, maxScrap + 1);

        GameMaster.Instance.GainScrap(foodToGain);

        return foodToGain.ToString();
    }
}