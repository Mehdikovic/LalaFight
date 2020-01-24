using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackTrailController : MonoBehaviour
{
    [SerializeField] private MeleeAttack _melee = null;
    [SerializeField] private TrailRenderer _trail = null;

    private void Awake()
    {
        _melee.Attacked += OnMeleeAttack;
    }

    private void OnMeleeAttack()
    {
        if (_trail.emitting)
            _trail.emitting = false;
        _trail.emitting = true;
    }
}
