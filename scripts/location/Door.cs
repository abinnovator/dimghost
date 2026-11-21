using Godot;

public partial class Door: Node3D {
    [Export] public Area3D _detectionArea;
    [Export] public Label3D OpenLabelOut;
    [Export] public Label3D OpenLabelIn;
    [Export] public Node3D visual;
    private bool isDoorOpen = false;
    private bool _isPlayerNear = false;

    public override void _Ready()
    {
        _detectionArea.BodyEntered += OnBodyEntered;
        _detectionArea.BodyExited += OnBodyExited;
        OpenLabelIn.Visible = false;
        OpenLabelOut.Visible = false;

    }
    public override void _UnhandledInput(InputEvent @event){
        if (@event.IsActionPressed("interact")){
            if (isDoorOpen){
                visual.Rotate(Vector3.Up, Mathf.DegToRad(-90));
            }else{
                visual.Rotate(Vector3.Up, Mathf.DegToRad(90));
            }
        }
    }
    private void OnBodyEntered(Node3D Body) {
        if (Body.Name == "Player"){
            OpenLabelIn.Visible = true;
            OpenLabelOut.Visible = true;
            _isPlayerNear = true;
        }
    }
    private void OnBodyExited(Node3D Body)
    {
        OpenLabelIn.Visible = false;
        OpenLabelOut.Visible = false;
        _isPlayerNear = false;
    }

}
