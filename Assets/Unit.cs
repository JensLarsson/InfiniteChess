using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Player player;
    protected Vector2Int position = new Vector2Int(0, 0);
    public Vector2Int Position
    {
        get { return position; }
    }

    protected int moves = 0;
    public int Moves
    {
        get { return moves; }
    }
    protected Vector2Int CalculateGridPosition(Vector2Int pos)
    {
        while (pos.x < 0) pos.x += 16;
        while (pos.y < 0) pos.y += 8;
        pos.x = pos.x % 16;
        pos.y = pos.y % 8;
        return pos;
    }
    Vector2Int[,] newMoveGrid = { { } };
    bool[,] moveGrid = { { } };


    public virtual void DestroyUnit(Unit unit)
    {
        Debug.Log(this + " Destroyed");
        Destroy(this.gameObject);
    }
    public virtual void AddMove()
    {
        moves++;
    }

    public virtual Vector2Int[,] GetMoveGrid(Unit[,] units = null)
    {
        return newMoveGrid;
    }
    public virtual void SetName()
    {
        GetComponent<Tile>().SetText("");
    }
    public void SetPosition(int x, int y)
    {
        position = new Vector2Int(x, y);
    }
    public void SetPosition(Vector2Int pos)
    {
        position = pos;
    }
}

public class Pawn : Unit
{
    int direction = 1;
    Vector2Int[,] newMoveGrid = {
        {new Vector2Int(1, 0)},
        {new Vector2Int(1, 1)},
        {new Vector2Int(1, -1)}
    };
    public override Vector2Int[,] GetMoveGrid(Unit[,] units = null)
    {
        Vector2Int[,] grid =
        {
            {new Vector2Int(99999, 99999), new Vector2Int(99999, 99999)},
            {new Vector2Int(99999, 99999), new Vector2Int(99999, 99999)},
            {new Vector2Int(99999, 99999), new Vector2Int(99999, 99999)}
        };

        Vector2Int pos = CalculateGridPosition(new Vector2Int(2, 0) * direction + position);
        if (moves == 0 && units[pos.x, pos.y] == null && units[pos.x - 1 * direction, pos.y] == null)
        {
            grid[0, 1] = pos;
        }
        pos = CalculateGridPosition(new Vector2Int(1, 0) * direction + position);
        if (units[pos.x, pos.y] == null)
        {
            grid[0, 0] = pos;
        }
        pos = CalculateGridPosition(new Vector2Int(1 * direction, 1) + position);
        if (units[pos.x, pos.y] != null)
        {
            grid[1, 0] = pos;
        }
        pos = CalculateGridPosition(new Vector2Int(1 * direction, -1) + position);
        if (units[pos.x, pos.y] != null)
        {
            grid[2, 0] = pos;
        }
        return grid;
    }
    //Temporarily also used as start function;
    public override void SetName()
    {
        GetComponent<Tile>().SetText("P");
        if (position.x == 2 || position.x == 10)
        {
            direction = -1;
        }
    }
}
public class King : Unit
{
    Vector2Int[,] newMoveGrid = {
        {new Vector2Int(1, 0)},
        {new Vector2Int(1, 1)},
        {new Vector2Int(1, -1)},
        {new Vector2Int(0, 1)},
        {new Vector2Int(0, -1)},
        {new Vector2Int(-1, 1)},
        {new Vector2Int(-1, 0)},
        {new Vector2Int(-1, -1)}
    };

    public override void DestroyUnit(Unit unit)
    {
        EventParameter eParam = new EventParameter()
        {
            playerParam = unit.player
        };
        EventManager.TriggerEvent("KingDestroyed", eParam);
        base.DestroyUnit(unit);
    }


