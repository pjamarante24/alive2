using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void OnPlay()
    {
        SceneManager.LoadScene("ZombieGameSmall");
    }
    public void OnExit()
    {
        Application.Quit();
    }
}
