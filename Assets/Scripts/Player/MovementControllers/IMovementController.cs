﻿using UnityEngine;

namespace MovementControllers
{
    public interface IMovementController
    {
        void Move(Vector2 _wantedDirection);

        void Jump(bool _buttonPressed);

        bool IsOnGround();
    }
}