    public override Vector2Int[,] GetMoveGrid(Unit[,] units = null)
    {
        Vector2Int[,] moveGrid = new Vector2Int[newMoveGrid.GetLength(0), newMoveGrid.GetLength(1)];
        for (int i = 0; i < moveGrid.GetLength(0); i++)
        {
            for (int j = 0; j < moveGrid.GetLength(1); j++)
            {
                moveGrid[i, j] = CalculateGridPosition(newMoveGrid[i, j] + position);
            }
        }
        return moveGrid;
    }
    public override void SetName()
    {
        GetComponent<Tile>().SetText("K");
    }
}

public class Knight : Unit
{
    Vector2Int[,] newMoveGrid = {
        {new Vector2Int(2, 1)},
        {new Vector2Int(2, -1)},
        {new Vector2Int(-2, 1)},
        {new Vector2Int(-2, -1)},
        {new Vector2Int(1, 2)},
        {new Vector2Int(1, -2)},
        {new Vector2Int(-1, 2)},
        {new Vector2Int(-1, -2)}
    };

    public override Vector2Int[,] GetMoveGrid(Unit[,] units = null)
    {
        Vector2Int[,] moveGrid = new Vector2Int[newMoveGrid.GetLength(0), newMoveGrid.GetLength(1)];
        for (int i = 0; i < moveGrid.GetLength(0); i++)
        {
            for (int j = 0; j < moveGrid.GetLength(1); j++)
            {
                moveGrid[i, j] = CalculateGridPosition(newMoveGrid[i, j] + position);
            }
        }
        return moveGrid;
    }

    public override void SetName()
    {
        GetComponent<Tile>().SetText("Kn");
    }
}
public class Bishop : Unit
{
    Vector2Int[,] newMoveGrid = {
        {new Vector2Int(1, 1),new Vector2Int(2,2),new Vector2Int(3,3),new Vector2Int(4,4),new Vector2Int(5,5),new Vector2Int(6,6),new Vector2Int(7,7),
            new Vector2Int(8, 8),new Vector2Int(9, 9),new Vector2Int(10, 10),new Vector2Int(11, 11),new Vector2Int(12, 12),new Vector2Int(13, 13),new Vector2Int(14, 14),new Vector2Int(15, 15)},

        {new Vector2Int(-1, -1),new Vector2Int(-2,-2),new Vector2Int(-3,-3),new Vector2Int(-4,-4),new Vector2Int(-5,-5),new Vector2Int(-6,-6),new Vector2Int(-7,-7),
            new Vector2Int(-8, -8),new Vector2Int(-9, -9),new Vector2Int(-10, -10),new Vector2Int(-11, -11),new Vector2Int(-12, -12),new Vector2Int(-13, -13),
            new Vector2Int(-14, -14),new Vector2Int(-15, -15)},

        {new Vector2Int(-1, 1),new Vector2Int(-2,2),new Vector2Int(-3,3),new Vector2Int(-4,4),new Vector2Int(-5,5),new Vector2Int(-6,6),new Vector2Int(-7,7),
            new Vector2Int(-8, 8),new Vector2Int(-9, 9),new Vector2Int(-10, 10),new Vector2Int(-11, 11),new Vector2Int(-12, 12),new Vector2Int(-13, 13),
            new Vector2Int(-14, 14),new Vector2Int(-15, 15)},

        {new Vector2Int(1, -1),new Vector2Int(2,-2),new Vector2Int(3,-3),new Vector2Int(4,-4),new Vector2Int(5,-5),new Vector2Int(6,-6),new Vector2Int(7,-7),
            new Vector2Int(8, -8),new Vector2Int(9, -9),new Vector2Int(10, -10),new Vector2Int(11, -11),new Vector2Int(12, -12),new Vector2Int(13, -13),
            new Vector2Int(14, -14),new Vector2Int(15, -15)}
    };
    public override Vector2Int[,] GetMoveGrid(Unit[,] units = null)
    {
        Vector2Int[,] moveGrid = new Vector2Int[newMoveGrid.GetLength(0), newMoveGrid.GetLength(1)];
        for (int i = 0; i < moveGrid.GetLength(0); i++)
        {
            for (int j = 0; j < moveGrid.GetLength(1); j++)
            {
                moveGrid[i, j] = CalculateGridPosition(newMoveGrid[i, j] + position);
            }
        }
        return moveGrid;
    }
    public override void SetName()
    {
        GetComponent<Tile>().SetText("B");
    }
}

