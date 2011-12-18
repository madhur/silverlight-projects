﻿using System;
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
using System.Windows.Threading;
using System.Xml.Linq;
using System.Windows.Browser;
using System.Diagnostics;

namespace OrgChart
{
    public partial class MainPage : UserControl
    {
        public List<Person> persons = new List<Person>();
        public double totalWidth;
        public double totalHight;
        public double buttonWidth;
        public double buttonHeight;
        public double plusButtonWidth;
        public double plusButtonHeight;
        public double levelHight;
        public double minHorizontalSpace;
        public double minVerticalSpace;
        public double fontSize;
        public SolidColorBrush LinesStroke;
        public double LinesStrokeThickness;
        public DispatcherTimer _doubleClickTimer;
        public double drawingScale = 1;
        private string m_dataUrl;
        private XDocument m_xdoc;

        public MainPage(string listGuid)
        {
            InitializeComponent();
            MyScoller.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
            
            if (string.IsNullOrEmpty(listGuid))
               throw new ArgumentNullException("listGuid");

          string baseUrl = string.Concat(HtmlPage.Document.DocumentUri.Scheme, "://", HtmlPage.Document.DocumentUri.GetComponents(UriComponents.HostAndPort, UriFormat.UriEscaped));  
          Guid lg = new Guid(listGuid);

          if (App.Current.Host.InitParams.Count>1)
          {
              baseUrl = HttpUtility.UrlDecode(App.Current.Host.InitParams["MS.SP.url"]);
          }

         
          // e.g. 
          m_dataUrl = string.Concat(baseUrl, "/_vti_bin/owssvr.dll?Cmd=Display&List=", HttpUtility.UrlEncode(lg.ToString()), "&XMLDATA=TRUE");
          Debug.WriteLine(m_dataUrl);
           InitializeComponent();

          // LoadXMLFile();
           Loaded += new RoutedEventHandler(Page_Loaded);
        }


        /// <summary>
       /// Page loaded event
       /// </summary>
       /// <param name="sender"></param>
      /// <param name="e"></param>
      private void Page_Loaded(object sender, RoutedEventArgs e)
       {
          // Credentials will be taken from the current IE user
          WebClient svc = new WebClient();
          svc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(XmlDownloadStringCompleted);
          svc.DownloadStringAsync(new Uri(m_dataUrl, UriKind.Absolute));
          Debug.WriteLine(m_dataUrl);
      }

        /// <summary>
       /// XML list data download complete
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
      private void XmlDownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
       {
           // e.result contains Xml string
           m_xdoc = XDocument.Load("madhur.xml");
          // m_xdoc = XDocument.Load("OrgChart_Data.xml");
        //  m_xdoc = XDocument.Parse(e.Result);
          Debug.WriteLine(m_xdoc);
          InitializeGrid();
         //  XMLFileLoaded(m_xdoc);
      }


