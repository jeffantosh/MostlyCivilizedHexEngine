using System;

public class MapObject
{
    public MapObject()
    {
        Guid = Guid.NewGuid();
    }

    public string Name;
    public readonly Guid Guid;
    public int HitPoints = 100;
    public bool CanBeAttacked = true;
    public int FactionID = 0;

    public bool IsDestroyed { get; private set; }

    public Hex Hex  { get; protected set; }

    public delegate void ObjectMovedDelegate ( Hex oldHex, Hex newHex );
    public event ObjectMovedDelegate OnObjectMoved;

    public delegate void ObjectDestroyedDelegate ( MapObject mo );
    public event ObjectDestroyedDelegate OnObjectDestroyed;

    /// <summary>
    /// This object is being removed from the map/game
    /// </summary>
    virtual public void Destroy()
    {
        IsDestroyed = true;

        OnObjectDestroyed?.Invoke(this);
    }

    virtual public void SetHex( Hex newHex )
    {
        Hex oldHex = Hex;

        Hex = newHex;

        OnObjectMoved?.Invoke(oldHex, newHex);
    }

    public bool Equals(MapObject obj)
    {
        return this.Guid == obj.Guid;
    }

}