public class Rook : Unit
{
    Vector2Int[,] newMoveGrid = {
        {new Vector2Int(1, 0),new Vector2Int(2,0),new Vector2Int(3,0),new Vector2Int(4,0),new Vector2Int(5,0),new Vector2Int(6,0),new Vector2Int(7,0),
        new Vector2Int(8, 0),new Vector2Int(9,0),new Vector2Int(10,0),new Vector2Int(11,0),new Vector2Int(12,0),new Vector2Int(13,0),new Vector2Int(14,0), new Vector2Int(15,0)},

        {new Vector2Int(-1, 0),new Vector2Int(-2,0),new Vector2Int(-3,0),new Vector2Int(-4,0),new Vector2Int(-5,0),new Vector2Int(-6,0),new Vector2Int(-7,0),
        new Vector2Int(-8, 0),new Vector2Int(-9,0),new Vector2Int(-10,0),new Vector2Int(-11,0),new Vector2Int(-12,0),new Vector2Int(-13,0),new Vector2Int(-14,0), new Vector2Int(-15,0)},

        {new Vector2Int(0, 1),new Vector2Int(0, 2),new Vector2Int(0, 3),new Vector2Int(0, 4),new Vector2Int(0, 5),new Vector2Int(0, 6),new Vector2Int(0, 7),
        new Vector2Int(0, 8),new Vector2Int(0, 9),new Vector2Int(0, 10),new Vector2Int(0, 11),new Vector2Int(0, 12),new Vector2Int(0, 13),new Vector2Int(0, 14),new Vector2Int(0, 15)},

        {new Vector2Int(0, -1),new Vector2Int(0, -2),new Vector2Int(0, -3),new Vector2Int(0, -4),new Vector2Int(0, -5),new Vector2Int(0, -6),new Vector2Int(0, -7),
        new Vector2Int(0, -8),new Vector2Int(0, -9),new Vector2Int(0, -10),new Vector2Int(0, -11),new Vector2Int(0, -12),new Vector2Int(0, -13),new Vector2Int(0, -14),new Vector2Int(0, -15)}
    };
    public override Vector2Int[,] GetMoveGrid(Unit[,] units = null)
    {
        Vector2Int[,] moveGrid = new Vector2Int[newMoveGrid.GetLength(0), newMoveGrid.GetLength(1)];
        for (int i = 0; i < moveGrid.GetLength(0); i++)
        {
            for (int j = 0; j < moveGrid.GetLength(1); j++)
            {
                moveGrid[i, j] = CalculateGridPosition(newMoveGrid[i, j] + position);
            }
        }
        return moveGrid;
    }

    public override void SetName()
    {
        GetComponent<Tile>().SetText("R");
    }
}

public class Queen : Unit
{

