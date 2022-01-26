using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AchievementPopUp : AchievementInfo
{
    [Tooltip("The animator of the object")]
    [SerializeField] private Animator anim;

    /// <summary>
    /// Set the info for the achievement
    /// </summary>
    /// <param name="achTitleName">The title of the achievement</param>
    /// <param name="achDesText">The description of the achievement</param>
    /// <param name="achSprite">The sprite for the achievement</param>
    /// <param name="showTime">How long the achievement will be displayed before disappearing</param>
    public void SetAchievement(string achTitleName, string achDesText, Sprite achSprite, float showTime)
    {
        base.SetAchievement(achTitleName, achDesText, achSprite);
        StartCoroutine(StartAnimation(showTime));
    }

    private IEnumerator StartAnimation(float time)
    {
        anim.SetTrigger("FadeIn");
        yield return new WaitForSeconds(time);
        anim.SetTrigger("FadeOut");
    }
}
