using UnityEngine;

public class RandomPlacer : ObstacleGenerator {
    public Obstacle[] obstacles;

    public override void GenerateItems(PipeInstance pipeInstance)
    {
        float angleStep = pipeInstance.CurveAngle / pipeInstance.CurveSegmentCount;

        for (int i = 0; i < pipeInstance.CurveSegmentCount; i++)
        {
            Obstacle obstacle = Instantiate<Obstacle>(obstacles[Random.Range(0, obstacles.Length)]);
            float pipeRotation = (Random.Range(0, pipeInstance.pipeSegmentCount) + 0.5f) * 360f / pipeInstance.pipeSegmentCount;
            obstacle.Position(pipeInstance, i * angleStep, pipeRotation);
        }
    }
}