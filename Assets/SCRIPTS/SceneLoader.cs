using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadDiamondDisc()
    {
        SceneManager.LoadSceneAsync(2);
    }
}