using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.IO;


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

        PictureBox[] list; // массив PictureBox -ов , использущихся в выдвигающейся части
        bool end; // нужно-ли отрисовывать последнюю картинку ?

        Image[] imlist; // массив изображений из файла

        public object Image_MouseClick { get; private set; }

        public Imagelist(int x,int y,int w,int h,Form1 f, string filename)
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
            

            

            Xb = picture.Left + picture.Width + d;
            Yb = picture.Top;
            Wb = w - picture.Width - 3 * d;
            Hb = picture.Height ;

            triangle[0] = new Point(Xb + 4,Yb + 4);
            triangle[1] = new Point(Xb + Wb - 4, Yb + 4);
            triangle[2] = new Point(Xb + Wb / 2, Yb + Hb - 3);

            place = (int) ( Math.Ceiling(((double)W) / ((double)(d + picture.Width)))) + 1;

            list = new PictureBox[place];
            for (int i = 0; i < place ; i++)
            {
                list[i] = new PictureBox();
            }

            parent.Controls.Add(picture);
            picture.Invalidate();

            FileStream file = new FileStream(filename, FileMode.Open);
            byte[] array = new byte[file.Length];
            file.Read(array, 0, array.Length);
            string textFromFile = System.Text.Encoding.Default.GetString(array);
            string[] Im = textFromFile.Split('\r');
            for (int i = 1; i< Im.Length;i++)
            {
                Im[i] = Im[i].Trim();
            }
            imlist = new Image[Im.Length];
            count = Im.Length;
            for (int i = 0;i< Im.Length; i++)
            {
                imlist[i] = Image.FromFile(Im[i]);

                Image result = new Bitmap(picture.Width, picture.Height);
                using (Graphics g = Graphics.FromImage((Image)result))
                {
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic; 
                    g.DrawImage(imlist[i], 0, 0, picture.Width, picture.Height);
                    g.Dispose();
                }
                imlist[i] = result;

            }
            file.Close();
            picture.Image = imlist[0];

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
                    Hs.Maximum = count * (picture.Width + d ) + d - W;
                    parent.Controls.Add(Hs);
                    graph.DrawRectangle(pen, X , Y + H , W, picture.Height + Sh + 2 * d);

                    end = false;
                   
                    
                    list[0].BackColor = Color.Black;
                    list[0].Location = new Point(X + d, Y + H + d);
                    list[0].Height = picture.Height;
                    list[0].Width = picture.Width;
                    list[0].Image = imlist[0];
                    list[0].SizeMode = PictureBoxSizeMode.Zoom;
                    list[0].Click += new System.EventHandler(this.Image_Click);
                    parent.Controls.Add(list[0]);

                    for (int i = 1; i < place - 1; i++)
                    {
                        
                        list[i].BackColor = Color.Black;
                        list[i].Location = new Point(X + d + (d + picture.Width) * i, Y + H + d);
                        list[i].Height = picture.Height;
                        if (d + (d + picture.Width) * i + picture.Width <= W)
                        {
                            list[i].Width = picture.Width;
                            list[i].Image = imlist[i];
                            list[i].SizeMode = PictureBoxSizeMode.Zoom;
                        }
                        else
                        {
                            list[i].Width = W - (d + picture.Width) * i - d;
                            if (i < (imlist.Length))
                            {
                                list[i].Image = imlist[i];
                            }
                           
                            list[i].SizeMode = PictureBoxSizeMode.StretchImage;
                            end = true;
                        }
                        parent.Controls.Add(list[i]);
                        list[i].Click += new System.EventHandler(this.Image_Click);
                    }
                    if (end == false)
                    {
                        list[place - 1].BackColor = Color.Black;
                        list[place - 1].Location = new Point(X + d + (d + picture.Width) * (place - 1), Y + H + d);
                        list[place - 1].Height = picture.Height;
                        list[place - 1].Width = W - (d + picture.Width) * (place - 1) - d;
                        if (place - 1  < (imlist.Length))
                        {
                            list[place - 1].Image = imlist[place - 1];
                        }
                        list[place - 1].SizeMode = PictureBoxSizeMode.StretchImage;
                        parent.Controls.Add(list[place - 1]);
                        list[place - 1].Click += new System.EventHandler(this.Image_Click);
                    }
                    list[place - 1].Click += new System.EventHandler(this.Image_Click);

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
                for(int i = 0; i< place; i++)
                {
                    parent.Controls.Remove(list[i]);
                }
            }
           

            graph.FillRectangle(brush, X, Y, W, H);
            graph.DrawRectangle(pen, X, Y, W, H); // Рисуем основу
            
            graph.FillRectangle(Brushes.LightGray, Xb, Yb, Wb, Hb);
            graph.FillPolygon(Brushes.Gray,triangle);
            graph.DrawRectangle(Pens.Black, Xb, Yb, Wb, Hb); // Рисуем кнопку
           

            // picture.Invalidate();

        }

        private void Image_Click(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)sender;
            picture.Image = pb.Image;
            selected = false;
            parent.Invalidate();
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

            int de = hs.Value % (d + picture.Width);

            int a = hs.Value / (picture.Width + d);

            end = false;

            if(de <= 10)
            {
                list[0].Location = new Point(X + d - de, Y + H + d );
                list[0].Height = picture.Height;
                list[0].Width = picture.Width;
                list[0].Image = imlist[a];
                list[0].SizeMode = PictureBoxSizeMode.Zoom;
            }
            else
            {
                list[0].Location = new Point(X , Y + H + d );
                list[0].Height = picture.Height;
                list[0].Width = picture.Width - (de - 10);
                list[0].Image = imlist[a];
                list[0].SizeMode = PictureBoxSizeMode.StretchImage;
            }
            


            for (int i = 1; i < place - 1; i++)
            {
                list[i].BackColor = Color.Black;
                list[i].Location = new Point(X + d + (d + picture.Width) * i - de, Y + H + d);
                list[i].Height = picture.Height;
                if ((d + picture.Width) * (i + 1) - de <= W)
                {
                    list[i].Width = picture.Width;
                    list[i].Image = imlist[i + a];
                    list[i].SizeMode = PictureBoxSizeMode.Zoom;
                }
                else
                {
                    list[i].Width = W - (d + picture.Width) * i + de - d;
                    list[i].Image = imlist[i + a];
                    list[i].SizeMode = PictureBoxSizeMode.StretchImage;
                    end = true;
                }
            }
            if (end == false)
            {
                list[place - 1].BackColor = Color.Black;
                list[place - 1].Location = new Point(X + d + (d + picture.Width) * (place - 1) - de, Y + H + d);
                list[place - 1].Height = picture.Height;
                list[place - 1].Width = W - (d + picture.Width) * (place - 1) - d + de;
                if (place - 1 + a < (imlist.Length ))
                {
                    list[place - 1].Image = imlist[place - 1 + a];
                }
                list[place - 1].SizeMode = PictureBoxSizeMode.StretchImage;
                if ( !parent.Controls.Contains(list[place - 1]))
                    parent.Controls.Add(list[place - 1]);
            }
            else
            {
                parent.Controls.Remove(list[place - 1]);
            }
        }
    }
}
