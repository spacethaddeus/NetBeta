using System;
using System.Numerics;

namespace NetBeta.Core.Entities;

public abstract class Entity
{
    private int _EntityID;
    private int _Health;

    public Vector3 PrevPosition = new();
    public Vector3 Position = new();
    public Vector2 PrevLook = new(); // (Yaw, Pitch)
    public Vector2 Look = new(); // (Yaw, Pitch)
    public bool OnGround = false;


    public void SetEntityID(int input)
    {
        _EntityID = input;
    }

    public int GetEntityID()
    {
        return _EntityID;
    }

    public void SetHealth(int input)
    {
        _Health = input;
    }

    public int GetHealth()
    {
        return _Health;
    }
}
