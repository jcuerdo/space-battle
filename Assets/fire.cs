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

			Texture2D texture = Resources.Load("enemy_killed") as Texture2D;
			SpriteRenderer[] components = collision.GetComponents<SpriteRenderer>();
			foreach(SpriteRenderer c in components)
			{
				c.sprite = Sprite.Create(texture,c.sprite.rect,new Vector2(0.5f,0.5f));
			}
			transform.rigidbody2D.position = new Vector2(10f,10f);
			GameObject text = GameObject.Find("points");
			if( !collision.collider2D.isTrigger ){
			int points = int.Parse(text.guiText.text);
			points++;
			text.guiText.text = points + "";		
			}
			collision.enabled = false;
		}
	}
}
