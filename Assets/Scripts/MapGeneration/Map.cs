using System;
using UnityEngine;


namespace LalaFight
{
    [Serializable]
    public struct Map
    {
        public int seed;

        [Header("Size")]
        [Range(2, 40)]
        public int width;
        [Range(2, 40)]
        public int height;

        [Range(0f, 1f)]
        [Header("")]
        public float obstanclePercent;

        [Header("ObstacleHeight")]
        public float minObstacleHeight;
        public float maxObstacleHeight;

        [Header("ObstacleColor")]
        public Color forgroundColor;
        public Color backgroundColor;

        public Coord mapCenter => new Coord(width / 2, height / 2); // Start point player
    }
}