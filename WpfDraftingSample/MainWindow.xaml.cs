using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
//eyeshot imports
using devDept.Eyeshot;
using devDept.Eyeshot.Control;

using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using static WpfDraftingSample.MyDesign;
using System.Windows.Controls.Primitives;
using devDept.Eyeshot.Translators;
using Microsoft.Win32;

namespace WpfDraftingSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {
        public MainWindow()
        {
            InitializeComponent();

            // Add any initialization after the InitializeComponent() call.

            design1.MaxHatchPatternLines = 1000;


            startRadioButton.Checked += radioButtons_CheckedChanged;
            midRadioButton.Checked += radioButtons_CheckedChanged;
            endRadioButton.Checked += radioButtons_CheckedChanged;
            centerRadioButton.Checked += radioButtons_CheckedChanged;
            pointRadioButton.Checked += radioButtons_CheckedChanged;
            selectByBox.Checked += radioButtons_CheckedChanged;
            selectByPick.Checked += radioButtons_CheckedChanged;

            design1.WorkCompleted += Design1_WorkCompleted;




        }

            private void Design1_WorkCompleted(object sender, devDept.WorkCompletedEventArgs e)
            {
                if (e.WorkUnit is ReadFile rf)
                {
                    rf.AddTo(design1);
                    design1.Invalidate();
                    MessageBox.Show("File successfully open...");
                }
                else
                {
                    MessageBox.Show("File Saved successfully...");
                }
            
            }

        public void showGridButton_OnClick(object sender, RoutedEventArgs e)
        {
            ClearPreviousSelection();
            design1.ActiveViewport.Grid.Visible = !design1.ActiveViewport.Grid.Visible;


            if (design1.ActiveViewport.Grid.Visible)
            {
                showGridButton.Background = new SolidColorBrush(Colors.LightGreen); // Highlighted color
            }
            else
            {
                showGridButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4681f4")); // Original color
            }

        }

        private void Ribbon_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void ClearPreviousSelection()
        {
            design1.SetView(viewType.Top, false, true);
           
            design1.ClearAllPrevious();
        }

        private void AddPointButton_Click(object sender, RoutedEventArgs e)
        {
            ClearPreviousSelection();
            design1.drawingPoints = true;
        }

        private void AddLineButton_Click(object sender, RoutedEventArgs e)
        {
            ClearPreviousSelection();
            design1.drawingLine = true;
        }

        private void AddCircleButton_Click(object sender, RoutedEventArgs e)
        {
            ClearPreviousSelection();
            design1.drawingCircle = true;
        }

        private void AddArcButton_Click(object sender, RoutedEventArgs e)
        {
            ClearPreviousSelection();
            design1.drawingArc = true;

          
        }

        private void SnapGridButton_Click(object sender, RoutedEventArgs e)
        {
            ClearPreviousSelection();

            design1.isSnapGridEnabled = !design1.isSnapGridEnabled;

            if (design1.isSnapGridEnabled)
            {
                SnapGridButton.Background = new SolidColorBrush(Colors.LightGreen); // Highlighted color
            }
            else
            {
                SnapGridButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4681f4")); // Original color
            }

        }

        private void radioButtons_CheckedChanged(object sender, EventArgs e)
        {
            if (endRadioButton.IsChecked != null && endRadioButton.IsChecked.Value)
            {
                design1.ActiveObjectSnap = objectSnapType.End;

            }
            else if (startRadioButton.IsChecked != null && startRadioButton.IsChecked.Value)
            {
                design1.ActiveObjectSnap = objectSnapType.Start;

            }
            else if (midRadioButton.IsChecked != null && midRadioButton.IsChecked.Value)
            {
                design1.ActiveObjectSnap = objectSnapType.Mid;

            }
            else if (centerRadioButton.IsChecked != null && centerRadioButton.IsChecked.Value)
            {
                design1.ActiveObjectSnap = objectSnapType.Center;

            }
            else if (pointRadioButton.IsChecked != null && pointRadioButton.IsChecked.Value)
            {
                design1.ActiveObjectSnap = objectSnapType.Point;

            }
            else if (selectByPick.IsChecked != null && selectByPick.IsChecked.Value)
            {
                design1.ActionMode = actionType.SelectByPick;

            }
            else if (selectByBox.IsChecked != null && selectByBox.IsChecked.Value)
            {
                design1.ActionMode = actionType.SelectByBox;

            }


        }

        private void SnapObjectButton_Click(object sender, RoutedEventArgs e)
        {
            ClearPreviousSelection();
            design1.ObjectSnapEnabled = !design1.ObjectSnapEnabled;

            if (design1.ObjectSnapEnabled==true)
            {
                snapPanel.IsEnabled = true;
            }
            else
            {
                snapPanel.IsEnabled = false;
            }

            if (design1.ObjectSnapEnabled)
            {
                SnapObjectButton.Background = new SolidColorBrush(Colors.LightGreen); // Highlighted color
            }
            else
            {
                SnapObjectButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4681f4")); // Original color
            }


        }

