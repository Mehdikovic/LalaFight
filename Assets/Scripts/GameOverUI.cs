using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Image _screenImage = null;
    [SerializeField] private GameObject _UIObject = null;
    [SerializeField] private Transform _player = null;

    private void Awake()
    {
        ChangeUIActive(false);
        _player.GetComponent<HealthController>().OnDeath += OnPlayerDeath;
        _player.GetComponent<PlayerController>().OnPlayerFall += OnPlayerDeath;
    }

    //EVENT- CALLED BY BUTTON IN GameOverCanvas
    private void OnStartNewGameButtonClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    private void OnPlayerDeath()
    {
        _screenImage.gameObject.SetActive(true);
        StartCoroutine(ScreenFadeIn(Color.clear, Color.black, 1f));
    }

    IEnumerator ScreenFadeIn(Color from, Color to, float duration)
    {
        float speed = 1 / duration;
        float percent = 0;
        while(percent < 1f)
        {
            percent += Time.deltaTime * speed;
            _screenImage.color = Color.Lerp(from, to, percent);
            yield return null;
        }

        _UIObject.SetActive(true);
        Cursor.visible = true;
        Time.timeScale = 0;
    }

    private void ChangeUIActive(bool active)
    {
        _UIObject.SetActive(active);
        _screenImage.gameObject.SetActive(active);
    }
}
