using UnityEngine;

[CreateAssetMenu(fileName = "New Skill Check Action", menuName = "Configs/Dialog Action/Skill ")]
public class SkillCheckAction : DialogAction
{
    [SerializeField] private SkillType skillType;
    [SerializeField] private int skillDificulty = 12;
    [SerializeField] private DialogAction failedAction;
    [SerializeField] private DialogAction successAction;
    
    public override string ExecuteAction()
    {
        int skillRolled = GameMaster.Instance.RollSkillCheck(skillType);

        string returnString = $"{skillType.ToString()} test result: {skillRolled.ToString()} ";

        if(skillRolled < skillDificulty)
        {
            returnString += failedAction.ExecuteAction();
        }
        else 
        {
            returnString += successAction.ExecuteAction();
        }

        return returnString;
    }

}