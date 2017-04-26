using UnityEngine;
using System.Collections;

public class GameStart : MonoBehaviour {

	// Use this for initialization
	void Start () {
        cMainApp.Instance.Init();
	}
	
	// Update is called once per frame
	void Update () {
        cMainApp.Instance.Process();
	}
}
