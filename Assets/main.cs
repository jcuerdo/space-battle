using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api;

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
	private bool started = false;
	private bool over = false;
	private AdMob admob = new AdMob();
	private Controll controll = new Controll();
	private float timeLastMessage;
	private bool instructions = false;

	private const string TWITTER_ADDRESS = "http://twitter.com/intent/tweet";
	private const string TWEET_LANGUAGE = "en"; 

	void ShareToTwitter (string textToDisplay)
	{
		Application.OpenURL(TWITTER_ADDRESS +
			"?text=" + WWW.EscapeURL(textToDisplay) +
			"&amp;lang=" + WWW.EscapeURL(TWEET_LANGUAGE));
	}
	
	void Start () {
		this.last_enemy = Time.timeSinceLevelLoad;
		this.last_rock = Time.timeSinceLevelLoad;
		this.timeLastMessage = 0;
		this.admob.requestBanner();
		this.admob.requestBannerInterstitial();
		this.initScoresPosition();
	}
	
	
	void Update () {
		if(started){
			this.admob.hideBanners();
			this.controll.movement(transform, scale, x_limit);
			this.controll.fire(transform,fire_speed);
			this.createEnemy();
			this.createRock();
			this.clearMessages();
		}
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
	
	void OnTriggerEnter2D(Collider2D collider) 
	{
		if (collider.gameObject.name.StartsWith ("enemy")||collider.gameObject.name.StartsWith ("rock")) {
			this.receibeAttack(collider);
		}
	}
	
	private void receibeAttack(Collider2D collider){
		this.lives--;
		this.hitMessage();
		Handheld.Vibrate();
		GameObject text = GameObject.Find("lives");
		text.GetComponent<GUIText>().text = lives + "";
		transform.localScale = new Vector2(transform.localScale.x + 0.05f,transform.localScale.y + 0.05f);
		if (lives == 0) {
			GetComponent<Rigidbody2D>().transform.rotation = new Quaternion(1,1,1,1);
			GetComponent<Rigidbody2D>().velocity = new Vector2(10,15);
			this.over = true;
		}
	}
	
	void OnGUI()
	{
		GUIStyle instructions_style =  new GUIStyle();
		instructions_style.fontSize = Screen.width/40;
		instructions_style.normal.textColor = Color.white;

		GUIStyle text_style =  new GUIStyle();
		text_style.fontSize = Screen.width/30;
		text_style.alignment = TextAnchor.MiddleCenter;
		text_style.normal.textColor = Color.white;
		
		
		GUIStyle button_style = new GUIStyle(GUI.skin.button);
		button_style.fontSize = Screen.width/30;
		
		Texture2D left = (Texture2D)(Resources.Load( "left" ));
		Texture2D right = (Texture2D)(Resources.Load( "right" ));
		if ( !this.started )
		{
			this.admob.showInterstitial();
			this.admob.showBanners();
			int best_score = PlayerPrefs.GetInt( "best_score" );
			int last_score = PlayerPrefs.GetInt( "last_score" );
			if(this.instructions){
				GUI.Box(new Rect (Screen.width/4,Screen.height/4 - 5, Screen.width/2 , Screen.height/2f ), "" );
				GUI.Box(new Rect( Screen.width/2 - Screen.width/5,Screen.height/4,Screen.width/2 - Screen.width/6,Screen.height/6), "Move your phone to move your triangle  \n\r 'green' and press the screen to shoot. \n\rYou can kill red enemies, but you cant \n\r kill black ones, you can only dodge.",instructions_style );
				if( GUI.Button(new Rect( Screen.width/2 - Screen.width/6,Screen.height/5 + (Screen.height/3) ,Screen.width/2 - Screen.width/6,Screen.height/8), "Back to menu",button_style )) 
				{
					this.instructions = false;
				}
			}
			else
			{
			GUI.Box(new Rect (Screen.width/4,Screen.height/4 - 5, Screen.width/2 , Screen.height/1.5f ), "" );
			GUI.Box(new Rect( Screen.width/2 - Screen.width/6,Screen.height/4,Screen.width/2 - Screen.width/6,Screen.height/8), "Best: " + best_score + " Last: " + last_score, text_style );
			if( GUI.Button(new Rect( Screen.width/2 - Screen.width/6,Screen.height/4 + (Screen.height/8) ,Screen.width/2 - Screen.width/6,Screen.height/8), "Start Game",button_style )) 
			{
				this.started = true;
			}
			if( GUI.Button(new Rect( Screen.width/2 - Screen.width/6,Screen.height/4 + (Screen.height/8*2) + 20,Screen.width/2 - Screen.width/6,Screen.height/8), "Quit",button_style )) 
			{
				Application.Quit();
			}
			if( GUI.Button(new Rect( Screen.width/2 - Screen.width/6,Screen.height/4 + (Screen.height/8*2) + 120,Screen.width/2 - Screen.width/6,Screen.height/8), "Instructions",button_style )) 
			{
				this.instructions = true;
			}
			}
		}
		else
		{
			if( this.over )
			{
				this.setScoreRecord();
				if( GUI.Button(new Rect( Screen.width/2 - Screen.width/6,Screen.height/2 - Screen.height/6,Screen.width/2 - Screen.width/6,Screen.height/8), "Back to menu",button_style )) 
				{
					Application.LoadLevel(0);
				}
			}
		}
	}
	
	private void setScoreRecord()
	{
		int best_score = PlayerPrefs.GetInt( "best_score" );
		GameObject pointsTxt = GameObject.Find("points");
		int points = int.Parse(pointsTxt.GetComponent<GUIText>().text);
		
		if ( points >= best_score )
		{
			PlayerPrefs.SetInt( "best_score", points);
		}
		PlayerPrefs.SetInt( "last_score", points );
	}

	private void hitMessage()
	{
		GameObject message = GameObject.Find("message");
		Vector3 messageNewPosition = Camera.main.transform.position;
		messageNewPosition.x = messageNewPosition.x + Screen.width / 4500f;
		messageNewPosition.y = messageNewPosition.y - Screen.height / 2500f;

		message.transform.position = messageNewPosition;

		GameObject lives = GameObject.Find("lives");

		message.GetComponent<GUIText>().text = "HIT " + this.lives + " lives left";

		this.timeLastMessage = Time.timeSinceLevelLoad;
	}

	private void clearMessages()
	{
		if(this.timeLastMessage + 1 < Time.timeSinceLevelLoad)
		{
			GameObject message = GameObject.Find("message");
			message.transform.position = new Vector3(-1f,-1f,-1f);			
		}
	}

	private void initScoresPosition()
	{
		GameObject points = GameObject.Find("points");
		GameObject lives = GameObject.Find("lives");

		Vector2 pointsNewPosition = Camera.main.transform.position;
		Vector2 livesNewPosition = Camera.main.transform.position;

		pointsNewPosition.y = pointsNewPosition.y -  (Screen.height / 14000f);
		livesNewPosition.y = livesNewPosition.y - (Screen.height / 4900f);

		pointsNewPosition.x = pointsNewPosition.x + (Screen.width / 12000f);
		livesNewPosition.x = livesNewPosition.x + (Screen.width / 12000f);

		points.gameObject.transform.position = pointsNewPosition;
		lives.gameObject.transform.position = livesNewPosition;
	}
}


