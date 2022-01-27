using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AchievementInfo : MonoBehaviour
{
    [Tooltip("The title of the achievement")]
    [SerializeField] private TextMeshProUGUI titleText;
    [Tooltip("The description of the achievement")]
    [SerializeField] private TextMeshProUGUI descriptionText;
    [Tooltip("The achievement logo")]
    [SerializeField] private Image achievementLogoImage;

    public Image AchievementLogoImage { get { return achievementLogoImage; } }

    /// <summary>
    /// Set the info for the achievement
    /// </summary>
    /// <param name="achTitleName">The title of the achievement</param>
    /// <param name="achDesText">The description of the achievement</param>
    /// <param name="achSprite">The sprite for the achievement</param>
    public virtual void SetAchievement(string achTitleName, string achDesText, Sprite achSprite = null)
    {
        titleText.text = achTitleName;
        descriptionText.text = achDesText;
        achievementLogoImage.sprite = achSprite;
    }
}
