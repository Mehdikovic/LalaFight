using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    [SerializeField] private float _lifetime = 10f;
    
    void Awake()
    {
        Destroy(gameObject, _lifetime);
    }

}
