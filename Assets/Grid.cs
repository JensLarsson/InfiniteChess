using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public Camera camera;
    public GameObject tile;
    public GameObject unitGameObject;
    public Color playerColA = Color.white, playerColB = Color.black;
    float speed = 3;
    float xOffset = 0, yOffset = 0;
    List<Player> players = new List<Player>();
    bool playerFlag = false;
    Player player;
    int PlayerFlag
    {
        get
        {
            if (!playerFlag) return 0;
            else return 1;
        }
    }
    bool gameOver = true;
    Unit selectedUnit;
    Unit[,] units;
    List<Tile> moveMarkers = new List<Tile>();
    [HideInInspector] public static Tile[,] tiles;
    public int xMax = 16, yMax = 8;
    // Start is called before the first frame update



    public void CreateBoard(bool Hosting)
    {
        Mesh mesh = new Mesh();
        mesh.vertices = new Vector3[] {
            new Vector3(0, 0), new Vector3(xMax, 0),
            new Vector3(0, yMax), new Vector3(xMax, yMax) };
        mesh.triangles = new int[] { 0, 2, 3, 0, 3, 1 };

        GetComponent<MeshCollider>().sharedMesh = mesh;

        tiles = new Tile[xMax, yMax];
        units = new Unit[xMax, yMax];
        for (int x = 0; x < xMax; x++)
        {
            for (int y = 0; y < yMax; y++)
            {
                GameObject gObject = Instantiate(tile, transform);
                tiles[x, y] = gObject.GetComponent<Tile>();
                tiles[x, y].transform.position = new Vector3(x + 0.5f, y + 0.5f, 0);
                tiles[x, y].name = $"{x},{y}";
                tiles[x, y].SetText(x, y);

                if ((x + y) % 2 == 0)
                {
                    tiles[x, y].GetComponent<SpriteRenderer>().color = Color.black;
                }
            }
        }
        camera.transform.position = new Vector3(xMax / 2, yMax / 2, -6.5f);
        players.Add(new Player(playerColA, "Player A"));
        players.Add(new Player(playerColB, "Player B"));
        EventManager.Subscribe("KingDestroyed", GameOver);
        EventManager.Subscribe("MakeMove", MakeMove);

        //Player 1
        CreateUnit<Pawn>(2, 2, players[0]);
        CreateUnit<Pawn>(2, 3, players[0]);
        CreateUnit<Pawn>(2, 4, players[0]);
        CreateUnit<Pawn>(2, 5, players[0]);
        CreateUnit<Rook>(3, 5, players[0]);
        CreateUnit<Knight>(3, 2, players[0]);
        CreateUnit<Bishop>(3, 4, players[0]);
        CreateUnit<Queen>(3, 3, players[0]);
        CreateUnit<Rook>(4, 2, players[0]);
        CreateUnit<Knight>(4, 5, players[0]);
        CreateUnit<Bishop>(4, 3, players[0]);
        CreateUnit<King>(4, 4, players[0]);
        CreateUnit<Pawn>(5, 2, players[0]);
        CreateUnit<Pawn>(5, 3, players[0]);
        CreateUnit<Pawn>(5, 4, players[0]);
        CreateUnit<Pawn>(5, 5, players[0]);

        //Player 2
        CreateUnit<Pawn>(10, 2, players[1]);
        CreateUnit<Pawn>(10, 3, players[1]);
        CreateUnit<Pawn>(10, 4, players[1]);
        CreateUnit<Pawn>(10, 5, players[1]);
        CreateUnit<Rook>(12, 5, players[1]);
        CreateUnit<Knight>(12, 2, players[1]);
        CreateUnit<Bishop>(12, 3, players[1]);
        CreateUnit<Queen>(12, 4, players[1]);
        CreateUnit<Knight>(11, 5, players[1]);
        CreateUnit<Bishop>(11, 4, players[1]);
        CreateUnit<Rook>(11, 2, players[1]);
        CreateUnit<King>(11, 3, players[1]);
        CreateUnit<Pawn>(13, 2, players[1]);
        CreateUnit<Pawn>(13, 3, players[1]);
        CreateUnit<Pawn>(13, 4, players[1]);
        CreateUnit<Pawn>(13, 5, players[1]);
        gameOver = false;
        if (Hosting)
        {
            player = players[0];
        }
        else
        {
            player = players[1];
        }
    }

    void CreateUnit<T>(int x, int y, Player player) where T : Unit
    {
        GameObject gObject = Instantiate(unitGameObject);
        Unit unit = gObject.AddComponent<T>();
        gObject.GetComponent<SpriteRenderer>().color = player.playerColour;
        unit.SetPosition(x, y);
        unit.SetName();
        unit.player = player;
        moveUnit(unit.gameObject, unit.Position);
        units[x, y] = unit;

    }

    public void MakeMove(Vector2Int from, Vector2Int to)
    {
        if (MovePossible(from, to))
        {
            if (HasUnit(to))
            {
                units[to.x, to.y].DestroyUnit(units[from.x, from.y]);
            }
            units[to.x, to.y] = units[from.x, from.y];
            units[from.x, from.y] = null;
            units[to.x, to.y].SetPosition(to);
            units[to.x, to.y].AddMove();
            playerFlag = !playerFlag;
        }
    }
    public void MakeMove(EventParameter eParam)
    {
        if (players[PlayerFlag] != player)
        {
            string[] nums = eParam.stringParam.Split(' ');
            Vector2Int from = new Vector2Int(int.Parse(nums[0]), int.Parse(nums[1]));
            Vector2Int to = new Vector2Int(int.Parse(nums[2]), int.Parse(nums[3]));
            Debug.Log(from + "" + to);
            if (MovePossible(from, to))
            {
                Unit tempUnit = null;
                if (HasUnit(to))
                {
                    tempUnit = units[to.x, to.y];
                }
                units[to.x, to.y] = units[from.x, from.y];
                units[from.x, from.y] = null;
                units[to.x, to.y].SetPosition(to);
                units[to.x, to.y].AddMove();
                playerFlag = !playerFlag;
                if (tempUnit != null)
                {
                    tempUnit.DestroyUnit(units[to.x, to.y]);
                }
            }
        }
    }
    bool MovePossible(Vector2Int from, Vector2Int to)
    {
        List<Vector2Int> moves = PossibleMoves(units[from.x, from.y]);
        bool possible = false;
        foreach (Vector2Int vec in moves)
        {
            if (vec.x == to.x && vec.y == to.y) possible = true;
        }
        return possible;
    }

    public void ClickBoard(Vector3 mousePos)
    {
        if (!gameOver && player == players[PlayerFlag])
        {
            mousePos = CalculateGridPosition(mousePos.x - xOffset, mousePos.y - yOffset);
            Vector2Int pos = new Vector2Int((int)mousePos.x, (int)mousePos.y);
            if (moveMarkers.Count > 0 && HasMarker(pos))
            {

                EventParameter eParam = new EventParameter()
                {
                    stringParam = $"&MOVE|{selectedUnit.Position.x.ToString()} {selectedUnit.Position.y.ToString()} {pos.x.ToString()} {pos.y.ToString()}"
                };
                EventManager.TriggerEvent("SendMove", eParam);
                MakeMove(selectedUnit.Position, pos);
                //units[selectedUnit.Position.x, selectedUnit.Position.y] = null;
                //selectedUnit.SetPosition(pos);
                //selectedUnit.AddMove();
                //units[pos.x, pos.y] = selectedUnit;

                ClearMoveMarkers();
                selectedUnit = null;
                //playerFlag = !playerFlag;
            }
            else if (HasUnit(pos) && moveMarkers.Count == 0 && units[pos.x, pos.y].player == player)
            {
                SetMoveMarkers(units[pos.x, pos.y]);
                selectedUnit = units[pos.x, pos.y];
            }
            else
            {
                ClearMoveMarkers();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver)
        {
            xOffset -= Input.GetAxis("Horizontal") * Time.deltaTime * speed;
            yOffset -= Input.GetAxis("Vertical") * Time.deltaTime * speed;
            //Handle Mouse Click
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    //If click on board
                    if (hit.point.x >= 0 && hit.point.x < xMax && hit.point.y >= 0 && hit.point.y < yMax)
                    {
                        ClickBoard(hit.point);
                    }
                }
            }

            //Movement for Tiles
            for (int x = 0; x < xMax; x++)
            {
                for (int y = 0; y < yMax; y++)
                {
                    tiles[x, y].transform.position = CalculateGridPosition(x + 0.5f + xOffset, y + 0.5f + yOffset);
                }
            }
            //Movement for units
            foreach (Unit unit in units)
            {
                if (unit != null)
                {
                    moveUnit(unit);
                }
            }
            foreach (Tile tObject in moveMarkers)
            {
                moveUnit(tObject.gameObject, tObject.position);
            }
        }
    }

    void SetMoveMarkers(Unit unit)
    {
        List<Vector2Int> positions = PossibleMoves(unit);
        foreach (Vector2Int pos in positions)
        {
            GameObject gObject = Instantiate(tile);
            moveUnit(gObject, pos);
            gObject.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.5f);
            gObject.GetComponent<SpriteRenderer>().sortingOrder = 10;
            moveMarkers.Add(gObject.GetComponent<Tile>());
            gObject.GetComponent<Tile>().position = pos;
        }
    }


    List<Vector2Int> PossibleMoves(Unit unit)
    {
        List<Vector2Int> possitions = new List<Vector2Int>();
        Vector2Int[,] moveGrid = unit.GetMoveGrid(units);
        for (int i = 0; i < moveGrid.GetLength(0); i++)
        {
            for (int j = 0; j < moveGrid.GetLength(1); j++)
            {
                if (moveGrid[i, j].x == 99999)
                {
                    continue;
                }
                Vector2Int pos = CalculateGridPosition(moveGrid[i, j]);
                bool contains = false;
                foreach (Vector2Int vec in possitions) //Check if there already is a marker set at this location
                {
                    if (pos.x == vec.x && pos.y == vec.y)
                    {
                        contains = true;
                        break;
                    }
                }
                foreach (Unit u in units)
                {
                    if (u != null && u.Position.x == pos.x && u.Position.y == pos.y)
                    {
                        if (u.player == unit.player)
                        {
                            contains = true;
                        }
                        j = 100000;
                        break;
                    }
                }
                if (!contains)
                {
                    possitions.Add(pos);
                }

            }
        }
        return possitions;
    }

    bool HasUnit(Vector2Int pos)
    {
        if (units[pos.x, pos.y] != null)
        {
            return true;
        }
        return false;
    }
    bool HasUnit(int x, int y)
    {
        if (units[x, y] != null)
        {
            return true;
        }
        return false;
    }

    bool HasMarker(Vector2Int pos) //This should be fixed to class with proper hashcodes instead of GameObject
    {
        bool marker = false;
        foreach (Tile tObject in moveMarkers)
        {
            if (tObject.position.x == pos.x && tObject.position.y == pos.y)
            {
                marker = true;
            }
        }
        return marker;
    }

    void ClearMoveMarkers()
    {
        foreach (Tile marker in moveMarkers)
        {
            Destroy(marker.gameObject);
        }
        moveMarkers = new List<Tile>();
    }

    void GameOver(EventParameter eParam)
    {
        gameOver = true;
    }

    //bool ValidPosition(Vector2Int pos)
    //{
    //    if (pos.x >= 0 && pos.x < xMax && pos.y >= 0 && pos.y < yMax)
    //    {
    //        return true;
    //    }
    //    return false;
    //}
    //bool ValidPosition(Vector2 pos)
    //{
    //    if (pos.x >= 0 && pos.x < xMax && pos.y >= 0 && pos.y < yMax)
    //    {
    //        return true;
    //    }
    //    return false;
    //}

    void moveUnit(GameObject gObject, Vector3 pos)
    {
        gObject.transform.position = tiles[(int)pos.x, (int)pos.y].transform.position;
    }
    void moveUnit(Unit unit)
    {
        unit.transform.position = tiles[unit.Position.x, unit.Position.y].transform.position;
    }
    void moveUnit(GameObject gObject, Vector2Int pos)
    {
        pos = CalculateGridPosition(pos);
        gObject.transform.position = tiles[(int)pos.x, (int)pos.y].transform.position;
    }

    Vector3 CalculateGridPosition(float x, float y)
    {
        while (x < 0) x += xMax;
        while (y < 0) y += yMax;
        return new Vector3(x % xMax, y % yMax);
    }
    Vector2Int CalculateGridPosition(Vector2Int pos)
    {
        while (pos.x < 0) pos.x += xMax;
        while (pos.y < 0) pos.y += yMax;
        pos.x = pos.x % xMax;
        pos.y = pos.y % yMax;
        return pos;
    }
    Vector2Int CalculateGridPosition(int x, int y)
    {
        while (x < 0) x += xMax;
        while (y < 0) y += yMax;
        x = x % xMax;
        y = y % yMax;
        return new Vector2Int(x, y);
    }
}
