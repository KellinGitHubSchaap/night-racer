using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Achievement
{
    public string Title { get { return title; } }
    public string Description { get { return description; } }
    public Sprite AchLogoSprite { get { return achLogoSprite; } }

    private string title;
    private string description;
    private Sprite achLogoSprite;
    private Sprite achBackgroundSprite;
    private GameObject achievement;
    private bool unlocked;

    public Achievement(string title, string description, Sprite achLogoSprite, Sprite achBackgroundSprite, GameObject achievement)
    {
        this.title = title;
        this.description = description;
        this.achLogoSprite = achLogoSprite;
        this.achBackgroundSprite = achBackgroundSprite;
        this.achievement = achievement;
        unlocked = false;
        LoadAchievements();
    }

    /// <summary>
    /// Check if the achievement is already earned or not
    /// </summary>
    /// <returns></returns>
    public bool EarnAchievement(Sprite sprite)
    {
        if (!unlocked)
        {
            achievement.GetComponent<Image>().sprite = sprite;
            SaveAchievement(true);
            return true;
        }
        else
            return false;
    }

    /// <summary>
    /// Call this to save the earned achievement
    /// </summary>
    /// <param name="value">If the achievement is earned or not</param>
    private void SaveAchievement(bool value)
    {
        unlocked = value;
        PlayerPrefs.SetInt(title, value ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void LoadAchievements()
    {
        unlocked = PlayerPrefs.GetInt(title) == 1 ? true : false;
        if (unlocked)
            achievement.GetComponent<Image>().sprite = achBackgroundSprite;
    }
}
