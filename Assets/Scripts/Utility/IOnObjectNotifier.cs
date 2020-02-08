using UnityEngine;

public interface IOnObjectNotifier<T> where T : MonoBehaviour
{
    void OnAwakeCalled(T broadcaster);
}
