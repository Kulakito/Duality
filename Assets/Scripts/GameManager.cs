using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject _losePanel;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(_losePanel.transform.parent.gameObject);
    }

    private void Start()
    {
        GhostPowerTracker.OnGhostPowerEnd += EnableLosePanel;
    }

    private void OnDestroy()
    {
        GhostPowerTracker.OnGhostPowerEnd -= EnableLosePanel;
    }

    private void EnableLosePanel()
    {
        _losePanel.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
        _losePanel.SetActive(false);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
