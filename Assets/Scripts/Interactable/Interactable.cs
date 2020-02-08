using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] protected string _message = null;
    public virtual void Interact(Transform player) { }
}