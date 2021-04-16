using System;
using System.Drawing;
using System.Windows.Forms;

namespace SecondProject
{
    public partial class Form1 : Form
    {
        Imagelist il1, il2; // Списки изображений

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics graph = this.CreateGraphics();
            il1.Draw(graph);
            
          //  il2.Draw(graph);
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            
            
            if (il1.isButton(e.X, e.Y) || (il1.selected == true && il1.isOpen(e.X,e.Y)))
            {
                il1.Select(true);
                Invalidate();
            }
            else
            {
                il1.Select(false);
                Invalidate();
            }
            

            //if (il2.isInside(e.X, e.Y))
            //{
            //    il2.Select(true);
            //    Invalidate();
            //}
            //else
            //{
            //    il2.Select(false);
            //    Invalidate();
            //}
        }

        public Form1()
        {
            InitializeComponent();

            il1 = new Imagelist(100,100,200,150,this);
           // il2 = new ImageList(100, 100, 75, 98);
        }
    }
}
