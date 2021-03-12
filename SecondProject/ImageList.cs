using System;
using System.Drawing;

namespace SecondProject
{
    class ImageList
    {
        int X, Y; // Координаты
        int W, H; // Размер
        Color Line;// Цвет контура
        Color Fill;// Цвет закраски
        bool selected;// Состояние выделенности

        public ImageList(int x,int y,int w,int h)
        {
            X = x;
            Y = y;
            W = w;
            H = h;
            Line = Color.Black;
            Fill = Color.Red;
        }

        public void Draw(Graphics graph)
        {
            Pen pen = new Pen(Line);
            if(selected == true)
            {
                pen.Color = Color.Green;
            }
            SolidBrush brush = new SolidBrush(Fill);

            graph.FillRectangle(brush, X, Y, W, H);
            graph.DrawRectangle(pen, X, Y, W, H);
        }

        public void Select(bool value)
        {
            selected = value;
            //Перерисовать
            
        }

        public bool isInside(int x, int y)
        {
            if(x<= X+W && x>= X && y>= Y && y<= Y + H)
            {
                return true;
            }
            return false;
        }
    }
}
