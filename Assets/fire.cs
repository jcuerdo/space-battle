using UnityEngine;
using System.Collections;

public class fire : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D collision) 
	{
		if( collision.gameObject.name.StartsWith( "enemy" )  )
		{
			GameObject text = GameObject.Find("points");
			if( !collision.GetComponent<Collider2D>().isTrigger ){
			int points = int.Parse(text.GetComponent<GUIText>().text);
			points++;
			text.GetComponent<GUIText>().text = points + "";		
			}
			collision.enabled = false;
		}
	}
}
