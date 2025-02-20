using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class TextSpawner : MonoBehaviour
{
    [SerializeField] private GameObject textPrefab;
    [SerializeField] private int startingLayer;
    [SerializeField] private Color color;
    [SerializeField] private float timeToExist = 3;
    [SerializeField] private Vector3 direction = Vector3.up;
    [SerializeField] private List<TimerScript> timerList = new List<TimerScript>();
    [SerializeField] private List<TextMeshPro> textList = new List<TextMeshPro>();
    [HideInInspector] public string textToWriteInDebug;
    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < timerList.Count; ++i)
        {
            timerList[i].Update();
        }
    }
    public void CreateText(string text)
    {
        TextMeshPro textObject = Instantiate(textPrefab, transform.position, Quaternion.identity, transform).GetComponent<TextMeshPro>();

        textObject.text = text;
        textObject.color = color;
        textObject.sortingOrder = (textList.Count == 0) ? startingLayer : textList[textList.Count-1].sortingOrder + 1;
        textList.Add(textObject);

        TimerScript newTimer = new TimerScript();

        newTimer.runOnUpdate += () => UpdateObject(newTimer, textObject);
        newTimer.runOnFinish += () => FinishObject(newTimer, textObject);
        timerList.Add(newTimer);
        newTimer.Start(timeToExist);
    }
    private void UpdateObject(TimerScript timer, TextMeshPro textMesh)
    {
        Color newColor = textMesh.color;
        newColor.a = 1 - timer.Progress();
        textMesh.color = newColor;
        textMesh.transform.position = Vector3.Lerp(transform.position, transform.position + transform.TransformDirection(direction), timer.Progress());
    }
    private void FinishObject(TimerScript timer, TextMeshPro textMesh)
    {
        timerList.Remove(timer);
        textList.Remove(textMesh);
        Destroy(textMesh.gameObject);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(TextSpawner))]
public class TextSpawner_Editor : Editor
{
    string textToWrite = "TextToWrite";
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        TextSpawner pointer = (TextSpawner)target;

        textToWrite = GUILayout.TextField(textToWrite);
        if (GUILayout.Button("CreateNewText"))
        {
            pointer.CreateText(textToWrite);
        }
    }
}

#endif