         /// <summary>
       /// Set up the grid 
        /// </summary>
       private void InitializeGrid()
       {
         //  Person firstNode = Person.GetPerson("OrgChartRootNode", "My Organization", "", "", "", "", "");
        //   firstNode.MinChildWidth = totalWidth;
        //   persons.Add(firstNode);

         

            // The Task object is a simple data class to roll yourself with the properties you need
            XNamespace z = "#RowsetSchema";
            var Result = from row in m_xdoc.Descendants(z + "row")
               select new Person
               {
                   ID = row.Attribute("ows_PersonID") != null ? row.Attribute("ows_PersonID").Value : null,
                   Name = row.Attribute("ows_LinkTitle") != null ? row.Attribute("ows_LinkTitle").Value : null,
                   ManagerID = row.Attribute("ows_ManagerID") != null ? row.Attribute("ows_ManagerID").Value : null,
                   //Department = row.Attribute("ows_Department") != null ? row.Attribute("ows_Department").Value : null,
                   //Extension = row.Attribute("ows_Extension") != null ? row.Attribute("ows_Extension").Value : null,
                   //Email = row.Attribute("ows_Email") != null ? row.Attribute("ows_Email").Value : null,
                   Field1 = row.Attribute("ows_Field1") != null ? row.Attribute("ows_Field1").Value : null,
                   Field2 = row.Attribute("ows_Field2") != null ? row.Attribute("ows_Field2").Value : null,
                   Field3 = row.Attribute("ows_Field3") != null ? row.Attribute("ows_Field3").Value : null,
                   Field4 = row.Attribute("ows_Field4") != null ? row.Attribute("ows_Field4").Value : null,
                   Field5 = row.Attribute("ows_Field5") != null ? row.Attribute("ows_Field5").Value : null,
                   dotted= row.Attribute("ows_Dotted") != null ? row.Attribute("ows_Dotted").Value : null
               };


           
            Person[] nodes = Result.ToArray();

            for (int i = 0; i < nodes.Length; i++)
            {
               // Debug.WriteLine(nodes[i].Data);
                if (string.IsNullOrEmpty(nodes[i].ManagerID))
                {
                    nodes[i].ManagerID = "1";
                }
                if (nodes[i].ManagerID == "0")
                {
                    nodes[i].ManagerID = "";
                }
               
            }
            persons.AddRange(nodes);
            Rescale();
          // myDataGrid.ItemsSource = myData;
        }

        private void Rescale()
        {
            MyCanvas.Children.Clear();
            buttonWidth = 200 * drawingScale;
            buttonHeight = 80 * drawingScale;
            minHorizontalSpace = 50 * drawingScale;
            minVerticalSpace = 10 * drawingScale;
            plusButtonWidth = 12 * drawingScale;
            plusButtonHeight = 12 * drawingScale;
            fontSize = 10 * drawingScale;
            LinesStrokeThickness = 1 * drawingScale;
            levelHight = buttonHeight + minVerticalSpace * 2;
            Refresh();
        }

        private void Refresh()
        {
            MyCanvas.Width = buttonWidth;
            MyCanvas.Height = buttonHeight;
            totalWidth = MyCanvas.Width;
            totalHight = MyCanvas.Height;
            persons[0].MinChildWidth = buttonWidth + minHorizontalSpace;
            persons[0].StartX = 0;
            persons[0].X = persons[0].MinChildWidth / 2;
            SetLevel(persons[0], 1);
            CalculateWidth(persons[0]);
            CalculateX(persons[0]);
            DrawNode(persons[0]);
        }

        #region Load XML
        //private void LoadXMLFile()
        //{
        //    WebClient xmlClient = new WebClient();
        //    xmlClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(XMLFileLoaded);
        //    xmlClient.DownloadStringAsync(new Uri("OrgChart_Data.xml", UriKind.RelativeOrAbsolute));
        //}

        void XMLFileLoaded(XDocument lobjDocument)
        {
           
                Person firstNode = Person.GetPerson("OrgChartRootNode", "My Organization", "", "", "", "", "");
                firstNode.MinChildWidth = totalWidth;
                persons.Add(firstNode);

             //   XElement lobjDocument = XElement.Parse(e.Result.ToString());

                var Result = from view1 in lobjDocument.Descendants("Person")
                             select new Person
                             {
                                 Name = (string)view1.Element("Name"),
                                 ID = (string)view1.Element("ID"),
                                 ManagerID = (string)view1.Element("ManagerID"),
                                 Title = (string)view1.Element("Title"),
                                 Department = (string)view1.Element("Department"),
                                 Extension = (string)view1.Element("Extension"),
                                 Email = (string)view1.Element("Email")
                             };

                Person[] nodes = Result.ToArray();

                for (int i = 0; i < nodes.Length; i++)
                {
                   // Debug.WriteLine(nodes[i].Data);
                    if (string.IsNullOrEmpty(nodes[i].ManagerID))
                    {
                        nodes[i].ManagerID = "OrgChartRootNode";
                    }
                }
                persons.AddRange(nodes);
                Rescale();
            
        }
        #endregion

