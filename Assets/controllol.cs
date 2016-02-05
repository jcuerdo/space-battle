using UnityEngine;
using System.Collections;

public class Controll : MonoBehaviour {

	public void movement(Transform transform, float scale, float xLimit)
	{
		float x;
		if (Application.platform != RuntimePlatform.Android){
			x = Input.GetAxis("Horizontal") * scale * Time.deltaTime; 
		}
		else{
			x = Input.acceleration.x;
		}
		if( Mathf.Abs(transform.position.x + x) < (xLimit) ){
			transform.Translate(x, 0, 0); 
		}
	}
	
	public void fire(Transform transform, float fireSpeed)
	{
		if( Input.GetMouseButtonDown(0) )
		{
			doFire(transform, fireSpeed);
			return;
		}
		foreach (Touch touch in Input.touches) {
			if (touch.phase == TouchPhase.Ended){
				doFire(transform, fireSpeed);
			}
		}
	}

	private void doFire(Transform transform, float fireSpeed)
	{
		Vector2 plane_position = transform.position;
		Vector2 fire_position = new Vector2 ( plane_position.x,-0.7f );
		GameObject fire_bullet = GameObject.Find("fire");
		GameObject new_fire = (GameObject) Instantiate( fire_bullet,fire_position, Quaternion.identity );
		new_fire.GetComponent<Rigidbody2D>().velocity = Vector2.up;
		new_fire.GetComponent<Rigidbody2D>().velocity = new_fire.GetComponent<Rigidbody2D>().velocity * fireSpeed;
	}

}
