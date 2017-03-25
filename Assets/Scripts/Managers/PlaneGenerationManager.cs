using UnityEngine;
using System.Collections.Generic;

namespace PlaneGeneration {

    public class PlaneGenerationManager : MonoBehaviour
    {
        public GameObject planePrefab;
        public GameObject player;
        public int numberOfDificultySettings = 3;

        //Visual obstacle requirements
        public int visualDisplacementHorizontal;
        public int visualDisplacementHorizontalRandomRange;
        public int visualDisplacementForward;
        public int visualDisplacementForwardInitial;
        public int visualDisplacementVertical;

        //Sound obstacle placement requirements
        public GameObject soundObstaclePrefab;
        public int soundDisplacementRandomFactor;
        public int soundDisplacementFromPlayer;

        //Tactile obstacle placement requirements
        public GameObject tactileObstaclePrefab;
        public int tactileDisplacementInitial;
        public int tactileDisplacementRandomFactorLow;
        public int tactileDisplacementRandomFactorHigh;
        public int tactileDisplacementFromPlayer;

        //Dynamic obstacle placement  
        public GameObject dynamicObstaclePrefab;
        public int dynamicDisplacementRandomFactorLow;
        public int dynamicDisplacementRandomFactorHigh;

        private GameObject obstacleCollector;

        //private GameObject visualObstacle;

        //Plane update requirements
        private float sizeOfPlaneX;
        private float sizeOfPlaneZ;

        private GameObject[,] planes;
        private GameObject planesContainer;

        private int playerXPosition;
        //private float playerYPosition; removed -- Unused
        private int playerZPosition;

        private int xPosVisualObstacle;

        private GameObject soundObstacle;
        private int soundZScale;
        //private float initialSoundZScale;
        //private uint soundCounter;

        private GameObject tactileObstacle;
        private int tactileZScale;

        private GameObject dynamicObstacle;

        private int xClamp;
        private int lowerClamp;
        private int upperClamp;

        private int visualPlacementZTrigger;
        private int visualPlacementInitialTrigger;
        private int nextVisualZDisplacement;

        private int nextSoundZDisplacement;
        private int soundPlacementZTrigger;

        private int nextTactileZDisplacement;
        private int tactilePlacementZTrigger;

        private int dynamicPlacementZTrigger;

        private int currentPlayerLevel;
        private int previousPlayerLevel;
        private int maxPlayerLevel;

        private int[] rotationArray = { 0, 90, 180, 270 };

        private int initialVOPrefabByDificultyLevel = 0, lastVOPrefabByDificultyLevel; // Prefab options by dificulty level
        private int totalNumberOfVOPrefabs, RandomGenerationProbabilityVO, numberOfObstaclesInEachDifficultyLevel;

        //Pooling Variables
        public bool poolAfterComplete = true;

        void Start()
        {
            InitializePlayerVariables();

            SetupPlaneGrid();

            SetupVisualObstacles();

            LoadSoundObstacle();

            LoadTactileObstacle();

            InitializeVisualObstacles();

            EventBusManager.onSoundEvent += ManageLevelGenerationState;
        }

        void OnDisable()
        {
            EventBusManager.onSoundEvent -= ManageLevelGenerationState;
        }

        #region On Start Setup

        private void InitializePlayerVariables()
        {
            playerXPosition = (int)player.transform.position.x;
            playerZPosition = (int)player.transform.position.z;
            previousPlayerLevel = PlayerStateScript.GetPlayerLevel();
            maxPlayerLevel = PlayerStateScript.GetMaxPlayerLevel();
            currentPlayerLevel = previousPlayerLevel;
        }

