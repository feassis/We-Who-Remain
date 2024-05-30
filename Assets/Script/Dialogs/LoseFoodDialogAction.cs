using UnityEngine;

[CreateAssetMenu(fileName = "New LoseFoodAction Action", menuName = "Configs/Dialog Action/Lose Food")]
public class LoseFoodDialogAction : DialogAction
{
    [SerializeField] private int minFood;
    [SerializeField] private int maxFood;

    public override string ExecuteAction()
    {
        var foodToGain = Random.Range(minFood, maxFood + 1);

        GameMaster.Instance.LoseFood(foodToGain);

        return foodToGain.ToString();
    }
}
