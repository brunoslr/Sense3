using UnityEngine;
using System.Collections;

public class PlayerMeshScript : MonoBehaviour {

    private SkinnedMeshRenderer playerMesh;
    private float blendShapeMultiplier;
    public int blendShapeMaxValue = 100;

	// Use this for initialization
	void Start () {

        blendShapeMultiplier = blendShapeMaxValue/ PlayerStateScript.GetMaxPlayerLevel();


        playerMesh = this.GetComponentInChildren<SkinnedMeshRenderer>();

        if (playerMesh != null)
        {
            EventBusManager.onSoundEvent += UpdatePlayerShape;
            EventBusManager.onObstacleEvent += UpdatePlayerShape;
        }

	}

    void OnDisable()
    {
        EventBusManager.onSoundEvent -= UpdatePlayerShape;
        EventBusManager.onObstacleEvent -= UpdatePlayerShape;
    }

    //updates blend shape related to the player
    public void UpdatePlayerShape()
    {
        int playerLevel = PlayerStateScript.GetPlayerLevel();
        playerMesh.SetBlendShapeWeight(0, playerLevel * blendShapeMultiplier);
    }


}
