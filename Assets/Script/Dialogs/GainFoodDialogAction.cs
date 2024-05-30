using UnityEngine;

[CreateAssetMenu(fileName = "New GainFoodAction Action", menuName = "Configs/Dialog Action/Gain Food")]
public class GainFoodDialogAction : DialogAction
{
    [SerializeField] private int minFood;
    [SerializeField] private int maxFood;

    public override string ExecuteAction()
    {
        var foodToGain = Random.Range(minFood, maxFood + 1);

        GameMaster.Instance.GainFood(foodToGain);

        return foodToGain.ToString();
    }
}
