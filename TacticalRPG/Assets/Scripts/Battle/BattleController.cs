using UnityEngine;
using System.Collections.Generic;
using TacticalRPG.Map;
using TacticalRPG.View;
using TTacticalRPG.Map;
using UnityEngine.InputSystem;


namespace TacticalRPG.Battle
{

    public class BattleController : MonoBehaviour
    {
        [SerializeField] private MapLoader mapLoader;
        [SerializeField] private MapView mapView;
        [SerializeField] private Transform unit;
        [SerializeField] private Vector2Int unitStartCell;
        [SerializeField] private int unitMovement = 4;

        private BattleMap _map;
        private Camera _camera;
        private Vector2Int _unitCell;
        private Dictionary<Vector2Int, int> _reachable; // null = nada selecionado

        private void Start()
        {
            _map = mapLoader.Map;
            _camera = Camera.main;
            _unitCell = unitStartCell;

            if (TerrainCosts.GetWalkingCost(_map.GetCell(_unitCell).BaseTerrain) == null)
            {
                Debug.LogError($"A unidade começa em {_unitCell}, que é um terreno bloqueado.");
            }

            unit.position = mapView.CellToWorldCenter(_unitCell);
        }

        private void Update()
        {
            Mouse mouse = Mouse.current;
            if (mouse == null || !mouse.leftButton.wasPressedThisFrame)
            {
                return;
            }

            Vector3 world = _camera.ScreenToWorldPoint(mouse.position.ReadValue());
            Vector2Int clicked = mapView.WorldToCell(world);

            if (!_map.IsInside(clicked))
            {
                Deselect();
            }
            else if (clicked == _unitCell)
            {
                Select();
            }
            else if (_reachable != null && _reachable.ContainsKey(clicked))
            {
                MoveUnitTo(clicked);
            }
            else
            {
                Deselect();
            }
        }

        private void Select()
        {
            _reachable = Pathfinder.GetReachableCells(_map, _unitCell, unitMovement);
            mapView.ShowReachable(_reachable.Keys);
        }

        private void MoveUnitTo(Vector2Int cell)
        {
            _unitCell = cell;
            unit.position = mapView.CellToWorldCenter(cell);
            Deselect();
        }

        private void Deselect()
        {
            _reachable = null;
            mapView.ClearHighlight();
        }

    }
}
