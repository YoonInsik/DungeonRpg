using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BasicMapGen : AbstractMapGen
{
    /*    [SerializeField]
        protected SimpleRandomWalkSO randomWalkParameters;

        protected override void RunProceduralGeneration(out HashSet<Vector2Int> floorPositions, out HashSet<Vector2Int> wallPositions)
        {
            floorPositions = RunRandomWalk(randomWalkParameters, startPosition);
            WallGenerator.CreateWalls(floorPositions, out wallPositions);
        }

        protected HashSet<Vector2Int> RunRandomWalk(SimpleRandomWalkSO parameters, Vector2Int position)
        {
            var currentPosition = position;
            HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
            for (int i = 0; i < parameters.iterations; i++)
            {
                var path = ProceduralGenerationAlgorithms.SimpleRandomWalk(currentPosition, parameters.walkLength);
                floorPositions.UnionWith(path);
                if (parameters.startRandomlyEachIteration)
                    currentPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
            }
            return floorPositions;
        }*/
    protected override void RunProceduralGeneration(out HashSet<Vector2Int> floorPositions, out HashSet<Vector2Int> wallPositions)
    {
        throw new System.NotImplementedException();
    }
}
