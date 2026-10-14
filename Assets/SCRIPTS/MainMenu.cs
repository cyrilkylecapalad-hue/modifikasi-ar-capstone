using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
  public void Modify()
    {
        SceneManager.LoadSceneAsync(1);
    }
}
