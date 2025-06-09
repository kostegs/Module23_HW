using UnityEngine;

public class Health
{
    private int _value;

    public int Value => _value;

    public Health(int value)
    {
        _value = value;
    }

    public void Reduce(int value)
    {
        if (_value < 0)
        {
            Debug.LogError("ѕоказать уменьшени€ здоровь€ меньше нул€");
            return;
        }

        _value -= value;

        if(_value < 0) 
            _value = 0;
    }
}
