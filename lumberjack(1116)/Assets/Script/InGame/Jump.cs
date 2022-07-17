using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour {
    Vector2 pos;
    public float JumpPower;

    public float ray_Distance = 1.5f;
	public Vector3 ray_Pos;
    Rigidbody2D rigidbody;
    Physics2D physics;

    int layer = 9;
    public bool On_Jump = false;

	GameManager GM;
    // Use this for initialization

    void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
		GM = GameObject.Find("GameManager").GetComponent<GameManager>();


		layer = ~layer;
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {

		if(Input.GetButton("Jump") && (GM.GetGameState() == "Play" || GM.GetGameState() == "MonsterBattle"))
        {
            Player_Jump();
        }

		if (On_Jump)
        {
			if(rigidbody.velocity.y < 0)
			{
				RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, ray_Distance, layer);
                Debug.DrawRay(transform.position, Vector2.down * ray_Distance, Color.yellow);
                if (hit)
	            {
	                if (hit.collider.tag == "Plan")
	                {
	                    GetComponent<Animator>().SetBool("OnJump", false);
	                    On_Jump = false;
						rigidbody.velocity = Vector2.zero;
	                }
	            }
			}
        }
    }

    public void Player_Jump()
    {
		RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, ray_Distance, layer);
        Debug.DrawRay(transform.position, Vector2.down * ray_Distance, Color.yellow);
        if (hit)
        {
            if (hit.collider.tag == "Plan" && !On_Jump)
            {
                rigidbody.velocity = Vector2.zero;

                rigidbody.AddForce(new Vector2(0, JumpPower));
                GetComponent<Animator>().SetBool("OnJump", true);
                On_Jump = true;
            }
        }

    }
}