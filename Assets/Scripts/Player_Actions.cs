using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player_Actions : MonoBehaviour {

    List<GameObject> AllPillars = new List<GameObject>();
    List<GameObject> Pillars = new List<GameObject>();
    List<GameObject> Markers = new List<GameObject>();
    public GameObject CurrentPillar, TargetPillar;
    public CapsuleCollider HitArea;
    public bool OnTheMove = false;
    private float InternalSpeed;
    float journeyLength, startTime;
    public GameObject model;
    public GameObject Marker;
    public float Speed;
    public float JumpRange;
    public int HP;
    
    void Start () {
        HP = 10;
        InternalSpeed = Speed;
        AllPillars = new List<GameObject>(GameObject.FindGameObjectsWithTag("Pillars"));
    }
	
	void Update () {
        if(OnTheMove == false)
        {
            if(this.GetComponent<Controll>().xbox_a == true)
            {
                SearchForPillars(new Vector3(this.GetComponent<Controll>().lshorizontal, 0, -this.GetComponent<Controll>().lsvertical));
            }

            if (Input.GetKeyDown(KeyCode.DownArrow) && Input.GetKeyDown(KeyCode.RightArrow))
            {
                SearchForPillars(new Vector3(90, 0, -90));
                
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) && Input.GetKeyDown(KeyCode.LeftArrow))
            {
                SearchForPillars(new Vector3(-90, 0, 90));
                
            }
            else if(Input.GetKeyDown(KeyCode.DownArrow) && Input.GetKeyDown(KeyCode.LeftArrow))
            {
                SearchForPillars(new Vector3(-90, 0, -90));
                
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) && Input.GetKeyDown(KeyCode.RightArrow))
            {
                SearchForPillars(new Vector3(90, 0, 90));
                
            }

            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                SearchForPillars(new Vector3(0, 0, -90));
                
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                SearchForPillars(new Vector3(0, 0, 90));
               
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                SearchForPillars(new Vector3(-90, 0, 0));
               
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                SearchForPillars(new Vector3(90, 0, 0));
                
            }
        }
        FinishMovement();
        RotateCharachter();
    }

    void SearchForPillars(Vector3 Direction)
    {
        Pillars.Clear();
        //Fill the pillar list from closest to farthest 
        RaycastHit hit;
        Ray ray = new Ray(this.transform.position, Direction);
        Debug.DrawRay(this.transform.position, new Vector3(Direction.x, Direction.y, Direction.z), Color.red, 10f);
        
        float distanceToObstacle = 0;
        int layermask = 1 << 9;
        
        
        if (Physics.SphereCast(ray, 2, out hit, JumpRange, layermask))
        {
            distanceToObstacle = hit.distance;
            if(hit.collider.gameObject.tag == "Pillars")
                Pillars.Add(hit.collider.gameObject);
        }
        if (Pillars.Count != 0)
        {
            MoveToPillar();
            foreach (GameObject markers in Markers)
            {
                Destroy(markers);
            }
        }
    }
    void MoveToPillar()
    {          
            OnTheMove = true;
            TargetPillar = Pillars[0];//takes the closest pillar 
            if (CurrentPillar == null)
            {
                CurrentPillar = GameObject.CreatePrimitive(PrimitiveType.Cube);
                CurrentPillar.transform.position = this.transform.position;
                CurrentPillar.SetActive(false);
            }
            journeyLength = Vector3.Distance(CurrentPillar.transform.position, TargetPillar.transform.position);
            startTime = Time.time;
            InternalSpeed = InternalSpeed * Vector3.Distance(CurrentPillar.transform.position, TargetPillar.transform.position);
    }
    void FinishMovement()
    {
        if (!OnTheMove)
            return;

        try
        {
            if (CurrentPillar != TargetPillar)
            {
                //move
                float distCovered = (Time.time - startTime) * InternalSpeed;
                float fracJourney = distCovered / journeyLength;
                this.transform.position = Vector3.Lerp(CurrentPillar.transform.position + new Vector3(0, 10, 0),
                    TargetPillar.transform.position + new Vector3(0, 10, 0),
                    fracJourney);
                if(TargetPillar.transform.position.x == this.transform.position.x  && TargetPillar.transform.position.z == this.transform.position.z )
                {
                    OnTheMove = false;
                    CurrentPillar = TargetPillar;
                    InternalSpeed = Speed;
                    ShowPossibleDestinations();
                }
            }
        }
        catch
        {
            Debug.Log("MoveToPillar Error!");
        }
    }

    void RotateCharachter()
    {
        //this.transform.Rotate(new Vector3(0, this.GetComponent<Controll>().lshorizontal, this.GetComponent<Controll>().lsvertical));
        //this.transform.rotation = new Quaternion(this.transform.rotation.x, this.GetComponent<Controll>().lsvertical, this.transform.rotation.z, this.transform.rotation.w);
        if (this.GetComponent<Controll>().lshorizontal != 0 || this.GetComponent<Controll>().lsvertical != 0)
            model.transform.eulerAngles = new Vector3(model.transform.eulerAngles.x, Mathf.Atan2(this.GetComponent<Controll>().lshorizontal, -this.GetComponent<Controll>().lsvertical) * Mathf.Rad2Deg, model.transform.eulerAngles.z);
        //else
            //model.transform.rotation = Quaternion.identity;
    }

    void ShowPossibleDestinations()
    {        
        foreach(GameObject pillar in AllPillars)
        {
            if (pillar.transform.position.x == this.transform.position.x && pillar.transform.position.z == this.transform.position.z)
            {
                continue;
            }
            if (Mathf.Abs(pillar.transform.position.x - this.transform.position.x) < JumpRange && Mathf.Abs(pillar.transform.position.z - this.transform.position.z) < JumpRange)
            {
                Markers.Add(Instantiate(Marker, pillar.transform.position + new Vector3(0, 10, 0), Quaternion.identity, this.transform));
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
       if(other.gameObject.tag == "Enemies")
        {
            other.gameObject.GetComponent<Enemy_Behaiviour>().HP--;
            Debug.Log("Hit");
        }    
    }
   

}
