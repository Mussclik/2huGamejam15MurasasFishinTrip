using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class ListObjectBase : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI itemName;
    [SerializeField] protected TextMeshProUGUI description;
    [SerializeField] protected Image spriteRenderer;
    [SerializeField] public Button button;
    [SerializeField] protected TextMeshProUGUI buttonText;
    protected PlayerMovement Player
    {
        get { return PlayerMovement.instance; }
    }

    public virtual void Start()
    {
        button.onClick.AddListener(OnActivation);
    }
    public abstract void OnActivation();
    public abstract void VisualUpdate();
}
