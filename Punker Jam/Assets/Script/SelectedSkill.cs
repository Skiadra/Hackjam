using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedSkill : MonoBehaviour
{
    [SerializeField] GameObject LevelUpScreen;
    public void Selected()
    {
        Text selectedText = gameObject.GetComponentInChildren<Text>();
        SkillList.skillSystem.SkillSet.Remove(selectedText.text);
        SkillList.skillSystem.ResetSkillButtons();
        LevelUpScreen.SetActive(false);
        // Movement.move.inControl = true;
    }
}
