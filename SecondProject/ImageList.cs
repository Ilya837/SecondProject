using System;
using System.Drawing;
using System.Windows.Forms;

namespace SecondProject
{
    class Imagelist
    {
        Form1 parent; // Родительская форма
        int d = 10; // Отступ между эллементами
        int X, Y; // Координаты
        int W, H; // Размер
        Color Line;// Цвет контура
        Color Fill;// Цвет закраски
        bool selected;// Состояние выделенности
        PictureBox picture ; // Выбранное изображение

        public Imagelist(int x,int y,int w,int h,Form1 f)
        {
            X = x;
            Y = y;
            W = w;
            H = h;
            parent = f;

            Line = Color.Black;
            Fill = Color.Red;

            picture = new PictureBox();
            picture.Left = x + d;
            picture.Top = y + d;
            picture.Width = 2/3 * w - 2*d;
            picture.Height = h - 2 * d;
            picture.SizeMode = PictureBoxSizeMode.Zoom;
            picture.Image = Image.FromFile("uk.jpg");
            
            parent.Controls.Add(picture);
            picture.Invalidate();
        }

        public void Draw(Graphics graph)
        {
            Pen pen = new Pen(Line);
            if(selected == true)
            {
                pen.Color = Color.Green;
            }
            SolidBrush brush = new SolidBrush(Fill);

           
            graph.DrawRectangle(pen, X, Y, W, H);

            picture.Invalidate();

        }

        public void Select(bool value)
        {
            selected = value;
            
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
