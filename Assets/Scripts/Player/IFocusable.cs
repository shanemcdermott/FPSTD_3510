using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CursorFocus
{
    Tile, Wall, Tower, Enemy, Default
}

public interface IFocusable {
    void onBeginFocus(PlayerController focuser);
    void onEndFocus(PlayerController focuser);
}
