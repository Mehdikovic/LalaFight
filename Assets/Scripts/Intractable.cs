using UnityEngine;

public class Intractable : MonoBehaviour
{
    [SerializeField] protected string _message = null;
    public virtual void Use(Transform _player) { }
}