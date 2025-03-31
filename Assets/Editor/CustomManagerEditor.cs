using UnityEditor;
using UnityEngine;

// [CustomEditor(typeof(BaseManager<FruitShop>))]
public class CustomManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GUIStyle headerStyle = new(EditorStyles.largeLabel)
        {
            fontSize = 16,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter,
            padding = new RectOffset(0, 0, 0, 0)
        };

        EditorGUILayout.LabelField("Teste", headerStyle);
        
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        
        DrawDefaultInspector();
    }
}