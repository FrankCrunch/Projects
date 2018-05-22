using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour {

    //Класс скрывает считывание нажатий клавиш игроками.
    public int layout = 0;

    public Vector2 HeadMoves()
    {
        //Функции возвращает вектор направления движения, в зависимости от нажатия клавиш игроком.
        switch (layout)
        {
            case 0: return new Vector2(Input.GetAxisRaw("(Keyboard layout) Head movement X"), Input.GetAxisRaw("(Keyboard layout) Head movement Y"));
            case 1: return new Vector2(Input.GetAxisRaw("(Joystick 1 layout) Head movement X"), Input.GetAxisRaw("(Joystick 1 layout) Head movement Y"));
        }
        return new Vector2(0, 0);
    }

    public Vector2 AssMoves()
    {
        //Функции возвращает вектор направления движения, в зависимости от нажатия клавиш игроком.
        switch (layout)
        {
            case 0: return new Vector2(Input.GetAxisRaw("(Keyboard layout) Ass movement X"), Input.GetAxisRaw("(Keyboard layout) Ass movement Y"));
            case 1: return new Vector2(Input.GetAxisRaw("(Joystick 1 layout) Ass movement X"), Input.GetAxisRaw("(Joystick 1 layout) Ass movement Y"));
        }
        return new Vector2(0, 0);
    }

    public bool Fart()
    {
        switch (layout)
        {
            case 0: return Input.GetButtonDown("(Keyboard layout) Fart");
            case 1: return Input.GetButtonDown("(Joystick 1 layout) Fart");
        }
        return false;
    }

    public bool Bite()
    {
        switch (layout)
        {
            case 0: return Input.GetButtonDown("(Keyboard layout) Bite");
            case 1: return Input.GetButtonDown("(Joystick 1 layout) Bite");
        }
        return false;
    }
}
