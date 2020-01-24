using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var mapGenerator = target as MapGenerator;
        if(DrawDefaultInspector())
            mapGenerator.AssembleMapGeneration();
        
        if (GUILayout.Button("Generate Map"))
            mapGenerator.AssembleMapGeneration();
    }
}
