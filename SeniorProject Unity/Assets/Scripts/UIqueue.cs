using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIqueue : MonoBehaviour {
	Renderer render;
	public GameObject ui;
	private Text uiText;
	public float textDistance = 3.0f;

	// Use this for initialization
	void Start () {
		render = gameObject.GetComponent<Renderer>();
		uiText = ui.GetComponent<Text>();
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	public bool lookingAtMe(float lookAtDist)
	{
		if (lookAtDist <= textDistance)
		{
			uiText.enabled = true;
			return true;
		}
		else
		{
			uiText.enabled = false;
			return false;
		}
	}
}