    Vector2Int[,] newMoveGrid = {
        {new Vector2Int(1, 1),new Vector2Int(2,2),new Vector2Int(3,3),new Vector2Int(4,4),new Vector2Int(5,5),new Vector2Int(6,6),new Vector2Int(7,7),
            new Vector2Int(8, 8),new Vector2Int(9, 9),new Vector2Int(10, 10),new Vector2Int(11, 11),new Vector2Int(12, 12),new Vector2Int(13, 13),new Vector2Int(14, 14),new Vector2Int(15, 15)},

        {new Vector2Int(-1, -1),new Vector2Int(-2,-2),new Vector2Int(-3,-3),new Vector2Int(-4,-4),new Vector2Int(-5,-5),new Vector2Int(-6,-6),new Vector2Int(-7,-7),
            new Vector2Int(-8, -8),new Vector2Int(-9, -9),new Vector2Int(-10, -10),new Vector2Int(-11, -11),new Vector2Int(-12, -12),new Vector2Int(-13, -13),
            new Vector2Int(-14, -14),new Vector2Int(-15, -15)},

        {new Vector2Int(-1, 1),new Vector2Int(-2,2),new Vector2Int(-3,3),new Vector2Int(-4,4),new Vector2Int(-5,5),new Vector2Int(-6,6),new Vector2Int(-7,7),
            new Vector2Int(-8, 8),new Vector2Int(-9, 9),new Vector2Int(-10, 10),new Vector2Int(-11, 11),new Vector2Int(-12, 12),new Vector2Int(-13, 13),
            new Vector2Int(-14, 14),new Vector2Int(-15, 15)},

        {new Vector2Int(1, -1),new Vector2Int(2,-2),new Vector2Int(3,-3),new Vector2Int(4,-4),new Vector2Int(5,-5),new Vector2Int(6,-6),new Vector2Int(7,-7),
            new Vector2Int(8, -8),new Vector2Int(9, -9),new Vector2Int(10, -10),new Vector2Int(11, -11),new Vector2Int(12, -12),new Vector2Int(13, -13),
            new Vector2Int(14, -14),new Vector2Int(15, -15)},

        {new Vector2Int(1, 0),new Vector2Int(2,0),new Vector2Int(3,0),new Vector2Int(4,0),new Vector2Int(5,0),new Vector2Int(6,0),new Vector2Int(7,0),
        new Vector2Int(8, 0),new Vector2Int(9,0),new Vector2Int(10,0),new Vector2Int(11,0),new Vector2Int(12,0),new Vector2Int(13,0),new Vector2Int(14,0), new Vector2Int(15,0)},

        {new Vector2Int(-1, 0),new Vector2Int(-2,0),new Vector2Int(-3,0),new Vector2Int(-4,0),new Vector2Int(-5,0),new Vector2Int(-6,0),new Vector2Int(-7,0),
        new Vector2Int(-8, 0),new Vector2Int(-9,0),new Vector2Int(-10,0),new Vector2Int(-11,0),new Vector2Int(-12,0),new Vector2Int(-13,0),new Vector2Int(-14,0), new Vector2Int(-15,0)},

        {new Vector2Int(0, 1),new Vector2Int(0, 2),new Vector2Int(0, 3),new Vector2Int(0, 4),new Vector2Int(0, 5),new Vector2Int(0, 6),new Vector2Int(0, 7),
        new Vector2Int(0, 8),new Vector2Int(0, 9),new Vector2Int(0, 10),new Vector2Int(0, 11),new Vector2Int(0, 12),new Vector2Int(0, 13),new Vector2Int(0, 14),new Vector2Int(0, 15)},

        {new Vector2Int(0, -1),new Vector2Int(0, -2),new Vector2Int(0, -3),new Vector2Int(0, -4),new Vector2Int(0, -5),new Vector2Int(0, -6),new Vector2Int(0, -7),
        new Vector2Int(0, -8),new Vector2Int(0, -9),new Vector2Int(0, -10),new Vector2Int(0, -11),new Vector2Int(0, -12),new Vector2Int(0, -13),new Vector2Int(0, -14),new Vector2Int(0, -15)}


    };
    public override Vector2Int[,] GetMoveGrid(Unit[,] units = null)
    {
        Vector2Int[,] moveGrid = new Vector2Int[newMoveGrid.GetLength(0), newMoveGrid.GetLength(1)];
        for (int i = 0; i < moveGrid.GetLength(0); i++)
        {
            for (int j = 0; j < moveGrid.GetLength(1); j++)
            {
                moveGrid[i, j] = CalculateGridPosition(newMoveGrid[i, j] + position);
            }
        }
        return moveGrid;
    }

    public override void SetName()
    {
        GetComponent<Tile>().SetText("Q");
    }
}