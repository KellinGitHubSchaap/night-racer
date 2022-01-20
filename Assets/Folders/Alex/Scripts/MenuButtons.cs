using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    /// <summary>
    /// Load a menu
    /// </summary>
    /// <param name="menus">The menu you want to load</param>
    public void LoadMenu(GameObject menus)
    {
        menus.SetActive(true);
    }

    /// <summary>
    /// Disable a menu
    /// </summary>
    /// <param name="menus">The menu you want to disable</param>
    public void DisableMenu(GameObject menus)
    {
        menus.SetActive(false);
    }

    /// <summary>
    /// Load a specific scene
    /// </summary>
    /// <param name="sceneName">The scene name that you want to load</param>
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Call this to restart the current scene
    /// </summary>
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Call this to quit the game
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
