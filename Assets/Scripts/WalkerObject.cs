using UnityEngine;

public class WalkerObject
{
    public Vector2 position;
    public Vector2 direction;
    public float chanceToChange;

    public WalkerObject(Vector2 _pos, Vector2 _dir, float _chanceToChange)
    {
        position = _pos;
        direction = _dir;
        chanceToChange = _chanceToChange;
    }
}
