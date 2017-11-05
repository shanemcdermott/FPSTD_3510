using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapFactory))]
public class MapFactoryEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        DrawDefaultInspector();
        MapFactory builder = (MapFactory)target;
        if (GUILayout.Button("Build Maze"))
        {
            builder.SetupScene();
        }

		if (GUILayout.Button ("Add Random Walls")) {
			builder.addSomeWalls (5);
		}
			
    }
		

}
