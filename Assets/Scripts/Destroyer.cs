using UnityEngine;


namespace LalaFight
{
    public class Destroyer : MonoBehaviour
    {
        [SerializeField] private float _lifetime = 10f;

        void Awake()
        {
            Destroy(gameObject, _lifetime);
        }
    }
}