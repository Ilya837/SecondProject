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

        public  bool selected;// Cостояние открывающегося окна
        PictureBox picture ; // Выбранное изображение

        int Xb, Yb; // Координаты кнопки
        int Wb, Hb; // Длинна и высота кнопки
        Point[] triangle = new Point[3]; 
        
        HScrollBar Hs; // Скролл изображений в открывшемся окне
        int Sh = 20,Sw; // Размеры скролла

        int count = 10;// Колличество картинок из файла
        int place ; // число картинок, помещающихся в выдвигающейся панельке


        public Imagelist(int x,int y,int w,int h,Form1 f)
        {
            X = x;
            Y = y;
            W = w;
            H = h;
            parent = f;

            Sw = W;

            Line = Color.Black;
            Fill = Color.White;

            picture = new PictureBox();
            picture.Width = Math.Min(2 * w / 3 - 2*d, h - 2 * d);
            picture.Height = picture.Width;
            picture.Left = x + d;
            picture.Top = y + (h - picture.Height) / 2;
            picture.SizeMode = PictureBoxSizeMode.Zoom;
            picture.Image = Image.FromFile("uk.jpg");

           

            Xb = picture.Left + picture.Width + d;
            Yb = picture.Top;
            Wb = w - picture.Width - 3 * d;
            Hb = picture.Height ;

            triangle[0] = new Point(Xb + 4,Yb + 4);
            triangle[1] = new Point(Xb + Wb - 4, Yb + 4);
            triangle[2] = new Point(Xb + Wb / 2, Yb + Hb - 3);

            place = W / (10 + picture.Width) + 1;

            parent.Controls.Add(picture);
            picture.Invalidate();
        }

        public void Draw(Graphics graph)
        {
            Pen pen = new Pen(Line);
            SolidBrush brush = new SolidBrush(Fill);
            if (selected == true)
            {
                if (!parent.Controls.Contains(Hs))
                {
                    graph.FillRectangle(brush, X, Y + H, W, picture.Height + 2 * d);
                     // Рисуем вылезающее окно
                    Hs = new HScrollBar();
                    Hs.Height = Sh ;
                    Hs.Width = W - 1;
                    Hs.Location = new Point(X + 1, Y + H + picture.Height + 2 * d);
                    this.Hs.Scroll += new System.Windows.Forms.ScrollEventHandler(this.Hs_Scroll);
                    Hs.Maximum = count * picture.Width + d * (count + 1) - H;
                    parent.Controls.Add(Hs);
                    graph.DrawRectangle(pen, X , Y + H , W, picture.Height + Sh + 2 * d);
                }
                else
                {
                    graph.FillRectangle(brush, X, Y + H, W, picture.Height + 2 * d);
                    // Рисуем вылезающее окно
                    graph.DrawRectangle(pen, X, Y + H, W, picture.Height + Sh + 2 * d);
                }
                
            }
            else
            {
                parent.Controls.Remove(Hs);
            }
           

            graph.FillRectangle(brush, X, Y, W, H);
            graph.DrawRectangle(pen, X, Y, W, H); // Рисуем основу
            
            graph.FillRectangle(Brushes.LightGray, Xb, Yb, Wb, Hb);
            graph.FillPolygon(Brushes.Gray,triangle);
            graph.DrawRectangle(Pens.Black, Xb, Yb, Wb, Hb); // Рисуем кнопку
           

            // picture.Invalidate();

        }

        public void Select(bool value)
        {
            selected = value;
            
        }

        internal bool isOpen(int x, int y)
        {
            if (x <= X + W && x >= X && y >= Y + H && y <= Y + H + picture.Height + Sh)
            {
                return true;
            }
            return false;
        }

        public bool isButton(int x, int y)
        {
            if(x<= Xb+Wb && x>= Xb && y>= Yb && y<= Yb + Hb)
            {
                return true;
            }
            return false;
        }

        private void Hs_Scroll(object sender, ScrollEventArgs e)
        {

            HScrollBar hs = (HScrollBar) sender;
            int os = hs.Value % (10 + picture.Width);
            if(os < 10)
            {
                int pr = d - os;
                for(int i = 0; i< place; i++)
                {
                    // 
                }
            }
            else
            {
                int pr = os - d;
                for(int i = 0; i < place; i++)
                {
                    //
                }
            }
        }
    }
}
