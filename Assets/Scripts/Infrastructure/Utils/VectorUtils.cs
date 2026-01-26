using System;
using UnityEngine;

namespace FormForge.Utils
{
	/// <summary>
	/// This is a static utility class which provides extra functions for working with Unity's built in vector types.
	/// </summary>
    public static class VectorUtils
    {
        /// <summary>
        /// Returns the absolute value of each component of a Vector2.
        /// </summary>
        public static Vector2 Abs(Vector2 value)
        {
            return new Vector2(
                value.x < 0 ? -value.x : value.x,
                value.y < 0 ? -value.y : value.y);
        }

        /// <summary>
        /// Returns the absolute value of each component of a Vector3.
        /// </summary>
        public static Vector3 Abs(Vector3 value)
        {
            return new Vector3(
                value.x < 0 ? -value.x : value.x,
                value.y < 0 ? -value.y : value.y,
                value.z < 0 ? -value.z : value.z);
        }

        /// <summary>
        /// Returns the absolute value of each component of a Vector4.
        /// </summary>
        public static Vector4 Abs(Vector4 value)
        {
            return new Vector4(
                value.x < 0 ? -value.x : value.x,
                value.y < 0 ? -value.y : value.y,
                value.z < 0 ? -value.z : value.z,
                value.w < 0 ? -value.w : value.w);
        }

        /// <summary>
        /// Determines the intersection point of a vector with a line segment.
        /// </summary>
        public static bool VectorLineIntersection(Vector2 center, Vector2 direction, Vector2 lineStart, Vector2 lineEnd, out Vector2 intersection)
        {
            Vector2 lineDirection = lineEnd - lineStart;
            float det = (direction.y * lineDirection.x - direction.x * lineDirection.y);

            if (det == 0.0f)
            {
                intersection = Vector2.zero;
                return false;
            }

            float lineCenterY = lineStart.y - center.y;
            float lineCenterX = center.x - lineStart.x;

            float directionFactor = (lineDirection.x * lineCenterY + lineDirection.y * lineCenterX) / det;
            if (directionFactor < 0.0f)
            {
                intersection = Vector2.zero;
                return false;
            }

            float lineFactor = (direction.x * lineCenterY + direction.y * lineCenterX) / det;
            if (lineFactor < 0.0f || lineFactor > 1.0f)
            {
                intersection = Vector2.zero;
                return false;
            }

            intersection = lineStart + lineFactor * lineDirection;
            return true;
        }

        /// <summary>
        /// Packs two float values into a single float variable. See here: https://stackoverflow.com/questions/17638800/storing-two-float-values-in-a-single-float-variable/17639008
        /// </summary>
        public static float Pack(Vector2 input, int precision = 4096)
        {
            Vector2 output = input;
            output.x = Mathf.Floor(output.x * (precision - 1));
            output.y = Mathf.Floor(output.y * (precision - 1));
            return output.x * precision + output.y;
        }

        /// <summary>
        /// Unacks two float values into a single float variable. 
        /// </summary>
        public static Vector2 Unpack(float input, int precision = 4096)
        {
            Vector2 output = Vector2.zero;
            output.y = input % precision;
            output.x = Mathf.Floor(input / precision);
            return output / (precision - 1);
        }

        /// <summary>
        /// Converts an angle to a normalized vector.
        /// </summary>
        static public Vector3 GetNormalizedVectorFromAngle(float unityAngle)
        {
            double angle = Mathf.PI * (90f - unityAngle) / 180.0;
            double sinAngle = Math.Sin(angle);
            double cosAngle = Math.Cos(angle);

            float x = -(float)cosAngle;
            float z = (float)sinAngle;
            Vector3 v = new Vector3(-x, 0, z);
            v.Normalize();
            return v;
        }

        /// <summary>
        /// Rotates an angle towards a target angle.
        /// </summary>
        static public float RotateTowardsAngle(float _currentAngle, float _targetAngle, float rotationSpeed, bool debugtrace = false, float _deltaTime = -1f, bool pctRotationSpeed = false)
        {
            float currentAngle = AdjustAngle(_currentAngle);
            float targetAngle = AdjustAngle(_targetAngle);

            float delta = GetAngleDifference(currentAngle, targetAngle, false);

            if (debugtrace)
            {
                Debug.Log($"# C: {currentAngle} T: {targetAngle} (D: {delta})");
            }

            if (pctRotationSpeed)
            {
                rotationSpeed = System.Math.Abs(delta) * rotationSpeed;
            }

            // Rotate toward target
            if (delta == 0)
            {
                return currentAngle;
            }
            else if (delta > 0)
            {
                if (currentAngle > targetAngle)
                {
                    currentAngle -= 360f;
                }
                currentAngle += rotationSpeed * _deltaTime;
                if (currentAngle > targetAngle)
                {
                    currentAngle = targetAngle;
                }
            }
            else
            {
                if (currentAngle < targetAngle)
                {
                    currentAngle += 360f;
                }
                currentAngle -= rotationSpeed * _deltaTime;
                if (currentAngle < targetAngle)
                {
                    currentAngle = targetAngle;
                }
            }
            return currentAngle;

        }

        /// <summary>
        /// Rotates a vector of angles towards a target vector of angles.
        /// </summary>
        static public Vector3 RotateTowardsAngle(Vector3 _currentAngle, Vector3 _targetAngle, float rotationSpeed, bool debugtrace = false, float _deltaTime = -1f, bool pctRotationSpeed = false)
        {
            Vector3 result = Vector3.zero;
            result.x = RotateTowardsAngle(_currentAngle.x, _targetAngle.x, rotationSpeed, debugtrace, _deltaTime, pctRotationSpeed);
            result.y = RotateTowardsAngle(_currentAngle.y, _targetAngle.y, rotationSpeed, debugtrace, _deltaTime, pctRotationSpeed);
            result.z = RotateTowardsAngle(_currentAngle.z, _targetAngle.z, rotationSpeed, debugtrace, _deltaTime, pctRotationSpeed);
            return result;
        }
        
        /// <summary>
        /// Return the 0-360 value of an angle
        /// </summary>
        static public float AdjustAngle(float _angle)
        {
            float angle = _angle % 360f;
            if (angle < 0f)
            {
                angle += 360f;
            }
            return angle;
        }

        /// <summary>
        /// Returns the difference in angles
        /// </summary>
        static public float GetAngleDifference(float current, float target, bool abs = true)
        {
            float angle = AdjustAngle(target) - AdjustAngle(current);
            if (angle > 180f)
            {
                angle -= 360f;
            }
            else if (angle < -180f)
            {
                angle += 360f;
            }

            if (abs)
            {
                return Math.Abs(angle);
            }
            return angle;
        }

        /// <summary>
        /// Interpolates between two vectors of angles.
        /// </summary>
        public static Vector3 AngleLerp(Vector3 from, Vector3 to, float t)
        {
            from.x = AngleLerp(from.x, to.x, t);
            from.y = AngleLerp(from.y, to.y, t);
            from.z = AngleLerp(from.z, to.z, t);
            return from;
        }

        /// <summary>
        /// Interpolates between two angles.
        /// </summary>
        public static float AngleLerp(float from, float to, float t)
        {
            float diff = to - from;
            if (diff > 180f) { diff -= 360f; }
            if (diff < -180f) { diff += 360f; }
            return from + Mathf.Lerp(0, diff, t);
        }
    }
}
