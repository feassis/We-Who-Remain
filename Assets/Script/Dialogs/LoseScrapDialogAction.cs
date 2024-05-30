using UnityEngine;

public partial class GainScrapDialogAction
{
    [CreateAssetMenu(fileName = "New LoseScrapAction Action", menuName = "Configs/Dialog Action/Lose Scrap")]
    public class LoseScrapDialogAction : DialogAction
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
}