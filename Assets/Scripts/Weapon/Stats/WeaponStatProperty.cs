using System;
using UnityEngine;

public abstract class WeaponStatProperty<T> : ScriptableObject, IWeaponProperty
{
    [SerializeField] private T _defaultValue = default(T);
    [SerializeField] private T[] _values = null;

    private int _currentLevel = 0;

    public event Action<T> OnStatUpdated;

    //TODo: delete this when the game is ready to play
    private void OnEnable()
    {
        _currentLevel = 0;
    }

    public virtual T value {
        get {
            if (_values.Length > 0)
                return _values[_currentLevel];
            return _defaultValue;
        }
    }

    public virtual int GetLevel()
    {
        return _currentLevel + 1;
    }
    public virtual bool Update()
    {
        if (_values.Length == 0)
            return false;
        if (_currentLevel == _values.Length - 1)
            return false;
        ++_currentLevel;
        RaiseOnStatUpdated();
        return true;
    }

    public void RaiseOnStatUpdated()
    {
        OnStatUpdated?.Invoke(value);
    }
}