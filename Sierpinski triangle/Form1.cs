using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sierpinski_triangle
{
    public partial class Form1 : Form
    {
        Brush bBlack = Brushes.Black;
        Brush bWhite = Brushes.White;
        int NumberOfIterations = 0;
        List<List<Triangle>> ListOfSetsOfTriangles = new List<List<Triangle>>(); 

        #region Initialization

        public Form1()
        {
            InitializeComponent();
            ResetListOfSetsOfTriangles();
        }

        private void ResetListOfSetsOfTriangles()
        {
            ListOfSetsOfTriangles.Clear();
            Triangle FirstTriangle = new Triangle(new Point(300, 0), new Point(0, 599), new Point(599, 599));
            List<Triangle> FirstSetOfTriangles = new List<Triangle>();
            FirstSetOfTriangles.Add(FirstTriangle);
            ListOfSetsOfTriangles.Add(FirstSetOfTriangles);
        }

        #endregion

        #region Shape Calculations

        public Point FindMiddlePoint (Point first, Point second)
        {
            Point result = new Point( (first.X + second.X) / 2, (first.Y + second.Y) / 2);

            return result;
        }

        public List<Triangle> CreateNextSetOfTriangles(List<Triangle> previousSet)
        {
            List<Triangle> NewSet = new List<Triangle>();

            foreach (Triangle x in previousSet)
            {
                NewSet.Add(new Triangle( x.Top,                           FindMiddlePoint(x.Left, x.Top),   FindMiddlePoint(x.Right, x.Top) ));
                NewSet.Add(new Triangle( FindMiddlePoint(x.Top, x.Left) , x.Left,                           FindMiddlePoint(x.Right, x.Left) ));
                NewSet.Add(new Triangle( FindMiddlePoint(x.Top, x.Right), FindMiddlePoint(x.Left, x.Right), x.Right ));
            }

            return NewSet;
        }

        public Triangle GetSubsetCordsOfTriangle(Triangle x)
        {
            Triangle result = new Triangle(FindMiddlePoint(x.Top, x.Right), FindMiddlePoint(x.Top, x.Left), FindMiddlePoint(x.Left, x.Right));
            return result;
        }

        public class Triangle
        {
            public Point Top, Left, Right;
            public Point[] array = new Point[3];

            public Triangle(Point x, Point y, Point z)
            {
                Top = array[0] = x;
                Left = array[1] = y;
                Right = array[2] = z;
            }
        }

        #endregion

        #region Events

        private void pbCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            
            g.FillPolygon(bBlack, ListOfSetsOfTriangles.First().First().array, FillMode.Winding);

            foreach (var set in ListOfSetsOfTriangles)
            {
                foreach (var triangle in set)
                {
                    g.FillPolygon(bWhite, GetSubsetCordsOfTriangle(triangle).array, FillMode.Winding);                    
                }
            }

            pbCanvas.Invalidate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int dummy;
            if (int.TryParse(textBox1.Text, out dummy))
            {
                NumberOfIterations = Convert.ToInt32(textBox1.Text);
                ResetListOfSetsOfTriangles();
                CreateAllSetsOfTriangles();
            }
        }

        private void CreateAllSetsOfTriangles()
        {
            while (NumberOfIterations > 1)
            {
                ListOfSetsOfTriangles.Add(CreateNextSetOfTriangles(ListOfSetsOfTriangles.Last()));
                NumberOfIterations--;
            }
        }

        #endregion
    }
}