        //private void Design1_SelectionChanged(object sender, EventArgs e)
        //{
        //    var selectedEntities = design1.Entities.Where(ent => ent.Selected).ToList();
        //    foreach (var entity in selectedEntities)
        //    {
        //        // Perform actions on selected entities
        //        // For example, change the color of the selected entity
        //        entity.ColorMethod = colorMethodType.byEntity;
        //        entity.Color = System.Drawing.Color.Red;
        //    }

        //    // Redraw the viewport to reflect the selection change
        //    design1.Invalidate();
        //}

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            ClearPreviousSelection();
            design1.SelectionEnabled = !design1.SelectionEnabled;

            if (design1.SelectionEnabled == true)
            {
                selectPanel.IsEnabled = true;
  
            }
            else
            {
              
                selectPanel.IsEnabled = false;
                selectByPick.IsChecked = false;
                selectByBox.IsChecked = false;


            }


            if (design1.SelectionEnabled)
            {
                SelectButton.Background = new SolidColorBrush(Colors.LightGreen); // Highlighted color
            }
            else
            {
                SelectButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4681f4")); // Original color
            }

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            design1.Entities.DeleteSelected();
            design1.Invalidate();   
        }

        private void MoveButton_Click(object sender, RoutedEventArgs e)
        {
           

            if (design1.selEntities != null)
            {
                design1.selEntities.Clear();
            }

            for (int i = design1.Entities.Count - 1; i > -1; i--)
            {
                Entity ent = design1.Entities[i];
                if (ent.Selected && (ent is ICurve || ent is BlockReference || ent is Text || ent is Leader))
                {
                    design1.selEntities.Add(ent);
                }
            }

            if (design1.selEntities.Count == 0)
                return;

            ClearPreviousSelection();
          
            design1.doingMove = true;
            foreach (Entity curve in design1.selEntities)
                curve.Selected = true;
        }

     
        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            MyPopup.IsOpen = false;
           
        }

        private void CheckParallel_Click(object sender, RoutedEventArgs e)
        {

            if (design1.selEntities != null)
            {
                design1.selEntities.Clear();
            }

            for (int i = design1.Entities.Count - 1; i > -1; i--)
            {
                Entity ent = design1.Entities[i];
                if (ent.Selected && (ent is ICurve || ent is BlockReference || ent is Text || ent is Leader))
                {
                    design1.selEntities.Add(ent);
                }
            }

            design1.CheckLinesParallelOrPerpendicular();
            PopupTextBlock.Text = design1.CheckLines;
            MyPopup.IsOpen = true;


        }

        private void RotateButton_Click(object sender, RoutedEventArgs e)
        {
            if (design1.selEntities != null)
            {
                design1.selEntities.Clear();
            }

            for (int i = design1.Entities.Count - 1; i > -1; i--)
            {
                Entity ent = design1.Entities[i];
                if (ent.Selected && (ent is ICurve || ent is BlockReference || ent is Text || ent is Leader))
                {
                    design1.selEntities.Add(ent);
                }
            }

            if (design1.selEntities.Count == 0)
                return;

            ClearPreviousSelection();

            design1.doingRotate = true;
            foreach (Entity curve in design1.selEntities)
                curve.Selected = true;
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            //ReadFile rf = new ReadFile(@"C:\Users\PTS-PC-63_USR02\Desktop\sample.eye");
            //design1.StartWork(rf);
            ////rf.DoWork();
            ////rf.AddTo(design1.Document);
            ////design1.Invalidate();
            ////design1.CreateControl();
            ///
            // Create an OpenFileDialog to allow the user to select a file to open
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Eyeshot files (*.eye)|*.eye|All files (*.*)|*.*"; 

            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                string filePath = openFileDialog.FileName;
                ReadFile rf = new ReadFile(filePath);
                design1.StartWork(rf);
            }
        }

        private void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            //devDept.Eyeshot.Translators.WriteAutodeskParams wap = new WriteAutodeskParams(design1.Document);
            //WriteAutodesk wa = new WriteAutodesk(design1.Document, @"C:\Users\PTS-PC-63_USR02\Desktop\sap1234.dwg");
            //wa.DoWork();
            //design1.CreateControl();
            //WriteFileParams wfp = new WriteFileParams(design1.Document);
            //WriteFile wf = new WriteFile(wfp, @"C:\Users\PTS-PC-63_USR02\Desktop\sample.eye");

            //design1.StartWork(wf);
            //design1.CreateControl();

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Eyeshot files (*.eye)|*.eye|All files (*.*)|*.*";
            saveFileDialog.DefaultExt = ".eye"; // Default file extension

            bool? result = saveFileDialog.ShowDialog();

            if (result == true)
            {
                string filePath = saveFileDialog.FileName;

                WriteFileParams wfp = new WriteFileParams(design1.Document);
                WriteFile wf = new WriteFile(wfp, filePath);

                design1.StartWork(wf);
                design1.CreateControl();
            }
        }

        private void AddRectangleButton_Click(object sender, RoutedEventArgs e)
        {
            ClearPreviousSelection();
            design1.drawingRectangle = true;
        }

        private void PerpendcularSnapButton_Click(object sender, RoutedEventArgs e)
        {
            ClearPreviousSelection();
            design1.perpendicularSnap = true;
        }

        private void ExtrudeButton_Click(object sender, RoutedEventArgs e)
        {
            design1.isExtrude=true;
            
        }
    }


    
}