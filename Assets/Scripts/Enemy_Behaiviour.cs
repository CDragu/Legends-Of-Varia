using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Behaiviour : MonoBehaviour {

    public bool soldier, shield, archer;
    public float speed;
    public AnimationCurve speedCurve;
    public float HP;
    GameObject player;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectsWithTag("Player")[0];
	}
	
	// Update is called once per frame
	void Update () {
		if(soldier == true)
        {
            Soldier_Action();
        }
        if(shield == true)
        {

        }
        if(archer == true)
        {

        }
        if(HP < 0)
        {
            Destroy(this.gameObject);
        }
	}

    void Soldier_Action()
    {
        float step = speed * Time.deltaTime * speedCurve.Evaluate(Time.time);
        this.transform.LookAt(new Vector3(player.transform.position.x, this.transform.position.y, player.transform.position.z));
        this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(player.transform.position.x, this.transform.position.y, player.transform.position.z), step);
        
    }
}