        #region Calculate Values
        private void SetLevel(Person parent, int level)
        {
            // Set the node level
            parent.Level = level;

            // Calculate the total height based on the number of levels
            if (totalHight < levelHight * (level + 2))
            {
                totalHight = levelHight * (level + 2);
                MyCanvas.Height = totalHight;
            }

            // Select the closed items under this parent
            var resultN = from n in persons
                          where n.ManagerID == parent.ID && n.Opened == false
                          select n;

            // Get the closed nodes number
            parent.HiddenSubNodes = resultN.Count();

            // Select the opend nodes under this parent
            var result = from n in persons
                         where n.ManagerID == parent.ID && n.Opened == true
                         select n;

            Person[] nodes = result.ToArray();

            // Get the Opend nodes number
            parent.SubNodes = nodes.Length;

            // Call the child nodes
            for (int i = 0; i < nodes.Length; i++)
            {
                nodes[i].NodeOrder = i + 1;
                nodes[i].MinChildWidth = buttonWidth + minHorizontalSpace;
                SetLevel(nodes[i], parent.Level + 1);
            }
        }

        private void CalculateWidth(Person parent)
        {
            if (parent.SubNodes > 0)
            {
                var result = from n in persons
                             where n.ManagerID == parent.ID && n.Opened == true
                             orderby n.NodeOrder
                             select n;

                Person[] nodes = result.ToArray();
                double width = 0;               
                
                for (int i = 0; i < nodes.Length; i++)
                {
                    // calculate the width of the child before adding it to the parent
                    CalculateWidth(nodes[i]);

                    // calculate the new width
                    width = width + nodes[i].MinChildWidth;
                }

                if (width > parent.MinChildWidth)
                {
                    parent.MinChildWidth = width;
                    if (MyCanvas.Width < width)
                    {
                        MyCanvas.Width = width;
                        //totalWidth = width;
                    }
                }                
            }
        }

        private void CalculateX(Person parent)
        {
            if (parent.SubNodes > 0)
            {
                var result = from n in persons
                             where n.ManagerID == parent.ID && n.Opened == true
                             orderby n.NodeOrder
                             select n;

                Person[] nodes = result.ToArray();
                
                // Calculate the startX for each node
                double start = parent.StartX;
                for (int i = 0; i < nodes.Length; i++)
                {
                    nodes[i].StartX = start;
                    nodes[i].X = nodes[i].StartX + nodes[i].MinChildWidth / 2;
                    CalculateX(nodes[i]);
                    start = start + nodes[i].MinChildWidth;
                }

                // realign the parent node to the middle of the child nodes
                if (nodes.Length > 1)
                {
                    parent.X = (nodes[0].X + nodes[nodes.Length - 1].X) / 2;
                }
                else // root element
                {
                    parent.X = nodes[0].X;
                }
            }
        }
        #endregion

        #region Draw
        private void DrawNode(Person parent)
        {
            // Check if the current node is the parent node or not
            if (parent.ManagerID == "")
            {
                AddBox(MyCanvas, parent.X, parent.Level * levelHight, null, parent.ID, parent.Name, parent.Title, parent.Department, parent.Extension, false, true, parent.HiddenSubNodes > 0, parent.Field1, parent.Field2, parent.Field3, parent.Field4, parent.Field5, parent.dotted);
            }

            // Get the child nodes
            var results = from n in persons
                          where n.ManagerID == parent.ID && n.Opened == true
                          select n;

            foreach (Person p in results)
            {
                AddBox(MyCanvas, p.X, p.Level * levelHight, parent.X, p.ID, p.Name, p.Title, p.Department, p.Extension, true, p.SubNodes > 0, p.HiddenSubNodes > 0, p.Field1, p.Field2, p.Field3, p.Field4, p.Field5, p.dotted);
                DrawNode(p);
            }
        }
        
