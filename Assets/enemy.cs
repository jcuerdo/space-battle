using UnityEngine;
using System.Collections;

public class enemy : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D collision) 
	{
		if( collision.gameObject.name.StartsWith( "fire" ) || collision.gameObject.name.StartsWith( "main" )  )
		{
			GetComponent<Rigidbody2D>().transform.rotation = new Quaternion(1,1,1,1);
			GetComponent<Rigidbody2D>().velocity = new Vector2(10,15);

		}
	}
}
