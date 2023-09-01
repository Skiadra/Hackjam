using System.Collections;
using System.Collections.Generic;
using static ExpManager;
using static SkillList;
using UnityEngine;
using System;

public class ExpProgression : MonoBehaviour
{
    [SerializeField] int currentExp, currentLevel, maxExp;
    public GameObject levelUpScreen;
    public static bool justLevelUp;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            ExpManager.Instance.AddExperience(100);
        }
    }

    private void Start()
    {
        SkillList.skillSystem.OnLevelChange += HandleLevelChange;
        ExpManager.Instance.OnExperienceChange += HandleExperienceChange;
    }

    private void OnDisable()
    {
        SkillList.skillSystem.OnLevelChange += HandleLevelChange;
        ExpManager.Instance.OnExperienceChange -= HandleExperienceChange;
    }

    private void HandleLevelChange()
    {
        LevelUp();
    }

    private void HandleExperienceChange(int newExp)
    {
        currentExp += newExp;
        if (currentExp >= maxExp)
        {
            SkillList.skillSystem.AddLevel();
        }
    }

    private void LevelUp()
    {
        Movement.move.inControl = false;
        justLevelUp = true;
        levelUpScreen.SetActive(true);
        currentLevel++;
        currentExp = currentExp - maxExp;
        maxExp += 100;
    }

}
