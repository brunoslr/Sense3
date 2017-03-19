using UnityEngine;
using System.Collections;
namespace DynamicObstacles
{
    public class MeteorStyle : MonoBehaviour
    {
        public Material particleMaterial;

        void OnEnable()
        {
            var playerPosition = GameObject.Find("Player").transform.position;
            transform.position = new Vector3(playerPosition.x, playerPosition.y, playerPosition.z + 1000);
        }
    }
}