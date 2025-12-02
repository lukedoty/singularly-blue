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
        JournalElement[] j = transform.GetChild(0).GetComponentsInChildren<JournalElement>();
        for (int i = 0; i < j.Length; i++)
        {
            if (j[i].Artifact == null) continue;
            Elements.Add(j[i].Artifact, j[i]);
        }
    }
}
