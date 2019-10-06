using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotPool : MonoBehaviour
{
    public static ShotPool shotPoolInstance;
    [SerializeField]
    private GameObject pooledShot;
    private bool notEnoughShotsInPool = true;
    private List<GameObject> shots;
    private void Awake()
    {
        shotPoolInstance = this;
    }
    void Start()
    {
        shots = new List<GameObject>();
    }
    public GameObject GetShot()
    {
        if (shots.Count > 0)
        {
            for (int i = 0; i < shots.Count; i++)
            {
                if (!shots[i].activeInHierarchy)
                {
                    return shots[i];
                }
            }
        }
        if (notEnoughShotsInPool)
        {
            GameObject st = Instantiate(pooledShot);
            st.SetActive(false);
            shots.Add(st);
            return st;
        }
        return null;
    }

}
