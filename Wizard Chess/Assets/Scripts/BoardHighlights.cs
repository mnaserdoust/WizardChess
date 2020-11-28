﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardHighlights : MonoBehaviour
{
    public static BoardHighlights Instance { set; get; }
    public GameObject highlightPrefab;
    private List<GameObject> highlights;

    private const float TILE_OFFSET = .5f;

    private void Start()
    {
        
        Instance = this;
        highlights = new List<GameObject>();
    }

    private GameObject getHighlightObject()
    {
        GameObject go = highlights.Find(g => !g.activeSelf);
        if(go == null)
        {
            go = Instantiate(highlightPrefab);
            highlights.Add(go);
        }

        return go;
    }

    public void highlightAllowedMoves(bool[,] moves)
    {
        for(int i=0;i<8;i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if(moves[i,j])
                {
                    GameObject go = getHighlightObject();
                    go.SetActive(true);
                    go.transform.position = new Vector3(i+TILE_OFFSET, 0, j+TILE_OFFSET);
                }
            }
        }
    }

    public void hideHighlights()
    {
        foreach (GameObject go in highlights)
            go.SetActive(false);
    }
}
