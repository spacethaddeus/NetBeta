using System;
using System.Numerics;

namespace NetBeta.Core.Entities;

public class Player : Entity
{
    public string Username = "";
    public Player()
    {
        SetHealth(20);
    }

    public void LoadData(string filePath)
    {
        
    }
}
