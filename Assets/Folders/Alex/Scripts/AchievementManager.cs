using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class NewAchievement
{
    [Tooltip("The title of the achievement")]
    public string title;
    [Tooltip("The description of the achievement")]
    public string description;
    [Tooltip("The sprite for the achivement")]
    public Sprite achievementLogoSprite;
}

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager instance;

    [Tooltip("How long the achievement will be displayed before disappearing")]
    [SerializeField] private float showTime = 3f;
    [Tooltip("The achievement visual")]
    [SerializeField] private GameObject visualAchievement;
    [Tooltip("The achievement popup prefab")]
    [SerializeField] private GameObject achievementPopUpPrefab;
    [Tooltip("Where the achievement panel will be spawned")]
    [SerializeField] private GameObject achievementHolder;
    [Tooltip("Where the visual achievements are placed")]
    [SerializeField] private Transform generalTransform;
    [Tooltip("The unlocked sprite")]
    [SerializeField] private Sprite unlockedSprite;
    [Tooltip("The sprites for the achivements")]
    [SerializeField] private Sprite[] AchLogoSprites;
    [SerializeField] private NewAchievement[] achievementInfo;

    private Dictionary<string, Achievement> achievements = new Dictionary<string, Achievement>();
    public List<string> achievementTitles = new List<string>();
    private int achievementsWaiting;
    private bool achievementEarned;
    private float yOffset;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;

        AddAchievements();
    }

    private void Start()
    {
        
    }

    //private void Update()
    //{
    //if (Input.GetKeyDown(KeyCode.T))
    //    EarnAchievement("AAAAAAAAAAAAA");
    //if (Input.GetKeyDown(KeyCode.Y))
    //    EarnAchievement("OH NO");
    //if (Input.GetKeyDown(KeyCode.U))
    //    EarnAchievement("HELP");
    //if (Input.GetKeyDown(KeyCode.I))
    //    EarnAchievement("FINALLYYY");
    //}

    /// <summary>
    /// Call this if a achievement is earned
    /// </summary>
    /// <param name="title">The title of the achievements</param>
    public void EarnAchievement(string title)
    {
        //Working but last earned achievement has delay
        if (!achievementEarned)
        {
            if (achievements[title].EarnAchievement(unlockedSprite))
            {
                achievementEarned = true;
                GameObject achievementPopUp = Instantiate(achievementPopUpPrefab, achievementHolder.transform.position, Quaternion.identity, achievementHolder.transform.parent);
                achievementPopUp.transform.SetParent(achievementHolder.transform);

                AchievementPopUp achPopUp = achievementPopUp.GetComponent<AchievementPopUp>();
                achPopUp.SetAchievement(achievements[title].Title, achievements[title].Description, achievements[title].AchLogoSprite, showTime);

                Destroy(achievementPopUp, showTime + 1);
            }
        }
        else
        {
            achievementTitles.Add(title);
            achievementsWaiting++;
            if (achievementsWaiting <= 1)
                StartCoroutine(EarnNextAchievement(achievementTitles[0]));
        }
    }

    private IEnumerator EarnNextAchievement(string title)
    {
        yield return new WaitForSeconds(showTime);
        achievementEarned = false;
        EarnAchievement(achievementTitles[0]);
        achievementTitles.RemoveAt(0);
        achievementsWaiting--;
        if (achievementsWaiting > 0)
            StartCoroutine(EarnNextAchievement(achievementTitles[0]));
    }

    /// <summary>
    /// Create the achievements and place in achievement menu
    /// </summary>
    /// <param name="title">The title of the achievement</param>
    /// <param name="description">The description of the achievement</param>
    /// <param name="achLogo">The sprite of the achievement</param>
    private void CreateAchievement(string title, string description, Sprite achLogo)
    {
        GameObject ach = Instantiate(visualAchievement, new Vector3(generalTransform.position.x, generalTransform.position.y - yOffset, generalTransform.position.z), Quaternion.identity, generalTransform.parent);
        ach.transform.SetParent(generalTransform);

        Achievement newAchievement = new Achievement(title, description, achLogo, unlockedSprite, ach);

        achievements.Add(title, newAchievement);

        AchievementInfo achInfo = ach.GetComponent<AchievementInfo>();
        achInfo.SetAchievement(title, description, achLogo);
    }

    /// <summary>
    /// Add the achievements to the achievement menu
    /// </summary>
    private void AddAchievements()
    {
        for (int i = 0; i < achievementInfo.Length; i++)
        {
            CreateAchievement(achievementInfo[i].title, achievementInfo[i].description, achievementInfo[i].achievementLogoSprite);
            yOffset += 1.19f;
        }
        //CreateAchievement("OH NO", "Join the game for the first time", AchLogoSprites[0]);
        //CreateAchievement("Drive", "Reach 100 meter", AchLogoSprites[0]);
        //CreateAchievement("To the horizon", "Reach 1000 meter", AchLogoSprites[0]);
        //CreateAchievement("WATCH THE ROAD", "Die within 3 seconds", AchLogoSprites[0]);
    }
}
