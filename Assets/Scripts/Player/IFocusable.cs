using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFocusable {
    void onBeginFocus(GameObject go);
    void onEndFocus(GameObject go);
}
