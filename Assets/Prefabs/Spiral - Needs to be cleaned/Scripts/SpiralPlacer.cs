using UnityEngine;

public class SpiralPlacer : ObstacleGenerator
{
    public Obstacle[] obstacles;

    public override void GenerateItems(PipeInstance pipeInstance)
    {
        float start = (Random.Range(0, pipeInstance.pipeSegmentCount) + 0.5f);
        float direction = Random.value < 0.5f ? 1f : -1f;

        float angleStep = pipeInstance.CurveAngle / pipeInstance.CurveSegmentCount;

        for (int i = 0; i < pipeInstance.CurveSegmentCount; i++)
        {
            Obstacle obstacle = Instantiate<Obstacle>(obstacles[Random.Range(0, obstacles.Length)]);
            float pipeRotation = (start + i * direction) * 360f / pipeInstance.pipeSegmentCount;
            obstacle.Position(pipeInstance, i * angleStep, pipeRotation);
        }
    }
}
