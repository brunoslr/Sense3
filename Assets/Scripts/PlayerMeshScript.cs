using UnityEngine;
using System.Collections;

public class PlayerMeshScript : MonoBehaviour {

    private SkinnedMeshRenderer playerMesh;
    private float blendShapeMultiplier;
    public int blendShapeMaxValue = 100;

	// Use this for initialization
	void Start () {

        blendShapeMultiplier = blendShapeMaxValue/ PlayerStateScript.ReturnMaxPlayerLevel();


        playerMesh = this.GetComponentInChildren<SkinnedMeshRenderer>();

        if (playerMesh != null)
        {
            CoreSystem.onSoundEvent += UpdatePlayerShape;
            CoreSystem.onObstacleEvent += UpdatePlayerShape;
        }

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //updates blend shape related to the player
    public void UpdatePlayerShape()
    {
        int playerLevel = PlayerStateScript.getPlayerLevel();
        playerMesh.SetBlendShapeWeight(0, playerLevel * blendShapeMultiplier);
        Debug.Log(playerLevel);
    }


    void OnDisable()
    {
        CoreSystem.onSoundEvent -= UpdatePlayerShape;
        CoreSystem.onObstacleEvent -= UpdatePlayerShape;
    }

}
