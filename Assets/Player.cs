using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class Player : MonoBehaviour
{
    private bool AutonomicMode = true;
    Vector2 previousMovement = Vector2.up;
    public Tilemap map;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.W) && CheckIfMovementPossible(Vector2.up)) //jesli wcisniety klawisz w gore && jesli mozliwe pojscie w gore
        {
            transform.Translate(Vector2.up); //do obecnej lokalizacjo dodA wektor UP i przesunie gracza
        }
        else if (Input.GetKeyDown(KeyCode.S) && CheckIfMovementPossible(Vector2.down))
        {
            transform.Translate(Vector2.down);
        }
        else if (Input.GetKeyDown(KeyCode.A) && CheckIfMovementPossible(Vector2.left))
        {
            transform.Translate(Vector2.left);
        }
        else if (Input.GetKeyDown(KeyCode.D) && CheckIfMovementPossible(Vector2.right))
        {
            transform.Translate(Vector2.right);
        }
    }

    private void FixedUpdate()
    {
        if (AutonomicMode)
        {
            if (CheckIfFinished()){
                AutonomicMode = false;
                return;
            }
            var possibleMovements = GetPossibleMovements();
            if (possibleMovements.Count > 1)
            {
                var previousPosition = new Vector2(-previousMovement.x, -previousMovement.y);
                possibleMovements.Remove(previousPosition);
            }

            var randomMovement = possibleMovements[Random.Range(0, possibleMovements.Count)];

            this.transform.Translate(randomMovement);
            previousMovement = randomMovement;
        }
    }

    bool CheckIfFinished()
    {
        Vector3Int tilePosition = map.WorldToCell(transform.position);
        TileBase tile = map.GetTile(tilePosition);
        return tile.name == "Finish";
    }

    bool CheckIfMovementPossible(Vector3 direction)
    {
        Vector3 positionToCheck = transform.position + direction;
        Vector3Int tilePosition = map.WorldToCell(positionToCheck); //tlumaczy/konwertuje wspolrzedne swiata (globalne) na pozycje komorki w Tilemapie; (odnosi sie do mapy Tilemap >> funkcja pozwalajaca przekonw. wspolrzedne;
        TileBase tile = map.GetTile(tilePosition); //tilePosition == wspolrzedne lokalne/adres komorki; map.GetTile zwraca obiekt komorki ze wspolrzednych tilePosition
        return tile.name != "Wall";
    }

    // Returns a list of available move directions
    List<Vector3> GetPossibleMovements()
    {  
        List<Vector3> possibleMovements = new List<Vector3>();

        if (CheckIfMovementPossible(Vector3.left))
        {
            possibleMovements.Add(Vector3.left);
        }

        if (CheckIfMovementPossible(Vector3.right))
        {
            possibleMovements.Add(Vector3.right);
        }

        if (CheckIfMovementPossible(Vector3.up))
        {
            possibleMovements.Add(Vector3.up);
        }

        if (CheckIfMovementPossible(Vector3.down))
        {
            possibleMovements.Add(Vector3.down);
        }

        return possibleMovements;
    }
}
