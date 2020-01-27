using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Image _screenImage = null;
    [SerializeField] private GameObject _UIObject = null;
    [SerializeField] private Transform _player = null;

    //TODO : Remember we disable PlayerInput Update
    //private PlayerInput _playerInput;

    private void Awake()
    {
        //_playerInput = _player.GetComponent<PlayerInput>();

        ChangeUIActive(false);

        _player.GetComponent<HealthController>().OnDeath += OnPlayerDeath;
        _player.GetComponent<PlayerController>().OnPlayerFall += OnPlayerDeath;
    }

    private void OnStartNewGameButtonClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnPlayerDeath()
    {
        //Destroy(_playerInput);
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
    }

    private void ChangeUIActive(bool active)
    {
        _UIObject.SetActive(active);
        _screenImage.gameObject.SetActive(active);
    }
}