        private void SetupPlaneGrid()
        {
            int columns = 3;
            int rows = 3;

            planes = new GameObject[columns, rows];
            planesContainer = new GameObject("Planes");
            //(GameObject)Instantiate(TestMeteor, new Vector3(i, j, 0), Quaternion.identity);

            sizeOfPlaneX = planePrefab.GetComponent<Renderer>().bounds.size.x;
            sizeOfPlaneZ = planePrefab.GetComponent<Renderer>().bounds.size.z;
           // Vector3 planePosition;
            for (int i = 0; i < 3; i++)
            {
                for (int c = 0; c < 3; c++)
                {
                    planes[i, c] = (GameObject)Instantiate(planePrefab, 
                            new Vector3((float)sizeOfPlaneX * (i - 1), -1, (float)sizeOfPlaneZ * ((c - 1) * -1)),
                            Quaternion.identity);
                    planes[i, c].name = i + "," + c;
                    planes[i, c].transform.parent = planesContainer.transform;
                   
                }
            }
        }

        private void SetupVisualObstacles()
        {
            xClamp = visualDisplacementHorizontal / 2;
            lowerClamp = (int)player.transform.position.x - xClamp;
            upperClamp = (int)player.transform.position.x + xClamp;

            xPosVisualObstacle = (int)player.transform.position.x;

            totalNumberOfVOPrefabs = GetNumberOfPrefabsAvailable();
            numberOfObstaclesInEachDifficultyLevel = totalNumberOfVOPrefabs / numberOfDificultySettings;
            obstacleCollector = new GameObject("ObstacleCollector");

            ManageLevelGenerationState();

            visualPlacementZTrigger = visualDisplacementForwardInitial;
            visualPlacementInitialTrigger = visualDisplacementForwardInitial - visualDisplacementForward;
        }

        private void LoadSoundObstacle()
        {
            soundObstacle = Instantiate(soundObstaclePrefab);
            soundObstacle.SetActive(false);
            soundZScale = (int)soundObstacle.transform.localScale.z;
            nextSoundZDisplacement = soundDisplacementFromPlayer + (soundZScale / 4);
            soundObstacle.transform.position = new Vector3(Random.Range(lowerClamp, upperClamp), 0, nextSoundZDisplacement);
            soundPlacementZTrigger = nextSoundZDisplacement + (soundZScale / 2) + 500;
            soundObstacle.SetActive(true);
        }

        private void LoadTactileObstacle()
        {
            tactileObstacle = Instantiate(tactileObstaclePrefab);
            tactileObstacle.SetActive(false);
            tactileZScale = (int)tactileObstacle.transform.localScale.z;
            nextTactileZDisplacement = tactileDisplacementInitial + (tactileZScale / 2);
            tactileObstacle.transform.position = new Vector3(Random.Range(lowerClamp, upperClamp), 0, nextTactileZDisplacement);
            tactilePlacementZTrigger = nextTactileZDisplacement + (tactileZScale / 2) + Random.Range(tactileDisplacementRandomFactorLow, tactileDisplacementRandomFactorHigh);
            tactileObstacle.SetActive(true);
        }

