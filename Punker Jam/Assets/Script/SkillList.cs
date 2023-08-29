using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillList : MonoBehaviour
{
    public static SkillList skillSystem;
    public bool selectionSuccess = false;
    [SerializeField] public List<string> SkillSet;
    [SerializeField] GameObject skillOne, skillTwo;
    public delegate void LevelUpHandler();
    public event LevelUpHandler OnLevelChange;


    List<string> GetRandomElements(List<string> inputList, int count)
    {
        List<string> outputList = new List<string>();
        int index = -1;
        for (int i = 0; i < count; i++)
        {
            index = Random.Range(0, inputList.Count);
            outputList.Add(inputList[index]);
        }
        if (count > 1)
        {
            if (outputList[0].Equals(outputList[1]))
            {
                if (index + 1 >= inputList.Count) index = -1;
                outputList[1] = inputList[index + 1];
            }
        }
        return outputList;
    }
    void Awake()
    {
        if (skillSystem != null && skillSystem != this)
        {
            Destroy(this);
        }
        else
        {
            skillSystem = this;
        }
    }

    // void Update()
    // {
    //     if (ExpProgression.justLevelUp)
    //     {
    //         Movement.move.inControl = false;
    //         int num = 0;
    //         Debug.Log(SkillSet.Count);
    //         if (SkillSet.Count >= 2) num = 2;
    //         else num = SkillSet.Count;

    //         var randomList = GetRandomElements(SkillSet, num);

    //         Text skillText = skillOne.GetComponentInChildren<Text>();
    //         Text skillText2 = skillTwo.GetComponentInChildren<Text>();
    //         if (num > 0)
    //             skillText.text = randomList[0];
    //         if (num > 1)
    //             skillText2.text = randomList[1];
    //         ExpProgression.justLevelUp = false;
    //     }

    // }

    public void AddLevel()
    {
        Time.timeScale = 0f;
        OnLevelChange?.Invoke();
        // Movement.move.inControl = false;
        int num = 0;
        Debug.Log(SkillSet.Count);
        if (SkillSet.Count >= 2) num = 2;
        else num = SkillSet.Count;

        var randomList = GetRandomElements(SkillSet, num);

        Text skillText = skillOne.GetComponentInChildren<Text>();
        Text skillText2 = skillTwo.GetComponentInChildren<Text>();
        if (num > 0)
            skillText.text = randomList[0];
        if (num > 1)
            skillText2.text = randomList[1];
        ExpProgression.justLevelUp = false;
    }

    public void ResetSkillButtons()
    {
        Text skillText = skillOne.GetComponentInChildren<Text>();
        Text skillText2 = skillTwo.GetComponentInChildren<Text>();

        skillText.text = "Skill One";
        skillText2.text = "Skill Two";
        selectionSuccess = false;
        Time.timeScale = 1f;
    }
}
