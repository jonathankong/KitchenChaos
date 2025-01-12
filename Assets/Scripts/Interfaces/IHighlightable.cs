using UnityEngine;

public interface IHighlightable
{
    bool IsHighlighted { get; }
    void OnHighlight();
    void OnUnhighlight();
}
