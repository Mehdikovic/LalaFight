using System.Collections;
using UnityEngine;

public class IntractableController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        print("Intract");
    }
}
