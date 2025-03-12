using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encyclopedia : MonoBehaviour
{
    public static Encyclopedia instance;
    private int pageNumber = 0;
    [SerializeField] private EncyclopediaGameobject leftPage;
    [SerializeField] private EncyclopediaGameobject rightPage;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
    public void TurnPage(int idOfLeftPage)
    {

    }
}
