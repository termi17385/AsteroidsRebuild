using UnityEngine;

namespace Asteroids.Scripts
{
    public static class MovementUtils
    {
        public static void Thrust(float _speed, Rigidbody2D _rb2d, Transform _transform)
        {
            _rb2d.AddForce(_transform.up * _speed * Time.fixedDeltaTime);
            _rb2d.velocity = Vector2.ClampMagnitude(_rb2d.velocity, 25);
        }
    }
}