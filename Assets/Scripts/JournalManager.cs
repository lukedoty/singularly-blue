using UnityEngine;
using System.Collections.Generic;

public class JournalManager : MonoBehaviour
{

    public Dictionary<Artifact, JournalElement> Elements;

    private void Awake()
    {
        Elements = new Dictionary<Artifact, JournalElement>();
    }

    void Start()
    {
        JournalElement[] j = GetComponentsInChildren<JournalElement>();
        for (int i = 0; i < j.Length; i++)
        {
            Elements.Add(j[i].Artifact, j[i]);
        }
    }

}
