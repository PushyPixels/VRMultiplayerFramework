using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartCheck : MonoBehaviour
{
    public GameObject heart;

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetInt("HasSeenHeart",0) == 1)
        {
            heart.SetActive(true);
        }
    }
}
