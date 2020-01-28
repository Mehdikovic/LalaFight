using UnityEngine;

public class Intractable : MonoBehaviour
{
    [SerializeField] protected string _message = null;
    protected virtual void Awake() { }
    public virtual void Use(PlayerController _player) { }
}