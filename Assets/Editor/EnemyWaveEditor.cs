using UnityEditor;
using UnityEngine;
using System.Linq;
using System;

[CustomEditor(typeof(EnemyWave))]
public class EnemyWaveEditor : Editor
{
    SerializedProperty m_Enemies;
    SerializedProperty m_Ratio;
    SerializedProperty m_randomRatio;
    SerializedProperty m_enemyCount;

    bool randomizeButton;

    const int max = 100;
    int currMax = 0;
    int current = 0;
    private void OnEnable()
    {
        m_Enemies = serializedObject.FindProperty("enemies");
        m_randomRatio = serializedObject.FindProperty("randomRatio");
        m_Ratio = serializedObject.FindProperty("ratio");
        m_enemyCount = serializedObject.FindProperty("enemyCount");
    }
    public override void OnInspectorGUI()
    {
        EnemyWave myTarget = (EnemyWave)target;

        m_Enemies = serializedObject.FindProperty("enemies");
        m_Ratio = serializedObject.FindProperty("ratio");
        m_randomRatio = serializedObject.FindProperty("randomRatio");
        m_enemyCount = serializedObject.FindProperty("enemyCount");

        //base.OnInspectorGUI();
        EditorGUILayout.PropertyField(m_Enemies, includeChildren: true);

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(m_enemyCount, 
        new GUIContent("Enemy count", "Leave 0 if you want a dynamic wave which will scale enemy number based on % (100%)"));
        if (GUILayout.Button("Reset"))
        {
            m_enemyCount.intValue = 0;
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        m_randomRatio.boolValue = EditorGUILayout.Toggle("Random ratio", m_randomRatio.boolValue);

        if (m_randomRatio.boolValue)
        {
            randomizeButton = GUILayout.Button("Randomize");
            //if (randomizeButton)
            //{
            //    Randomize();
            //}
        }
        EditorGUILayout.EndHorizontal();
        m_Ratio.arraySize = m_Enemies.arraySize;

        DrawRatio();
        
        serializedObject.ApplyModifiedProperties();
    }

    void DrawRatio()
    {
        if (m_enemyCount.intValue > 0)
        {
            currMax = m_enemyCount.intValue;
        }
        else if (m_enemyCount.intValue <= 0)
        {
            currMax = max;
        }

        int sum = 0;
        if (m_randomRatio.boolValue && randomizeButton)
        {
            Randomize();
        }
        else
        {
            for (int i = 0; i < m_Ratio.arraySize; i++)
            {
                sum += m_Ratio.GetArrayElementAtIndex(i).intValue;
            }
            current = currMax - sum;
        }
        EditorGUILayout.TextField(current.ToString());
        for (int i = 0; i < m_Ratio.arraySize; i++)
        {
            m_Ratio.GetArrayElementAtIndex(i).intValue = EditorGUILayout.IntSlider(
            "" + m_Enemies.GetArrayElementAtIndex(i).objectReferenceValue.name,
            m_Ratio.GetArrayElementAtIndex(i).intValue,
            0, m_Ratio.GetArrayElementAtIndex(i).intValue + current);
        }
    }

    void Randomize()
    {
        int[] randomNums = new int[m_Ratio.arraySize];

        for(int i = 0; i < randomNums.Length; i++)
        {
            randomNums[i] = UnityEngine.Random.Range(0, currMax + 1);
        }
        //Scale factor to make number sum;
        float factor = (float)currMax / randomNums.Sum();

        string s = "";
        foreach(var v in randomNums)
        {
            s = s + ", " + v.ToString();
        }

        Debug.Log(s);

        string tm = "";
        string rounded = "";
        string rounded_2 = "";
        for(int i = 0; i < randomNums.Length; i++)
        {
            float tmp = (float)randomNums[i];
            tmp *= factor;
            tm = tm + " " + tmp.ToString();
            rounded = rounded + "; " + Math.Round(tmp, 2, MidpointRounding.AwayFromZero);
            rounded_2 = rounded_2 + "; " + Mathf.RoundToInt(tmp);
            randomNums[i] = Mathf.RoundToInt(tmp);
        }
        Debug.Log(tm);
        Debug.Log(rounded);
        Debug.Log(rounded_2);

        int sum = 0;

        for (int i = 0; i < m_Ratio.arraySize; i++)
        {
            sum += randomNums[i];
            m_Ratio.GetArrayElementAtIndex(i).intValue = randomNums[i];
            
        }

        current = sum;
    }
}
