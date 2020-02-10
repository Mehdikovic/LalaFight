using System;
using UnityEngine;
using UnityEngine.UI;

namespace LalaFight
{
    public class HealthHUDUI : MonoBehaviour
    {
        [SerializeField] private Sprite _defaultAvatar = null;
        [SerializeField] private Image _avatar = null;
        [SerializeField] private Image _healthBar = null;
        [SerializeField] private Image _armorBar = null;

        PlayerHealthController _playerhealthController;

        internal void UpdateUI()
        {
            if (_playerhealthController == null)
                return;
            
            _healthBar.fillAmount = _playerhealthController.remainingHealthPercent / 100f;
            _armorBar.fillAmount = _playerhealthController.remainingShieldPercent / 100f;
        }

        internal void Init(PlayerHealthController playerHealthController, Sprite sprite)
        {
            _playerhealthController = playerHealthController;
            _avatar.sprite = sprite ?? _defaultAvatar;
        }
    }
}