        public void DrawLine(Canvas canvas, double x1, double y1, double x2, double y2)
        {
            Line line = new Line();
            line.X1 = x1;
            line.Y1 = y1;
            line.X2 = x2;
            line.Y2 = y2;
            line.Stroke = Util.GetColorFromHex("#FF6495ed");
            line.StrokeThickness = LinesStrokeThickness;
            canvas.Children.Add(line);
        }

        public void AddBox(Canvas canvas, double x, double y, double? parentX, string ID, string name, string title, string department, string extension, bool root, bool subNodes, bool hiddenSubNodes, string field1, string field2, string field3, string field4, string field5, string dotted)
        {
            NodeBox nb = new NodeBox(drawingScale);
            nb.Name = ID;
            nb.EmployeeName = name;
            //b.Data = Data;
            nb.Field1 = field1;
            nb.Field2 = field2;
            nb.Field3 = field3;
            nb.Field4 = field4;
            nb.Field5 = field5;

            if (!string.IsNullOrEmpty(dotted))
            {
                DoubleCollection db = new DoubleCollection();
                db.Add(10);
                db.Add(2);
                nb.recBorder.StrokeDashArray = db;
            }

          //  nb.Title = title;
          //  nb.Department = department;
            nb.Extension = extension;
            nb.Width = buttonWidth;
            nb.Height = buttonHeight;
            nb.SetValue(Canvas.LeftProperty, x - nb.Width / 2);
            nb.SetValue(Canvas.TopProperty, y);

            // handle the double click actions
            _doubleClickTimer = new DispatcherTimer();
            _doubleClickTimer.Interval = new TimeSpan(0, 0, 0, 0, 200);
            _doubleClickTimer.Tick += new EventHandler(delegate { _doubleClickTimer.Stop(); });
            nb.MouseLeftButtonDown += new MouseButtonEventHandler(delegate
            {
                if (_doubleClickTimer.IsEnabled)
                {
                    _doubleClickTimer.Stop();
                    btnClicked(ID);
                }
                else
                {
                    _doubleClickTimer.Start();
                }
            });

            canvas.Children.Add(nb);

            // The line above the box
            if (root)
                DrawLine(canvas, x, y - minVerticalSpace, x, y);


            // Draw the horizontal line to the parent
            if (parentX != null)
                DrawLine(canvas, x, y - minVerticalSpace, Convert.ToDouble(parentX), y - minVerticalSpace);


            // Draw the line under the box
            if (subNodes || hiddenSubNodes)
                DrawLine(canvas, x, y + buttonHeight, x, y + buttonHeight + minVerticalSpace);

            // display the small plus
            if (hiddenSubNodes)
            {
                Button btn = new Button();
                btn.Name = "plus" + ID;
                btn.FontSize = fontSize / 2;
                btn.Click += new RoutedEventHandler(btn_Click);
                btn.Height = plusButtonHeight;
                btn.Width = plusButtonWidth;
                btn.Content = "+";
                btn.SetValue(Canvas.LeftProperty, x - btn.Width / 2);
                btn.SetValue(Canvas.TopProperty, y + buttonHeight + minVerticalSpace - minVerticalSpace / 2);
                canvas.Children.Add(btn);
            }
        }
        #endregion

        #region Events
        void btn_Click(object sender, RoutedEventArgs e)
        {
            Button clicked = (Button)sender;
            btnClicked(clicked.Name.Replace("plus", ""));
        }        

        public void btnClicked(string clickedButtonName)
        {
            for (int i = 0; i < persons.Count; i++)
            {
                if (persons[i].ID == clickedButtonName)
                {
                    MyCanvas.Children.Clear();
                    var results = from n in persons
                                  where n.ManagerID == persons[i].ID
                                  select n;

                    persons[i].Collapsed = !persons[i].Collapsed;
                    foreach (Person p in results)
                    {
                        p.Opened = !persons[i].Collapsed;
                    }
                    
                    Refresh();
                    return;
                }
            }
        }
        #endregion
               
    }
}
