using UnityEngine;


namespace LalaFight
{
    public interface IOnObjectNotifier<T> where T : MonoBehaviour
    {
        void OnAwakeCalled(T broadcaster);
    }
}