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
	private BannerView bannerView;
	private InterstitialAd interstitial;
	// Use this for initialization
	void Start () {
		this.last_enemy = Time.timeSinceLevelLoad;
		this.last_rock = Time.timeSinceLevelLoad;
		//RequestBanner();
		RequestBannerInter();
	}
	
	// Update is called once per frame
	void Update () {
		if(started){
			this.movement();
			this.fire();
			this.createEnemy();
			this.createRock();
		}
		else{
			ShowInterstitial()
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
		this.lives--;
		if (lives > 0) {
			GameObject text = GameObject.Find("lives");
			text.GetComponent<GUIText>().text = lives + "";
			transform.localScale = new Vector2(transform.localScale.x + 0.05f,transform.localScale.y + 0.05f);
		} 
		else {
			GetComponent<Rigidbody2D>().transform.rotation = new Quaternion(1,1,1,1);
			GetComponent<Rigidbody2D>().velocity = new Vector2(10,15);
			this.over = true;
		}

	}

	void OnGUI()
	{
		GUIStyle text_style =  new GUIStyle();
		text_style.fontSize = Screen.width/20;
		text_style.alignment = TextAnchor.MiddleCenter;
		text_style.normal.textColor = Color.white;
		
		
		GUIStyle button_style = new GUIStyle(GUI.skin.button);
		button_style.fontSize = Screen.width/20;
		
		Texture2D left = (Texture2D)(Resources.Load( "left" ));
		Texture2D right = (Texture2D)(Resources.Load( "right" ));
		if ( !this.started )
		{
			int best_score = PlayerPrefs.GetInt( "best_score" );
			int last_score = PlayerPrefs.GetInt( "last_score" );
			
			GUI.Box(new Rect (Screen.width/4,Screen.height/4 - 5, Screen.width/2 , Screen.height/2 ), "" );
			GUI.Box(new Rect( Screen.width/2 - Screen.width/6,Screen.height/4,Screen.width/2 - Screen.width/6,Screen.height/8), "Best: " + best_score + " Last: " + last_score, text_style );
			if( GUI.Button(new Rect( Screen.width/2 - Screen.width/6,Screen.height/4 + (Screen.height/8) ,Screen.width/2 - Screen.width/6,Screen.height/8), "Start Game",button_style )) 
			{
				this.started = true;
			}
			if( GUI.Button(new Rect( Screen.width/2 - Screen.width/6,Screen.height/4 + (Screen.height/8*2) + 20,Screen.width/2 - Screen.width/6,Screen.height/8), "Quit",button_style )) 
			{
				Application.Quit();
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

	private void RequestBanner()
	{

		#if UNITY_ANDROID
		string adUnitId = "ca-app-pub-6904186947817626/6737471590";
		#elif UNITY_IPHONE
		string adUnitId = "ca-app-pub-6904186947817626/6737471590";
		#else
		string adUnitId = "ca-app-pub-6904186947817626/6737471590";
		#endif

		bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
		AdRequest request = new AdRequest.Builder().Build();
		bannerView.LoadAd(request);

	}

	private void RequestBannerInter()
	{
		
		#if UNITY_ANDROID
		string adUnitId = "ca-app-pub-6904186947817626/8284077196";
		#elif UNITY_IPHONE
		string adUnitId = "ca-app-pub-6904186947817626/8284077196";
		#else
		string adUnitId = "ca-app-pub-6904186947817626/8284077196";
		#endif

		interstitial = new InterstitialAd(adUnitId);
		AdRequest request = new AdRequest.Builder().Build();
		interstitial.LoadAd(request);
	}

	private void ShowInterstitial() { 
		if (interstitial.IsLoaded()) { 
			interstitial.Show(); 
		}
		
	}



}
