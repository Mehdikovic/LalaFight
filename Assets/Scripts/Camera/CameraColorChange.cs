using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraColorChange : MonoBehaviour
{
    [Range(0.1f, 20f)]
    [SerializeField] float _timeBetweenTransition = 1f;
    [SerializeField] Color[] _colors = new Color[2];

    private Queue<Color> _colorQueue = null;
    private Camera _camera = null;

    private void Awake()
    {
        _colorQueue = new Queue<Color>(Utility.Shuffle(_colors, 0));
        _camera = GetComponent<Camera>();
        StartCoroutine(ChangeCameraColor());
    }

    IEnumerator ChangeCameraColor()
    {
        Color from = _colorQueue.Dequeue();
        while (true)
        {
            Color to = _colorQueue.Dequeue();
            float percent = 0f;
            while (percent <= 1)
            {
                float speed = 1 / _timeBetweenTransition;
                _camera.backgroundColor = Color.Lerp(from, to, percent);
                percent += Time.deltaTime * speed;
                yield return null;
            }

            _colorQueue.Enqueue(from);
            from = to;
            yield return null;
        }
    }
}
