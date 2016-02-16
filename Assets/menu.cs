using UnityEngine;
using System.Collections;

public class menu : MonoBehaviour {

	private AdMob admob = new AdMob();

	// Use this for initialization
	void Start () {
		this.admob.requestBanner();
		this.admob.requestBannerInterstitial();

		int best_score = PlayerPrefs.GetInt( "best_score" );
		int last_score = PlayerPrefs.GetInt( "last_score" );

		GameObject info = GameObject.Find("info");

		if(info  && (best_score>0 || last_score > 0))
		{	
			info.GetComponent<TextMesh>().text = "Best: " + best_score + " Last: " + last_score;
		}

	}
	
	// Update is called once per frame
	void Update () {
		this.admob.showInterstitial();
		this.admob.showBanners();
	}

	void OnMouseDown()
	{
		transform.GetComponent<TextMesh>().color = Color.black;

		if(transform.name == "quit")
		{
			Application.Quit();
		}
		if(transform.name == "start")
		{
			this.admob.hideBanners();

			Application.LoadLevel("main");
		}
		if(transform.name == "instructions")
		{
			Application.LoadLevel("instructions");
		}
		if(transform.name == "back")
		{
			Application.LoadLevel("menu");
		}
	}
}
