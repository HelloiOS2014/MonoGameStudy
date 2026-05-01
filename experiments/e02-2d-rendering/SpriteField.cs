using System;
using Microsoft.Xna.Framework;

namespace E02Rendering;

public static class SpriteField
{
    public static Vector2[] Create(int spriteCount, int viewportWidth, int viewportHeight, int spriteSize)
    {
        if (spriteCount < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(spriteCount));
        }

        var positions = new Vector2[spriteCount];
        var columns = Math.Max(1, (viewportWidth - spriteSize * 2) / spriteSize);
        var horizontalGap = Math.Max(1, (viewportWidth - spriteSize * 2) / columns);
        var verticalGap = spriteSize;
        var xMargin = spriteSize;
        var yMargin = spriteSize * 3;

        for (var i = 0; i < positions.Length; i++)
        {
            var column = i % columns;
            var row = i / columns;
            var x = xMargin + column * horizontalGap;
            var y = yMargin + row * verticalGap;
            if (y > viewportHeight - spriteSize)
            {
                y = yMargin + row % Math.Max(1, (viewportHeight - yMargin - spriteSize) / verticalGap) * verticalGap;
            }

            positions[i] = new Vector2(x, y);
        }

        return positions;
    }
}
