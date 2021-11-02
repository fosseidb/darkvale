using UnityEditor;
using UnityEngine;

public class Developer
{
    [MenuItem("Developer/Clear Player Prefs")]
    public static void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        // clear serialized saves

        Debug.Log("Player Prefs cleared");
    }

    [MenuItem("Developer/Econ/Give 100 GP")]
    public static void GivePlayer100GP()
    {
        
        Debug.Log("TODO: Give 100 GP");
    }

    [MenuItem("Developer/Econ/Give 100 XP")]
    public static void GivePlayer100XP()
    {

        Debug.Log("TODO: Give 100 XP");
    }

    [MenuItem("Developer/Set Stage/Cemetary")]
    public static void SetStageCemetary()
    {

        Debug.Log("TODO: Set Stage Cemetary");
    }


    [MenuItem("Developer/Set Stage/Forest")]
    public static void SetStageForest()
    {

        Debug.Log("TODO: Set Stage Forest");
    }


    [MenuItem("Developer/Set Stage/Siege")]
    public static void SetStageSiege()
    {

        Debug.Log("TODO: Set Stage Siege");
    }


    [MenuItem("Developer/Set Stage/Assault")]
    public static void SetStageAssault()
    {

        Debug.Log("TODO: Set Stage Assault");
    }

}
