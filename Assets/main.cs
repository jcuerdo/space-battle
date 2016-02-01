using UnityEngine;
using System.Collections;

public class main : MonoBehaviour {
	private float scale = 15f;
	private float x_limit = 7.3f;
	private float fire_speed = 100;
	private float last_enemy;
	private float time_between_enemy = 0.7f;
	private float last_rock;
	private float time_between_rock = 1.7f;
	private int enemy_speed = 7;
	private int rock_speed = 7;
	private int lives = 3;
	// Use this for initialization
	void Start () {
		this.last_enemy = Time.timeSinceLevelLoad;
		this.last_rock = Time.timeSinceLevelLoad;
	}
	
	// Update is called once per frame
	void Update () {
		this.movement();
		this.fire();
		this.createEnemy();
		this.createRock();
	}


	private void createEnemy()
	{
		if (Time.timeSinceLevelLoad - this.last_enemy > time_between_enemy) {
			GameObject enemy = GameObject.Find("enemy");
			Vector2 new_enemy_position = enemy.GetComponent<Rigidbody2D>().position;

			float x = Random.Range( -x_limit,x_limit );
			new_enemy_position.x = x;
			GameObject new_enemy = (GameObject) Instantiate( enemy,new_enemy_position, Quaternion.identity );
			new_enemy.GetComponent<Rigidbody2D>().velocity = Vector2.up * (-this.enemy_speed);
			new_enemy.GetComponent<Collider2D>().isTrigger = false;
			Texture2D texture = Resources.Load("enemigo") as Texture2D;
			SpriteRenderer[] components = new_enemy.GetComponents<SpriteRenderer>();
			foreach(SpriteRenderer c in components)
			{
				c.sprite = Sprite.Create(texture,c.sprite.rect,new Vector2(0.5f,0.5f));
			}
			this.last_enemy = Time.timeSinceLevelLoad;
		}

	}

	private void createRock()
	{
		if (Time.timeSinceLevelLoad - this.last_rock > time_between_rock) {
			GameObject rock = GameObject.Find("rock");
			Vector2 new_rock_position = rock.GetComponent<Rigidbody2D>().position;
			
			float x = Random.Range( -x_limit,x_limit );
			new_rock_position.x = x;
			GameObject new_rock = (GameObject) Instantiate( rock,new_rock_position, Quaternion.identity );
			new_rock.GetComponent<Rigidbody2D>().velocity = Vector2.up * (-this.enemy_speed);
			new_rock.GetComponent<Collider2D>().isTrigger = false;
			Texture2D texture = Resources.Load("rock") as Texture2D;
			SpriteRenderer[] components = new_rock.GetComponents<SpriteRenderer>();
			foreach(SpriteRenderer c in components)
			{
				c.sprite = Sprite.Create(texture,c.sprite.rect,new Vector2(0.5f,0.5f));
			}
			this.last_rock = Time.timeSinceLevelLoad;
		}
		
	}

	private void movement()
	{
		float x;
		if (Application.platform != RuntimePlatform.Android){
			x = Input.GetAxis("Horizontal") * scale * Time.deltaTime; 
		}
		else{
			x = Input.acceleration.x;
		}
		if( Mathf.Abs(transform.position.x + x) < (x_limit) ){
			transform.Translate(x, 0, 0); 
		}
	}

	private void fire()
	{
		if( Input.GetMouseButtonDown(0) )
		{
			doFire();
			return;
		}
		foreach (Touch touch in Input.touches) {
			if (touch.phase == TouchPhase.Ended){
				doFire();
			}
		}
	}

	private void doFire()
	{
		Vector2 plane_position = transform.position;
		Vector2 fire_position = new Vector2 ( plane_position.x,-0.7f );
		GameObject fire_bullet = GameObject.Find("fire");
		GameObject new_fire = (GameObject) Instantiate( fire_bullet,fire_position, Quaternion.identity );
		new_fire.GetComponent<Rigidbody2D>().velocity = Vector2.up;
		new_fire.GetComponent<Rigidbody2D>().velocity = new_fire.GetComponent<Rigidbody2D>().velocity * this.fire_speed;
	}
	
	void OnTriggerEnter2D(Collider2D collider) 
	{
		if (collider.gameObject.name.StartsWith ("enemy")||collider.gameObject.name.StartsWith ("rock")) {
			this.receibeAttack(collider);
		}
	}

	private void receibeAttack(Collider2D collider){
		this.destroyEnemy(collider);
		this.lives--;
		if (lives > 0) {
			GameObject text = GameObject.Find("lives");
			text.GetComponent<GUIText>().text = lives + "";
			transform.localScale = new Vector2(transform.localScale.x + 0.05f,transform.localScale.y + 0.05f);
		} 
		else {
			GetComponent<Rigidbody2D>().transform.rotation = new Quaternion(1,1,1,1);
			GetComponent<Rigidbody2D>().velocity = new Vector2(10,15);
			Application.LoadLevel(0);
		}

	}

	private void destroyEnemy(Collider2D collider){

	}
}
