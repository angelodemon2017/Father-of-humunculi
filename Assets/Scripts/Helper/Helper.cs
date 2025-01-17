﻿using System;
using UnityEngine;
using static SimpleExtensions;

public static class Helper
{
    public static Vector2 GetRandomPointOfCircle(float maxRadius)
    {
        return GetRandomPointOfCircle(0f, maxRadius);
    }

    public static Vector2 GetRandomPointOfCircle(float minRadius, float maxRadius)
    {
        return PointOnCircumference(0f, 0f, GetRandom(minRadius, maxRadius), GetRandom(0, 360));
    }

    public static Vector2 PointOnCircumference(float centerX, float centerY, float radius, float angleInDegrees)
    {
        float angleInRadians = angleInDegrees * (float)(Math.PI / 180);

        float x = centerX + radius * (float)Math.Cos(angleInRadians);
        float y = centerY + radius * (float)Math.Sin(angleInRadians);

        return new Vector2(x, y);
    }
}