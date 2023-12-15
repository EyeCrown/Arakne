using UnityEngine;
using System.Collections;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

// @kurtdekker - ultra Cheesy grid with in-built editor for Unity3D
//
// To use:
//	make an empty game object
//	drag this script on it
// 	make a prefab out of it
//	select the prefab to edit the grid
//
// now you can cheesy-easy edit a smallish grid in the Unity editor window
//
public class CheesyGrid : MonoBehaviour
{
    [SerializeField] private GameObject spider;
    [SerializeField] private GameObject mosquito;
    [SerializeField] private GameObject centipede;


    [Header("Cell values cycle from 0 to this-1")]
    public int MaxValue = 3;

    [Header("Actual saved payload. Use GetCell(x,y) to read!")]
    [Header("WARNING: changing this will nuke your data!")]
    public string data;

    public int across = 5;
    public int down = 30;

    // stretch goals for you:
    // TODO: make an array of colors perhaps?
    // TODO: make a color mapper??
    // TODO: map above characters to graphics??

    // for you to get stuff out of the grid to use in your game
    public string GetCell(int x, int y)
    {
        int n = GetIndex(x, y);
        return data.Substring(n, 1);
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        if (data == null || data.Length != (across * down))
        {
            Undo.RecordObject(this, "Resize");

            if (across < 1) across = 1;
            if (down < 1) down = 1;

            // make a default layout
            data = "";
            for (int y = 0; y < down; y++)
            {
                for (int x = 0; x < across; x++)
                {
                    string cell = "0";

                    if (x == 0 || y == 0 || x == (across - 1) || y == (down - 1))
                    {
                        cell = "1";
                    }
                    if (x == 1 && y == 1)
                    {
                        cell = "2";
                    }
                    if (x == (across - 2) && y == (down - 2))
                    {
                        cell = "3";
                    }
                    data = data + cell;
                }
            }

            EditorUtility.SetDirty(this);
        }
    }
    void Reset()
    {
        OnValidate();
    }
#endif

    int GetIndex(int x, int y)
    {
        if (x < 0) return -1;
        if (y < 0) return -1;
        if (x >= across) return -1;
        if (y >= down) return -1;
        return x + y * across;
    }

    void ToggleCell(int x, int y)
    {
        int n = GetIndex(x, y);
        if (n >= 0)
        {
            var cell = data.Substring(n, 1);

            int c = 0;
            int.TryParse(cell, out c);
            c++;
            if (c >= MaxValue) c = 0;

            cell = c.ToString();

#if UNITY_EDITOR
            Undo.RecordObject(this, "Toggle Cell");
#endif
            // reassemble
            data = data.Substring(0, n) + cell + data.Substring(n + 1);
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(CheesyGrid))]
    public class CheesyGridEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var grid = (CheesyGrid)target;

            EditorGUILayout.BeginVertical();

            GUILayout.Label("WARNING: Save and commit your prefab/scene OFTEN!");

            for (int y = 0; y < grid.down; y++)
            {
                GUILayout.BeginHorizontal();
                for (int x = 0; x < grid.across; x++)
                {
                    int n = grid.GetIndex(x, y);

                    var cell = grid.data.Substring(n, 1);

                    // hard-coded some cheesy color map - improve it by all means!
                    GUI.color = Color.gray;
                    if (cell == "1") GUI.color = Color.white;
                    if (cell == "2") GUI.color = Color.red;
                    if (cell == "3") GUI.color = Color.green;

                    if (GUILayout.Button(cell, GUILayout.Width(20)))
                    {
                        grid.ToggleCell(x, y);
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUI.color = Color.yellow;

            GUILayout.Label("DANGER ZONE BELOW THIS AREA!");

            GUI.color = Color.white;

            EditorGUILayout.EndVertical();

            DrawDefaultInspector();
        }
    }
#endif

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < down; i++)
        {
            for(int j = 0; j < 5; j++)
            {
                if (GetCell(j, i) == "1")
                {
                    Instantiate(spider, new Vector3((j - 2) * 3, 15, 0), Quaternion.identity);
                }

                if (GetCell(j, i) == "2")
                {
                    Instantiate(mosquito, new Vector3((j - 2) * 3, 15, 0), Quaternion.identity);
                }
            }

            if(i == 60)
            {
                GameObject precentipede;
                precentipede = Instantiate(centipede, new Vector3(4, 15, 4), Quaternion.identity);
                precentipede.GetComponentInChildren<SpriteRenderer>().material.color = new Color(1f,1f,1f,0.6f);
                precentipede.transform.localScale = precentipede.transform.localScale * 0.6f;
            }

            if(i == 100)
            {
                Instantiate(centipede, new Vector3(0, 15, 0), Quaternion.identity);
            }

            if (i == 142)
            {
                Instantiate(centipede, new Vector3(0, 15, 0), Quaternion.identity);
            }

            if(i == 180)
            {
               // GameManager.Instance.GameOver();
            }

            yield return new WaitForSeconds(1f);
        }
    }
}