        private void InitializeVisualObstacles()
        {

            float xpos = xPosVisualObstacle - (2 * visualDisplacementHorizontal) + Random.Range(-visualDisplacementHorizontalRandomRange, visualDisplacementHorizontalRandomRange);
            float zpos = visualDisplacementForwardInitial;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    GenerateVisualObstacleAtPosition(xpos, visualDisplacementVertical, zpos);
                    xpos += visualDisplacementHorizontal + Random.Range(-visualDisplacementHorizontalRandomRange, visualDisplacementHorizontalRandomRange);
                }
                zpos += ((5 - (i + 1)) / 5.0f) * visualDisplacementForwardInitial;
                xpos = xPosVisualObstacle - (2 * visualDisplacementHorizontal) + Random.Range(-visualDisplacementHorizontalRandomRange, visualDisplacementHorizontalRandomRange);
            }
            nextVisualZDisplacement = (int)zpos;
        }

        #endregion

        #region FixedUpdate
        void FixedUpdate()
        {
            UpdatePlayer();

            UpdatePlane();

            RemoveVisualObstaclesBehindTheCamera();

            if (currentPlayerLevel < maxPlayerLevel)
            {
                UpdateVisualObstacles();

                UpdateSoundObstacle();

                UpdateTactileObstacle();
            }

            else
            {
                DisableObstacles();
            }
        }

        private void UpdatePlayer()
        {
            playerXPosition = (int)player.transform.position.x;
            playerZPosition = (int)player.transform.position.z;
            currentPlayerLevel = PlayerStateScript.GetPlayerLevel();

            if (previousPlayerLevel != currentPlayerLevel)
            {
                if (currentPlayerLevel >= 0 && currentPlayerLevel <= 2)
                {
                    tactileDisplacementRandomFactorLow = 3000;
                    tactileDisplacementRandomFactorHigh = 4500;
                }
                else if (currentPlayerLevel >= 3 && currentPlayerLevel <= 5)
                {
                    tactileDisplacementRandomFactorLow = 1500;
                    tactileDisplacementRandomFactorHigh = 3000;
                }
                else if (currentPlayerLevel >= 6 && currentPlayerLevel <= 7)
                {
                    tactileDisplacementRandomFactorLow = 0;
                    tactileDisplacementRandomFactorHigh = 1500;
                }
                previousPlayerLevel = currentPlayerLevel;
            }
        }

        private void UpdateVisualObstacles()
        {

            if ((int)playerXPosition < lowerClamp)
            {
                xPosVisualObstacle -= (int)visualDisplacementHorizontal;
                upperClamp = lowerClamp;
                lowerClamp -= 2 * xClamp;
                CreateVisualObstaclesOnSide(-1.0f);
            }

            if ((int)playerXPosition > upperClamp)
            {
                xPosVisualObstacle += (int)visualDisplacementHorizontal;
                lowerClamp = upperClamp;
                upperClamp += 2 * xClamp;
                CreateVisualObstaclesOnSide(1.0f);
            }

            if (playerZPosition > visualPlacementZTrigger)
            {
                if (visualPlacementInitialTrigger > 100)
                {
                    visualPlacementZTrigger += visualPlacementInitialTrigger;
                    visualPlacementInitialTrigger -= visualDisplacementForward;
                }
                else
                {
                    visualPlacementZTrigger += (int)visualDisplacementForward;
                }
                nextVisualZDisplacement += (int)visualDisplacementForward;
                GenerateVisualObstacles();
            }
        }

        private void UpdateSoundObstacle()
        {
            if (playerZPosition > soundPlacementZTrigger)
            {
                nextSoundZDisplacement = playerZPosition + soundDisplacementFromPlayer + (soundZScale / 2) + Random.Range(-soundDisplacementRandomFactor, soundDisplacementRandomFactor);
                soundPlacementZTrigger = nextSoundZDisplacement + (soundZScale / 2) + 500;
                GenerateSoundObstacle(nextSoundZDisplacement);
            }
        }

        private void UpdateTactileObstacle()
        {
            if (playerZPosition > tactilePlacementZTrigger)
            {
                nextTactileZDisplacement = playerZPosition + tactileDisplacementFromPlayer + (tactileZScale / 2);
                tactilePlacementZTrigger = nextTactileZDisplacement + (tactileZScale / 2) + Random.Range(tactileDisplacementRandomFactorLow, tactileDisplacementRandomFactorHigh);
                GenerateTactileObstacle(nextTactileZDisplacement);
            }
        }

        private void RemoveVisualObstaclesBehindTheCamera()
        {
            Transform visualObstacleTransform;
            for (int i = 0; i < obstacleCollector.transform.childCount; i++)
            {
                visualObstacleTransform = obstacleCollector.transform.GetChild(i).transform;

                if (visualObstacleTransform.position.z + 150.0f < playerZPosition)
                {
                    SendObjectToPool(visualObstacleTransform.gameObject);
                }
            }
        }


        #region PlaneMovement

        private void UpdatePlane()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int c = 0; c < 3; c++)
                {
                    float planeXMin = planes[i, c].transform.position.x - (sizeOfPlaneX / 2);
                    float planeXMax = planeXMin + sizeOfPlaneX;
                    float planeZMin = planes[i, c].transform.position.z - (sizeOfPlaneZ / 2);
                    float planeZMax = planeZMin + sizeOfPlaneZ;
                    if (playerXPosition > planeXMin && playerXPosition < planeXMax && playerZPosition > planeZMin && playerZPosition < planeZMax)
                    {
                        UpdatePlaneGrid(i, c);
                    }
                }
            }
        }


        private void UpdatePlaneGrid(int x, int z)
        {
            if (x == 0)
            {
                MovePlaneLeft();
            }
            else if (x == 2)
            {
                MovePlaneRight();
            }
            if (z == 0)
            {
                MovePlaneAhead();
            }
        }

        private void MovePlaneLeft()
        {

            for (int i = 2; i > 0; i--)
            {
                int j = (i - 1) % 3;
                planes[i, 0].transform.position = planes[j, 0].transform.position;
                planes[i, 1].transform.position = planes[j, 1].transform.position;
                planes[i, 2].transform.position = planes[j, 2].transform.position;

            }

            for (int i = 0; i < 3; i++)
            {
                Vector3 addPos = new Vector3(-sizeOfPlaneX, 0, 0);
                planes[0, i].transform.position += addPos;

            }
        }

        private void MovePlaneRight()
        {

            for (int i = 0; i < 2; i++)
            {
                int j = (i + 1) % 3;
                planes[i, 0].transform.position = planes[j, 0].transform.position;
                planes[i, 1].transform.position = planes[j, 1].transform.position;
                planes[i, 2].transform.position = planes[j, 2].transform.position;

            }

            for (int i = 0; i < 3; i++)
            {
                Vector3 addPos = new Vector3(sizeOfPlaneX, 0, 0);
                planes[2, i].transform.position += addPos;

            }

        }

        private void MovePlaneAhead()
        {
             
          
            for (int i = 2; i> 0; i--)
            {
                int j = (i - 1) % 3;
                planes[0, i].transform.position = planes[0, j].transform.position;
                planes[1, i].transform.position = planes[1, j].transform.position;
                planes[2, i].transform.position = planes[2, j].transform.position;

            }

            for (int i = 0; i < 3; i++)
            {
                Vector3 addPos = new Vector3(0, 0, sizeOfPlaneZ);
                planes[i, 0].transform.position += addPos;

            }

        }

        #endregion

        #endregion

        /// <summary>
        /// Plane Generator Rules by player level
        /// </summary>
        private void ManageLevelGenerationState()
        {
            
            switch (currentPlayerLevel)
            {
                case 1:
                    initialVOPrefabByDificultyLevel = 0;
                    lastVOPrefabByDificultyLevel = numberOfObstaclesInEachDifficultyLevel;
                    RandomGenerationProbabilityVO = 50;
                    break;

                case 2:
                    initialVOPrefabByDificultyLevel = 0;
                    lastVOPrefabByDificultyLevel = (numberOfObstaclesInEachDifficultyLevel) * 3 / 2;
                    RandomGenerationProbabilityVO = 50;
                    break;

                case 3:
                    initialVOPrefabByDificultyLevel = numberOfObstaclesInEachDifficultyLevel / 2;
                    lastVOPrefabByDificultyLevel = (numberOfObstaclesInEachDifficultyLevel) * 2;
                    RandomGenerationProbabilityVO = 60;
                    break;

                case 4:
                    initialVOPrefabByDificultyLevel = numberOfObstaclesInEachDifficultyLevel;
                    lastVOPrefabByDificultyLevel = (numberOfObstaclesInEachDifficultyLevel) * 5 / 2;
                    RandomGenerationProbabilityVO = 75;
                    break;

                case 5:
                    initialVOPrefabByDificultyLevel = numberOfObstaclesInEachDifficultyLevel * 3 / 2;
                    lastVOPrefabByDificultyLevel = totalNumberOfVOPrefabs;
                    RandomGenerationProbabilityVO = 90;
                    break;

                case 6:
                    initialVOPrefabByDificultyLevel = 2 * numberOfObstaclesInEachDifficultyLevel;
                    lastVOPrefabByDificultyLevel = totalNumberOfVOPrefabs;
                    RandomGenerationProbabilityVO = 100;
                    break;

                default:
                    initialVOPrefabByDificultyLevel = 0;
                    lastVOPrefabByDificultyLevel = numberOfObstaclesInEachDifficultyLevel;
                    RandomGenerationProbabilityVO = 40;
                    break;

            }
        }

        private void GenerateVisualObstacles()
        {
            float xpos = xPosVisualObstacle - (2 * visualDisplacementHorizontal) + Random.Range(-visualDisplacementHorizontalRandomRange, visualDisplacementHorizontalRandomRange);
            for (int i = 0; i < 5; ++i)
            {
                GenerateVisualObstacleAtPosition(xpos, visualDisplacementVertical, nextVisualZDisplacement);
                xpos += visualDisplacementHorizontal + Random.Range(-visualDisplacementHorizontalRandomRange, visualDisplacementHorizontalRandomRange);
            }
        }

        private void CreateVisualObstaclesOnSide(float dir)
        {

            for (int i = -5; i < 1; i++)
            {
                float xpos = xPosVisualObstacle + (2 * visualDisplacementHorizontal * dir) + (Random.Range(0, visualDisplacementHorizontalRandomRange) * dir);
                Vector3 newPosition = new Vector3(xpos, 0, nextVisualZDisplacement + (visualDisplacementForward * i));
                if ((Physics.OverlapBox(newPosition, Vector3.one).Length <= 2))
                {
                    GenerateVisualObstacleAtPosition(xpos, visualDisplacementVertical, nextVisualZDisplacement + (visualDisplacementHorizontal * i));
                }
            }
        }

        private void GenerateVisualObstacleAtPosition(float xPos, float yPos, float zPos)
        {
            int pick;
            GameObject visualObstacle;
            if (Random.Range(0, 100) < RandomGenerationProbabilityVO)
            {
                pick = Random.Range(initialVOPrefabByDificultyLevel, lastVOPrefabByDificultyLevel);
                visualObstacle = GetObjectFromPoolByPrefabIndex(pick, true);
                SetupVisualObstacleRotation(visualObstacle);
                visualObstacle.transform.position = new Vector3(xPos, yPos, zPos);
                visualObstacle = null;
            }
        }

        /// <summary>
        /// We are using the layer tem as the Static Layer to define that an Visual Obstacle should not rotate
        /// </summary>
        /// <param name="visualObstacle"></param>
        private void SetupVisualObstacleRotation(GameObject visualObstacle)
        {

            int rotX, rotY, rotZ;

            if (visualObstacle.layer != 10)
            {
                rotX = rotationArray[Random.Range(0, 4)];
                rotY = rotationArray[Random.Range(0, 4)];
                rotZ = rotationArray[Random.Range(0, 4)];
                visualObstacle.transform.Rotate(new Vector3(rotX, rotY, rotZ));
            }
        }

        private void GenerateSoundObstacle(float zPosition)
        {
            soundObstacle.SetActive(true);
            soundObstacle.GetComponentInChildren<PickUpScript>().pickedUp = false;
            soundObstacle.transform.position = new Vector3(Random.Range(lowerClamp, upperClamp), 0, zPosition);
        }

        private void GenerateTactileObstacle(float zPosition)
        {
            tactileObstacle.SetActive(true);
            tactileObstacle.transform.position = new Vector3(Random.Range(lowerClamp, upperClamp), 0, zPosition);
        }

        private void DisableObstacles()
        {
            soundObstacle.SetActive(false);
            tactileObstacle.SetActive(false);
        }


        #region ObjectPool Calls

        private GameObject GetObjectFromPoolByPrefabIndex(int prefabIndex, bool allowGrowth)
        {
            GameObject newObj = ObjectPool.instance.GetObjectAtIndexPrefab(prefabIndex, allowGrowth);
            newObj.transform.parent = obstacleCollector.transform;
            return newObj;
        }

        private void SendObjectToPool(GameObject objectToPool)
        {
            ObjectPool.instance.PoolObject(objectToPool.gameObject);
        }

        private int GetNumberOfPrefabsAvailable()
        {
            return ObjectPool.instance.GetObjectsPrefabLength();
        }
        #endregion
    }
 }