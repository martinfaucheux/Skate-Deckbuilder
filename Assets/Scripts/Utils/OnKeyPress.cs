using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class OnKeyPress : MonoBehaviour
{
    public KeyCode key;
    public UnityEvent onKeyPress;

    private void Update()
    {
        if (Input.GetKeyDown(key))
            onKeyPress?.Invoke();
    }

    public void Quit() => Application.Quit();
    public void ReloadScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}
