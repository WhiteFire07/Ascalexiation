using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutscenePlayer : MonoBehaviour
{
    public List<GameObject> panels = new List<GameObject>();
    public GameObject currentPanel;
    public PlayerController pcont;
    public float index;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < transform.childCount; i++) {
            panels.Add(transform.GetChild(i).gameObject);
        }
        index = 0;
        currentPanel = panels[(int)index];
    }

    // Update is called once per frame
    void Update() {
        if(pcont.jump.triggered) {
            nextPanel();
            if(index == transform.childCount) {
                pcont.unFreeze();
                gameObject.SetActive(false);
            }
        }
    }
    
    void nextPanel() {
        currentPanel.SetActive(false);
        index++;
        if(index < transform.childCount){
            currentPanel = panels[(int)index];
            currentPanel.SetActive(true);
        }
    }
}
