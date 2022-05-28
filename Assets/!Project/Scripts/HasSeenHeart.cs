using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasSeenHeart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("HasSeenHeart", 1);
        PlayerPrefs.Save();
    }
}
