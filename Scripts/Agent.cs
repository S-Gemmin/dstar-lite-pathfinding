using UnityEngine;

public class Agent : MonoBehaviour
{
    [SerializeField] private GridVisual pfGridVisual;
    [SerializeField] private int width = 20;
    [SerializeField] private int height = 10;
    [SerializeField] private float speed = 10f;

    private Grid grid;
    private GridVisual gridVisual;
    private DStarLite dStarLite;
    private Camera cam;
    private Vertex v_goal;
    private Vertex next;

    private void Awake()
    {
        grid = new Grid(width, height);
        dStarLite = new DStarLite(grid);
        cam = Camera.main;
    }

    private void Start()
    {
        gridVisual = Instantiate(pfGridVisual);
        gridVisual.Setup(grid);
    }

    private void Update()
    {
        HandleMovement();
        HandleInput();

        dStarLite.UpdateAgentPosition(transformPositionVertex);
    }

    private Vertex transformPositionVertex => 
        gridVisual.GetVertex(transform.position);
    
    private Vertex mouseInputVertex => 
        gridVisual.GetVertex(cam.ScreenToWorldPoint(Input.mousePosition));

    private void HandleMovement()
    {
        if (v_goal == null && next == null || Vector2.Distance(transform.position, gridVisual.GetWorldPosition(v_goal)) < 0.05f) return;

        if (next == null || Vector2.Distance(transform.position, gridVisual.GetWorldPosition(next)) < 0.05f)
            next = dStarLite.FindNext(transformPositionVertex);

        if (next != null) // could still be null if path does not exist
            transform.position = Vector2.MoveTowards(transform.position, gridVisual.GetWorldPosition(next), speed * Time.deltaTime);
    }

    private void HandleInput()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Vertex v_start = transformPositionVertex;
            Vertex v_goal = mouseInputVertex;

            if (v_start.isWalkable && v_goal.isWalkable)
            {
                this.v_goal = mouseInputVertex;
                dStarLite.FindPath(v_start, v_goal);
                gridVisual.HighlightTargetVertex(v_goal);
                Debug.Log($"Finding Path from {v_start.x}, {v_start.y} to {v_goal.x}, {v_goal.y}.");
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Vertex vertex = mouseInputVertex;

            if (next != vertex && v_goal != vertex)
            {
                vertex.SetIsWalkable(!vertex.isWalkable);
                dStarLite.ChangeVertex(vertex);
                gridVisual.UpdateVertexVisual(vertex);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
            gridVisual.ShowNextSnapshot();
    }
}
