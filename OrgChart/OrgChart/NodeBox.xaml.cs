using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace OrgChart
{
    public partial class NodeBox : UserControl
    {
        private double _fontSize = 10;
        private double clipHeight = 200;
        public NodeBox(double scale)
        {
            _Scale = scale;
            InitializeComponent();

            tbEmployeeName.FontSize = _fontSize * Scale;
            tbEmployeeName.SetValue(Canvas.TopProperty, 5 * scale);
            tbEmployeeName.SetValue(Canvas.LeftProperty, 5 * scale);
            tbEmployeeName.Height = 20 * scale;
            tbEmployeeName.Width = clipHeight * scale;
            tbEmployeeName.TextWrapping = TextWrapping.NoWrap;
            tbEmployeeName.Clip = new RectangleGeometry() { Rect = new Rect(0, 0, clipHeight * scale, 20 * scale) };
            tbEmployeeName.TextAlignment = TextAlignment.Center;

            tbField1.FontSize = _fontSize * Scale;
            tbField1.SetValue(Canvas.TopProperty, 20 * scale);
            tbField1.SetValue(Canvas.LeftProperty, 5 * scale);
            tbField1.Height = 20 * scale;
            tbField1.Width = clipHeight * scale;
            tbField1.TextWrapping = TextWrapping.NoWrap;
            tbField1.Clip = new RectangleGeometry() { Rect = new Rect(0, 0, clipHeight * scale, 20 * scale) };
            tbField1.TextAlignment = TextAlignment.Center;

            tbField2.FontSize = _fontSize * Scale;
            tbField2.SetValue(Canvas.TopProperty, 35 * scale);
            tbField2.SetValue(Canvas.LeftProperty, 5 * scale);
            tbField2.Height = 20 * scale;
            tbField2.Width = clipHeight * scale;
            tbField2.TextWrapping = TextWrapping.NoWrap;
            tbField2.Clip = new RectangleGeometry() { Rect = new Rect(0, 0, clipHeight * scale, 20 * scale) };
            tbField2.TextAlignment = TextAlignment.Center;

            tbField3.FontSize = _fontSize * Scale;
            tbField3.SetValue(Canvas.TopProperty, 50 * scale);
            tbField3.SetValue(Canvas.LeftProperty, 5 * scale);
            tbField3.Height = 20 * scale;
            tbField3.Width = clipHeight * scale;
            tbField3.TextWrapping = TextWrapping.NoWrap;
            tbField3.Clip = new RectangleGeometry() { Rect = new Rect(0, 0, clipHeight * scale, 20 * scale) };
            tbField3.TextAlignment = TextAlignment.Center;

            tbField4.FontSize = _fontSize * Scale;
            tbField4.SetValue(Canvas.TopProperty, 65 * scale);
            tbField4.SetValue(Canvas.LeftProperty, 5 * scale);
            tbField4.Height = 20 * scale;
            tbField4.Width = clipHeight * scale;
            tbField4.TextWrapping = TextWrapping.NoWrap;
            tbField4.Clip = new RectangleGeometry() { Rect = new Rect(0, 0, clipHeight * scale, 20 * scale) };
            tbField4.TextAlignment = TextAlignment.Center;

            tbField5.FontSize = _fontSize * Scale;
            tbField5.SetValue(Canvas.TopProperty, 80 * scale);
            tbField5.SetValue(Canvas.LeftProperty, 5 * scale);
            tbField5.Height = 20 * scale;
            tbField5.Width = clipHeight * scale;
            tbField5.TextWrapping = TextWrapping.NoWrap;
            tbField5.Clip = new RectangleGeometry() { Rect = new Rect(0, 0, clipHeight * scale, 20 * scale) };
            tbField5.TextAlignment = TextAlignment.Center;


            //tbData.FontSize = _fontSize * Scale;
            //tbData.SetValue(Canvas.TopProperty, 20 * scale);
            //tbData.SetValue(Canvas.LeftProperty, 5 * scale);
            //tbData.Height = 20 * scale;
            //tbData.Width = 130 * scale;
            //tbData.TextWrapping = TextWrapping.NoWrap;
            //tbData.Clip = new RectangleGeometry() { Rect = new Rect(0, 0, 130 * scale, 20 * scale) };
            //tbData.TextAlignment = TextAlignment.Center;

            //tbTitle.FontSize = _fontSize * Scale;
            //tbTitle.SetValue(Canvas.TopProperty, 20 * scale);
            //tbTitle.SetValue(Canvas.LeftProperty, 5 * scale);
            //tbTitle.Height = 20 * scale;
            //tbTitle.Width = 130 * scale;
            //tbTitle.TextWrapping = TextWrapping.NoWrap;
            //tbTitle.Clip = new RectangleGeometry() { Rect = new Rect(0, 0, 130 * scale, 20 * scale) };

            //tbDepartment.FontSize = _fontSize * Scale;
            //tbDepartment.SetValue(Canvas.TopProperty, 35 * scale);
            //tbDepartment.SetValue(Canvas.LeftProperty, 5 * scale);
            //tbDepartment.Height = 20 * scale;
            //tbDepartment.TextWrapping = TextWrapping.NoWrap;
            //tbDepartment.Clip = new RectangleGeometry() { Rect = new Rect(0, 0, 130 * scale, 20 * scale) };

            //tbExtension.FontSize = _fontSize * Scale;
            //tbExtension.SetValue(Canvas.TopProperty, 60 * scale);
            //tbExtension.SetValue(Canvas.LeftProperty, 5 * scale);
            //tbExtension.Height = 20 * scale;
            //tbExtension.TextWrapping = TextWrapping.NoWrap;
            //tbExtension.Clip = new RectangleGeometry() { Rect = new Rect(0, 0, 130 * scale, 20 * scale) };

            recBorder.StrokeThickness = 2 * scale;
            recBorder.RadiusX = 5 * scale;
            recBorder.RadiusY = 5 * scale;
            recBorder.Width = this.Width * scale;
            recBorder.Height = this.Height * scale;
        }

        private double _Scale = 1;
        public double Scale
        {
            get
            {
                return _Scale;
            }
            set
            {
                _Scale = value;
            }
        }

        private string _ID;
        public string ID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;
            }
        }

        private string _EmployeeName;
        public string EmployeeName
        {
            get
            {
                return _EmployeeName;
            }
            set
            {
                _EmployeeName = value;
                tbEmployeeName.Text = _EmployeeName;
            }
        }

        private string _Field1;
        public string Field1
        {
            get
            {
                return _Field1;
            }
            set
            {
                _Field1 = value;
                tbField1.Text = _Field1;
            }
        }

        private string _Field2;
        public string Field2
        {
            get
            {
                return _Field2;
            }
            set
            {
                _Field2 = value;
                tbField2.Text = _Field2;
            }
        }

        private string _Field3;
        public string Field3
        {
            get
            {
                return _Field3;
            }
            set
            {
                _Field3 = value;
                tbField3.Text = _Field3;
            }
        }

        private string _Field4;
        public string Field4
        {
            get
            {
                return _Field4;
            }
            set
            {
                _Field4 = value;
                tbField4.Text = _Field4;
            }
        }

        private string _Field5;
        public string Field5
        {
            get
            {
                return _Field5;
            }
            set
            {
                _Field5 = value;
                tbField5.Text = _Field5;
            }
        }

        //private string _Title;
        //public string Title
        //{
        //    get
        //    {
        //        return _Title;
        //    }
        //    set
        //    {
        //        _Title = value;
        //        tbTitle.Text = _Title;
        //    }
        //}

        //private string _Department;
        //public string Department
        //{
        //    get
        //    {
        //        return _Department;
        //    }
        //    set
        //    {
        //        _Department = value;
        //        tbDepartment.Text = _Department;
        //    }
        //}

        private string _Extension;
        public string Extension
        {
            get
            {
                return _Extension;
            }
            set
            {
                _Extension = value;

               

            }
        }

        private void canvMain_MouseEnter(object sender, MouseEventArgs e)
        {
            this.mouseEnter.Begin();
        }

        private void canvMain_MouseLeave(object sender, MouseEventArgs e)
        {
            this.mouseLeave.Begin();
        }
    }
}
