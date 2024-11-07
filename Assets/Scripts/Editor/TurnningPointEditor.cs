using System.Reflection;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;

[CustomEditor(typeof(TurnningPoint))]
public class TurnningPointEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TurnningPoint turnningPoint = (TurnningPoint)target;

        FieldInfo isEndPointField = typeof(TurnningPoint).GetField("isEndPoint", BindingFlags.NonPublic | BindingFlags.Instance);
        FieldInfo nextPointField = typeof(TurnningPoint).GetField("nextPoint", BindingFlags.NonPublic | BindingFlags.Instance);

        bool isEndPoint = (bool)isEndPointField.GetValue(turnningPoint);
        isEndPoint = EditorGUILayout.Toggle("Is End Point", isEndPoint);
        isEndPointField.SetValue(turnningPoint, isEndPoint);

        if (!isEndPoint)
        {
            Vector3 nextPoint = (Vector3)nextPointField.GetValue(turnningPoint);
            nextPoint = EditorGUILayout.Vector3Field("Next Point", nextPoint);
            nextPointField.SetValue(turnningPoint, nextPoint);
        }

        if(GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
