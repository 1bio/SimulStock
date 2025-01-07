using System.Diagnostics;
using System.Numerics;
using GameServer;

class Player
{
    public int id;
    public string username;
    
    public Vector3 position;
    public Quaternion rotation;

    private float moveSpeed = 5f / Constants.TICKS_PER_SEC;
    private bool[] inputs;

    public int vAxis;
    public int hAxis;

    public Player(int _id, string _username, Vector3 _spawnPosition){
        id = _id;
        username = _username;
        position = _spawnPosition;
        rotation = Quaternion.Identity;

        inputs = new bool[4];
    }

    public void Update() {
        Vector2 _inputDirection = Vector2.Zero;
        
        if (inputs[0]){
            _inputDirection.Y += 1;
        }
        if (inputs[1])
        {
            _inputDirection.Y -= 1;
        }
        if (inputs[2])
        {
            _inputDirection.X -= 1;
        }
        if (inputs[3])
        {
            _inputDirection.X += 1;
        }

        Move(_inputDirection);

        SetAnimationState(_inputDirection);
    }

    /* // 3D 
    private void Move(Vector2 _inputDirection)
    {
        Vector3 _forward = Vector3.Transform(new Vector3(0, 0, 1), rotation);
        Vector3 _right = Vector3.Normalize(Vector3.Cross(_forward, new Vector3(0, 1, 0)));

        Vector3 _moveDirection = _right * _inputDirection.X + _forward * _inputDirection.Y;
        position += _moveDirection * moveSpeed;

        ServerSend.PlayerPosition(this);
        ServerSend.PlayerRotation(this);
    } */

    // 2D
    private void Move(Vector2 _inputDirection)
    {
        Vector3 _up = Vector3.Transform(new Vector3(0, 1, 0), Quaternion.Identity);
        Vector3 _right = Vector3.Transform(new Vector3(1, 0, 0), Quaternion.Identity);

        Vector3 _moveDirection = _up * _inputDirection.Y + _right * _inputDirection.X;
        position += _moveDirection * moveSpeed;

        ServerSend.PlayerPosition(this);
    }

    public void SetInput(bool[] _inputs, Quaternion _rotation)
    {
        inputs = _inputs;
        rotation = _rotation;
    }

    public void SetAxis(int _vAxis, int _hAxis)
    {
        vAxis = _vAxis;
        hAxis = _hAxis;
    }

    public void SetAnimationState(Vector2 _inputDirection)
    {
        // 이동 방향에 따라 vAxis, hAxis 설정
        if (_inputDirection.Y > 0)
        {
            vAxis = 1;  // 위쪽
        }
        else if (_inputDirection.Y < 0)
        {
            vAxis = -1; // 아래쪽
        }
        else
        {
            vAxis = 0;  // 이동 없음
        }

        if (_inputDirection.X > 0)
        {
            hAxis = 1;  // 오른쪽
        }
        else if (_inputDirection.X < 0)
        {
            hAxis = -1; // 왼쪽
        }
        else
        {
            hAxis = 0;  // 이동 없음
        }

        ServerSend.PlayerAnimation(this);
    }
}