using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LockType {
    Cyan, Red, White
}

public static class LockTypeToColor {
    private static readonly Color[] COLORS = new Color[] { Color.cyan, new Color(0.5f, 0, 0), Color.white };

    public static Color Convert(LockType lockType) {
        return COLORS[(int) lockType];
    }
}