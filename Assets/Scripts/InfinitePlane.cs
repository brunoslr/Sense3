using UnityEngine;
using System.Collections;

public class InfinitePlane : MonoBehaviour {
	
	public GameObject plane; 
	public GameObject player; 
	
	private float sizeOfPlaneX;
	private float sizeOfPlaneZ;
	
	private GameObject[,] planes = new GameObject[3, 3];
	
	// Use this for initialization
	void Start () {
		sizeOfPlaneX = plane.GetComponent<Renderer>().bounds.size.x;
		sizeOfPlaneZ = plane.GetComponent<Renderer>().bounds.size.z;
		for(int i = 0; i < 3; i++){
			for(int c = 0; c < 3; c++){
					planes[i, c] = GameObject.Instantiate(plane) as GameObject;
					planes[i, c].name = i+","+c;
			}
		}
				
		// Reposition all of the planes.
		for(int i = -1; i < 2; i++){
			for(int c = -1; c < 2; c++){
				// Adjust the x, z positions.
				Vector3 tempPos = new Vector3((float)sizeOfPlaneX*(i), 0, (float)sizeOfPlaneZ*(c));
				planes[i+1, c+1].transform.position += tempPos;
			}
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		var playerX = player.transform.position.x;
		var playerZ = player.transform.position.z;
		for (int i = 0; i < 3; i++){
			for (int c = 0; c < 3; c++){
				float planeXMin = planes[i, c].transform.position.x - (sizeOfPlaneX/2);
				float planeXMax = planeXMin + sizeOfPlaneX;
				float planeZMin = planes[i, c].transform.position.z - (sizeOfPlaneZ/2);
				float planeZMax = planeZMin + sizeOfPlaneZ;
				if(playerX > planeXMin && playerX < planeXMax && playerZ > planeZMin && playerZ < planeZMax){
					updatePlanes(i, c);	
				}
			}
		}
	}
	
	public void updatePlanes(int x, int z){
		if(x == 0){
			MoveLeft();
		}
		else if(x == 2){
			MoveRight();
		}

		if(z == 2){
			MoveAhead();
		}
	}
	
	public void MoveLeft(){
		Debug.Log("Left");
		GameObject[,] newPlanes = new GameObject[3, 3];
		for(int i = 0; i < 3; i++){
			Vector3 addPos = new Vector3(-sizeOfPlaneX*3, 0, 0);
			planes[2, i].transform.position +=	addPos;
		}
		for(int i = 0; i < 3; i++){
			int c = i - 1;
			c = (c == -1)? 2 : c;
			newPlanes[i, 0] = planes[c, 0];
			newPlanes[i, 1] = planes[c, 1];
			newPlanes[i, 2] = planes[c, 2]; 
		}
		planes = newPlanes;
	}
	
	public void MoveRight(){
		Debug.Log("Right");
		GameObject[,] newPlanes = new GameObject[3, 3];
		for(int i = 0; i < 3; i++){
			Vector3 addPos = new Vector3(sizeOfPlaneX*3, 0, 0);
			planes[0, i].transform.position += addPos;
		}
		for(int i = 0; i < 3; i++){
			int c = i + 1;
			c = (c == 3)? 0 : c;
			newPlanes[i, 0] = planes[c, 0];
			newPlanes[i, 1] = planes[c, 1];
			newPlanes[i, 2] = planes[c, 2]; 
		}
		planes = newPlanes;
	}
	
	public void MoveAhead(){
		Debug.Log("Ahead");
		GameObject[,] newPlanes = new GameObject[3, 3];
		for(int i = 0; i < 3; i++){
			Vector3 addPos = new Vector3(0, 0, sizeOfPlaneZ*3);
			planes[i, 0].transform.position += addPos;
		}
		for(int i = 0; i < 3; i++){
			int c = i - 1;
			c = (c == -1)? 2 : c;
			newPlanes[0, i] = planes[0, c];
			newPlanes[1, i] = planes[1, c];
			newPlanes[2, i] = planes[2, c]; 
		}
		planes = newPlanes;
	}
	
	public void moveDown(){
		Debug.Log("Down");
		GameObject[,] newPlanes = new GameObject[3, 3];
		for(int i = 0; i < 3; i++){
			Vector3 addPos = new Vector3(0, 0, -sizeOfPlaneX*3);
			planes[i, 0].transform.position += addPos;
		}
		for(int i = 0; i < 3; i++){
			int c = i + 1;
			c = (c == 3)? 0 : c;
			newPlanes[0, i] = planes[0, c];
			newPlanes[1, i] = planes[1, c];
			newPlanes[2, i] = planes[2, c]; 
		}
		planes = newPlanes;
	}
}
