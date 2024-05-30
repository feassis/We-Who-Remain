using UnityEngine;

[CreateAssetMenu(fileName = "New GainScrapAction Action", menuName = "Configs/Dialog Action/Gain Scrap")]
public partial class GainScrapDialogAction : DialogAction
{
    [SerializeField] private int minScrap;
    [SerializeField] private int maxScrap;

    public override string ExecuteAction()
    {
        var scrapToGain = Random.Range(minScrap, maxScrap + 1);

        GameMaster.Instance.GainScrap(scrapToGain);

        return "You've gained " + scrapToGain.ToString() + " scrap";
    }
}