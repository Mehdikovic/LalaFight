using UnityEngine;


namespace LalaFight
{
    [System.Serializable]
    public struct Wave
    {
        public bool infinite;
        public float timeBetweenSpawn;
        public int enemyHealth;
        public int enemyAmount;
        public int enemySpeed;
        public int enemyDamage;
        public float enemyAttackRate;
        public float enemyAttackSpeed;
        public Color enemyColor1;
        public Color enemyColor2;
    }
}