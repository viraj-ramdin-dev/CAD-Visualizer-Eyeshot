using devDept.Eyeshot;
using devDept.Eyeshot.Control;
using devDept.Eyeshot.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//eyeshot imports
using devDept.Eyeshot;
using devDept.Eyeshot.Control;

using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;
using devDept.Graphics;
using System.Drawing;
using devDept.Eyeshot.Control.MultiTouch.Interop;
using System.Xml;

using Point = devDept.Eyeshot.Entities.Point;
using static WpfDraftingSample.MyDesign;
using static devDept.LicenseManager;
using static devDept.Eyeshot.Entities.Solid;
using System.Diagnostics.Eventing.Reader;
using System.Windows.Controls;
using System.Net.NetworkInformation;


namespace WpfDraftingSample
{
    public class MyDesign : Design
    {
        internal System.Drawing.Point mouseLocation;
        // Always draw on XY plane, view is alwyas topview
        internal Plane planeXY = Plane.XY;
        // Current selection/position
        internal Point3D current;

        internal Plane DrawingPlane;


        string activeOperationLabel ="";

        //flags

        public bool drawingPoints;

        public bool drawingLine;

        public bool drawingCircle;

        public bool drawingArc;
        public bool drawingRectangle;

        public bool perpendicularSnap;
        public bool isExtrude;



        public bool isSnapGridEnabled;



        internal bool cursorOutside;

        // list to store points
        internal List<Point3D> points = new List<Point3D>();

        // circle
        internal Plane drawingPlane;

        // Current arc radius
        internal double radius;

        internal double arcSpanAngle;

        internal double startAngle;

        internal double endAngle;

        // for cursor
        //public Drawing Drawing;


        // object sanpping line
        public bool ObjectSnapEnabled { get; set; }

        public bool SelectionEnabled { get; set; }


        public enum objectSnapType
        {
            None,
            Point,
            End,
            Start,
            Mid,
            Center,
            Quad
        }

        public objectSnapType ActiveObjectSnap = objectSnapType.End;

        internal Point3D snapPoint = null;

        bool currentlySnapping = false;
        internal SnapPoint[] snapPoints;
        internal SnapPoint myCustomSnapPoint;

        //selection 

        public bool SelctionEnabled { get; set; }
        public bool doingMove;
        public bool doingRotate;


        public List<Entity> selEntities = new List<Entity>();


        // for rectnagle last point
        Point3D rectangleLastPoint;




        Block wheelBlock = new Block("wheelBlock");


        protected void createMyRectangleBlock()
        {

            string blockName = "MyBlock";


            if (!Blocks.Contains(blockName))
            {

                // Wheels - Circles
                wheelBlock.Entities.Add(new Circle(new Point3D(-6, -2, 0), 2)); // Left wheel
                wheelBlock.Entities.Add(new Circle(new Point3D(6, -2, 0), 2));  // Right wheel
                Blocks.Add(wheelBlock);


            }


            if (!Blocks.Contains(blockName))
            {
               
                Block block = new Block(blockName);

                block.Entities.Add(new LinearPath(new Point3D[]
                {
            new Point3D(-10, 0, 0), // Bottom-left
            new Point3D(10, 0, 0),  // Bottom-right
            new Point3D(10, 5, 0),  // Top-right
            new Point3D(-10, 5, 0), // Top-left
            new Point3D(-10, 0, 0)  // Close the rectangle
                }));

                // Car Roof - Another smaller rectangle
                block.Entities.Add(new LinearPath(new Point3D[]
                {
            new Point3D(-6, 5, 0), // Bottom-left
            new Point3D(6, 5, 0),  // Bottom-right
            new Point3D(4, 8, 0),  // Top-right
            new Point3D(-4, 8, 0), // Top-left
            new Point3D(-6, 5, 0)  // Close the rectangle
                }));

                block.Entities.Add(new BlockReference(0, 0, 0, "wheelBlock", 1, 1, 1, 0));
                    
                Blocks.Add(block);

            }

            BlockReference blockRef = new BlockReference(current.X, current.Y, 0, blockName, 1, 1, 1, 0);
            Entities.Add(blockRef);
        }




        //check paralell lines
        public string CheckLines;
        public void CheckLinesParallelOrPerpendicular()
        {
            // Check if there are exactly two entities and both are Line entities
            if (selEntities.Count == 2 && selEntities[0] is Line && selEntities[1] is Line)
            {
                // Cast the selected entities to Line
                Line line1 = selEntities[0] as Line;
                Line line2 = selEntities[1] as Line;

                // Normalize the direction vectors
                Vector3D dirVector1=line1.Direction;
                Vector3D dirVector2=line2.Direction;

                dirVector1.Normalize();
                dirVector2.Normalize();
                bool areParallel = Vector3D.AreParallel(dirVector1, dirVector2);
                bool arePerpendicular= Vector3D.AreOrthogonal(dirVector1, dirVector2);

                if (areParallel)
                {
                    CheckLines="The two lines are parallel.";
                }
                else if (arePerpendicular)
                {
                    CheckLines = "The two lines are perpendicular.";
                }
                else
                {
                    CheckLines="The two lines are neither parallel nor perpendicular."; 
                }
            }
            else
            {
                CheckLines="Please select exactly two line entities.";
            }
        }
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {

            if (e.RightButton == MouseButtonState.Pressed)
            {
                ClearAllPrevious();
                base.OnMouseDown(e);
            }
            System.Drawing.Point mousePos;
            mousePos = RenderContextUtility.ConvertPoint(GetMousePosition(e));

            if (ActiveViewport.ToolBar.Contains(mousePos))
            {
                base.OnMouseDown(e);

                return;
            }



            //createMyRectangleBlock();


            devDept.Eyeshot.Entities.Point point = new devDept.Eyeshot.Entities.Point(current);
            if (drawingPoints)
            {
                point.LineWeightMethod = colorMethodType.byEntity;
                point.LineWeight = 3;
                Entities.Add(point);

            }
            else
            {
                points.Add(current);

                if (drawingLine && points.Count == 2)
                {


                    Vector3D direction = new Vector3D(points[0], points[1]);


                    //direction.Normalize();
                    Point3D endPoint = points[0] + direction;

                    //Point3D endPoint = points[1];


                    Line line = new Line(points[0], endPoint);

                    Entities.Add(line);
                    drawingLine = false;
                }
                else if (drawingCircle && points.Count == 2)
                {
                    double radius = points[0].DistanceTo(points[1]);
                    Circle circle = new Circle(planeXY, points[0], radius);
                    Entities.Add(circle);
                    drawingCircle = false;
                }
                else if (drawingArc && points.Count == 3)
                {
                    double radius = points[0].DistanceTo(points[1]);
                    Arc arc = new Arc(planeXY, points[0], radius, startAngle, endAngle);
                    //Arc arc = new Arc(planeXY, planeXY.Origin, radius, 0,arcSpanAngle);


                    //Arc arc = new Arc(DrawingPlane, DrawingPlane.Origin, radius, 0, arcSpanAngle);

                    Entities.Add(arc);

                    drawingArc = false;
                }
                else if (doingMove && points.Count == 2)
                {
                    foreach (Entity ent in this.selEntities)
                    {
                        Vector3D movement = new Vector3D(points[0], points[1]);
                        ent.Translate(movement);
                    }

                    Entities.Regen();
                    ClearAllPrevious();

                }
                else if (doingRotate && points.Count == 3)
                {
                    foreach (Entity ent in this.selEntities)
                    {
                        ent.Rotate(arcSpanAngle, Vector3D.AxisZ, points[0]);
                    }

                    Entities.Regen();
                    ClearAllPrevious();

                }

                else if (drawingRectangle && points.Count == 3 && rectangleLastPoint!=null)
                {
                
                    Line line1 = new Line(points[0], points[1]);
                    Line line2 = new Line(points[1], points[2]);
                    Line line3 = new Line(points[2], rectangleLastPoint);
                    Line line4 = new Line(rectangleLastPoint, points[0]);

                    Entities.Add(line1);
                    Entities.Add(line2);
                    Entities.Add(line3);
                    Entities.Add(line4);
                  
                    drawingRectangle = false;

                }
                else if (perpendicularSnap )
                {



                    LinearPath lp = new LinearPath(points);

                   

                    if (!isExtrude)
                    {
                        Entities.Add(lp);

                    }
                    else
                    {
                        Vector3D extrusionVector = new Vector3D(0, 0, 10);
                        Brep brep = lp.ExtrudeAsBrep(extrusionVector, tolerance: 0.60);
                        Entities.Add(brep);

                        //Mesh brep = lp.ExtrudeAsMesh(extrusionVector, 0.60, Mesh.natureType.Plain);
                        //Entities.Add(brep);

                        //Surface[] brep = lp.ExtrudeAsSurface(extrusionVector,0.45,0.60);
                        //if (brep.Length > 0) { 
                        //Entities.Add(brep[brep.Length-1]);
                        //}
                    }
                    //perpendicularSnap = false;

                }


            }

            Invalidate();
            base.OnMouseDown(e);
        }

        public void doExtrusionOfSelectedEnitites()
        {

            Line line = new Line(new Point3D(0, 0, 0), new Point3D(10, 0, 0));
            Vector3D extrusionVector = new Vector3D(0, 0, 10);
            Brep brep = line.ExtrudeAsBrep(extrusionVector, tolerance: 0.60);
            Entities.Add(brep);
            Invalidate();
        }


        protected override void OnMouseMove(System.Windows.Input.MouseEventArgs e)
        {
            mouseLocation = RenderContextUtility.ConvertPoint(GetMousePosition(e));

            if (ObjectSnapEnabled)
            {
                //snapPoint = null;
                //snapPoints = GetSnapPoints(mouseLocation);
                //int index = GetEntityUnderMouseCursor(mouseLocation);
                //SelectedItem selItem = GetItemUnderMouseCursor(mouseLocation);
                Entity ent = GetEntityByPosition(mouseLocation);
                handleObjectSnapping(ent);
            }
            


            if (drawingPoints)
                activeOperationLabel = "Points: ";
            else if (drawingLine)
                activeOperationLabel = "Line: ";
            else if (drawingCircle)
                activeOperationLabel = "Circle: ";
            else if (drawingArc)
                activeOperationLabel = "Arc: ";
            else if (doingMove)
                activeOperationLabel = "Move: ";
            else if (doingRotate)
                activeOperationLabel = "Rotate: ";
            else if (drawingRectangle)
                activeOperationLabel = "Draw Rectangle";
            else
                activeOperationLabel = "";


            if (current == null || ActionMode != actionType.None || ActiveViewport.ToolBar.Contains(mouseLocation))
            {
                base.OnMouseMove(e);

                return;
            }
          
            PaintBackBuffer();
            SwapBuffers();

            base.OnMouseMove(e);

        }


        protected  void handleObjectSnapping(Entity ent)
        {
            if (ent != null)
            {

                if (ent is Point)
                {
                    devDept.Eyeshot.Entities.Point point = (devDept.Eyeshot.Entities.Point)ent;

                    switch (ActiveObjectSnap)
                    {
                        case objectSnapType.Point:
                            myCustomSnapPoint = new SnapPoint(point.Vertices[0], objectSnapType.Point);
                            break;
                    }
                }
                else if (ent is Line) //line
                {
                    Line line = (Line)ent;

                    switch (ActiveObjectSnap)
                    {
                        //case objectSnapType.Start:
                        //    snapPoints = new SnapPoint[]{ new SnapPoint(line.StartPoint, objectSnapType.Start)};
                        //    break;
                        //case objectSnapType.End:
                        //    snapPoints = new SnapPoint[]{ new SnapPoint(line.EndPoint, objectSnapType.End)};
                        //    break;
                        //case objectSnapType.Mid:
                        //    snapPoints = new SnapPoint[] { new SnapPoint(line.MidPoint, objectSnapType.Mid)};
                        //    break;
                        case objectSnapType.Start:
                            myCustomSnapPoint = new SnapPoint(line.StartPoint, objectSnapType.Start);
                            break;
                        case objectSnapType.End:
                            myCustomSnapPoint = new SnapPoint(line.EndPoint, objectSnapType.End);
                            break;
                        case objectSnapType.Mid:
                            myCustomSnapPoint = new SnapPoint(line.MidPoint, objectSnapType.Mid);
                            break;

                    }
                }
                else if (ent is Arc) //Arc
                {
                    Arc arc = (Arc)ent;

                    switch (ActiveObjectSnap)
                    {
                        case objectSnapType.End:
                            myCustomSnapPoint = new SnapPoint(arc.EndPoint, objectSnapType.End);
                            break;
                        case objectSnapType.Start:
                            myCustomSnapPoint = new SnapPoint(arc.StartPoint, objectSnapType.Start);
                            break;
                        case objectSnapType.Mid:
                            myCustomSnapPoint = new SnapPoint(arc.MidPoint, objectSnapType.Mid);
                            break;
                        case objectSnapType.Center:
                            myCustomSnapPoint = new SnapPoint(arc.Center, objectSnapType.Center);
                            break;
                    }
                }
                else if (ent is Circle) //Circle
                {
                    Circle circle = (Circle)ent;

                    switch (ActiveObjectSnap)
                    {
                        case objectSnapType.End:
                            myCustomSnapPoint = new SnapPoint(circle.EndPoint, objectSnapType.End);
                            break;
                        case objectSnapType.Start:
                            myCustomSnapPoint = new SnapPoint(circle.StartPoint, objectSnapType.Start);
                            break;
                        case objectSnapType.Mid:
                            myCustomSnapPoint = new SnapPoint(circle.PointAt(circle.Domain.Mid), objectSnapType.Mid);
                            break;
                        case objectSnapType.Center:
                            myCustomSnapPoint = new SnapPoint(circle.Center, objectSnapType.Center);
                            break;
                    }
                }



            }
            else
            {
                myCustomSnapPoint = null;
                //snapPoints = null;
            }
        }

        protected override void DrawOverlay(DrawSceneParams myParams)
        {


            ScreenToPlane(mouseLocation, planeXY, out current);

            if (ActionMode == actionType.None && !doingMove && !doingRotate)
            {
                if (isSnapGridEnabled)
                {
                    if (SnapToGrid(ref current))
                        RenderContext.SetLineSize(4);

                }


                if (ObjectSnapEnabled && myCustomSnapPoint != null)
                {
                    current = myCustomSnapPoint;
                    RenderContext.SetLineSize(4);

                }

                //DrawCrossHairMousePointer();



                //DrawText(mouseLocation.X, (int)Size.Height - mouseLocation.Y + 10,
                //                    activeOperationLabel + "X = " + current.X.ToString("f2") + ", " +
                //                    "Y = " + current.Y.ToString("f2"),
                //                    new System.Drawing.Font("Tahoma", 10f),
                //                    Color.Black, ContentAlignment.BottomLeft);




            }

            if (drawingLine && points.Count == 1)
            {
                DrawInteractiveLines();
            }
            else if (drawingCircle && points.Count == 1)
            {
                DrawInteractiveCircle();
            }
            else if (drawingArc && points.Count > 0)
            {
                DrawInteractiveArc();
            }
            else if (doingMove)
            {

                DrawInteractiveMove();

            }
            else if (doingRotate)
            {
                DrawInteractiveRotate();
            }
            else if (drawingRectangle && points.Count >0 ) {
                DrawInteractiveRectangle();
            }
            else if (perpendicularSnap && points.Count > 1)
            {
                DrawInteractivePerpendicularSnap(ref current);
            }

            if (ActionMode == actionType.None && !doingMove && !doingRotate && current!=null)
            {
                DrawCrossHairMousePointer();




                DrawText(mouseLocation.X, (int)Size.Height - mouseLocation.Y + 10,
                                    activeOperationLabel + "X = " + current.X.ToString("f2") + ", " +
                                    "Y = " + current.Y.ToString("f2"),
                                    new System.Drawing.Font("Tahoma", 10f),
                                    Color.Black, ContentAlignment.BottomLeft);
            }

                base.DrawOverlay(myParams);

        }

        private Point3D ScreenPoint(Point3D point, bool isOnScreen)
        {
            return isOnScreen ? point : WorldToScreen(point);
        }

        private const int magnetRange = 3;
        internal bool SnapToGrid(ref Point3D ptToSnap)
        {
            double gridSnap = ActiveViewport.Grid.Step;
            Point2D gridPoint = new Point2D(Math.Round(ptToSnap.X / gridSnap) * gridSnap, Math.Round(ptToSnap.Y / gridSnap) * gridSnap);

            if (Point2D.Distance(gridPoint, ptToSnap) < magnetRange)
            {
                ptToSnap.X = gridPoint.X;
                ptToSnap.Y = gridPoint.Y;

                return true;
            }

            return false;
        }


        public void DrawCrossHairMousePointer()
        {
            if (current!=null) { 
            double len = 40F;

            Point2D currentScreen = WorldToScreen(current);

            RenderContext.DrawLine(
             new Point2D(currentScreen.X - len, currentScreen.Y),
             new Point2D(currentScreen.X + len, currentScreen.Y));

            RenderContext.DrawLine(
            new Point2D(currentScreen.X, currentScreen.Y - len),
            new Point2D(currentScreen.X, currentScreen.Y + len));
            }
        }

     


        private Point2D[] GetScreenVertices(IList<Point3D> vertices)
        {
            Point2D[] screenPts = new Point2D[vertices.Count];

            for (int i = 0; i < vertices.Count; i++)
            {
                screenPts[i] = WorldToScreen(vertices[i]);
            }
            return screenPts;
        }

        
        // handle interactive ness of various entities


        // Draws interactive/elastic lines as per user clicks on mouse move
        private void DrawInteractiveLines()
        {
            if (points.Count == 0)
                return;

            //Point2D[] screenPts = GetScreenVertices(points);
            //RenderContext.DrawLineStrip(screenPts);
            //RenderContext.DrawLineStrip(WorldToScreen(points[0]));


            if (ActionMode == actionType.None && !ActiveViewport.ToolBar.Contains(mouseLocation) && points.Count > 0)
            {
                // Draw elastic line
                //RenderContext.DrawLine(screenPts[screenPts.Length - 1], WorldToScreen(current));
                RenderContext.DrawLine(WorldToScreen(points[0]), WorldToScreen(current));

            }
        }
        private void DrawInteractiveRectangle()
        {
            if(points.Count == 1)
            {
                RenderContext.DrawLine(WorldToScreen(points[0]), WorldToScreen(current));

            }
            if (points.Count == 2)
            {
                RenderContext.DrawLine(WorldToScreen(points[0]), WorldToScreen(points[1]));

            
                Vector3D dirVector1 = new Vector3D(points[0], points[1]);
                dirVector1.Normalize();

                Vector3D currentVector = new Vector3D(points[1], current);

                // logic for 3rd point projection
                Vector3D projection = Vector3D.Cross(dirVector1, Vector3D.Cross(currentVector, dirVector1));
                projection.Normalize();

                Point3D newCurrent = points[1] + projection * currentVector.Length;


                rectangleLastPoint = points[0] + projection * currentVector.Length;


                RenderContext.DrawLine(WorldToScreen(points[1]), WorldToScreen(newCurrent));

                RenderContext.DrawLine(WorldToScreen(points[0]), WorldToScreen(rectangleLastPoint));

                RenderContext.DrawLine(WorldToScreen(rectangleLastPoint), WorldToScreen(newCurrent));

                current = newCurrent;
            }
        }

        private void DrawInteractivePerpendicularSnap(ref Point3D ptToSnap)
        {
            if (points.Count == 0)
                return;

            Point2D[] screenPts = GetScreenVertices(points);

            RenderContext.DrawLineStrip(screenPts);

            if (ActionMode == actionType.None && !ActiveViewport.ToolBar.Contains(mouseLocation) && points.Count > 0)
            {
                // Draw elastic line
                RenderContext.DrawLine(screenPts[screenPts.Length - 1], WorldToScreen(current));
            }

            if(points.Count >= 2)
            {

                Vector3D dirVector1 = new Vector3D(points[points.Count-2], points[points.Count - 1]);
                dirVector1.Normalize();

                Vector3D currentVector = new Vector3D(points[points.Count - 1], current);

                // logic for 3rd point projection
                Vector3D projection = Vector3D.Cross(dirVector1, Vector3D.Cross(currentVector, dirVector1));
                projection.Normalize();

                Point3D newCurrent = points[points.Count - 1] + projection * currentVector.Length;

                double cosineAngle = Vector3D.Dot(projection, currentVector) / (projection.Length * currentVector.Length);

                // Check if the angle is less than 20 degrees (cosine(20 degrees) ~ 0.94)
                double angleThreshold = Math.Cos(20 * Math.PI / 180); // 20 degrees in radians

                if (cosineAngle >= angleThreshold)
                {
               
                    ptToSnap = newCurrent;
                  
                }



                //current = newCurrent;
            }
        }
        private void DrawInteractiveCircle()
        {
            radius = points[0].DistanceTo(current);

            if (radius > 1e-3)
            {
                Circle tempCircle = new Circle(planeXY, points[0], radius);
                DrawScreenCurve(tempCircle);
            }

        }
        private void DrawScreenCurve(ICurve curve)
        {
            const int subd = 100;

            Point3D[] pts = new Point3D[subd + 1];

            for (int i = 0; i <= subd; i++)
            {
                pts[i] = WorldToScreen(curve.PointAt(curve.Domain.ParameterAt((double)i / subd)));
            }

            RenderContext.DrawLineStrip(pts);
        }


        private void DrawInteractiveArc()
        {
            Point2D[] screenPts = GetScreenVertices(points);

            RenderContext.DrawLineStrip(screenPts);


            if (ActionMode == actionType.None && !ActiveViewport.ToolBar.Contains(mouseLocation) && points.Count > 0)
            {
                // Draw elastic line
                RenderContext.DrawLine(WorldToScreen(points[0]), WorldToScreen(current));

                //draw three point arc
                if (points.Count == 2)
                {

                    radius = points[0].DistanceTo(points[1]);

                    if (radius > 1e-3)
                    {
                        

                        Vector2D v1 = new Vector2D(points[0], points[1]);
                        v1.Normalize();
                      
                        Vector2D v2 = new Vector2D(points[0], current);
                        v2.Normalize();

                        arcSpanAngle = Vector2D.SignedAngleBetween(v1, v2);

                        // Normalize the vectors
                        

                        // Calculate the angle between the vectors
                        startAngle = Math.Atan2(v1.Y, v1.X);
                        endAngle = Math.Atan2(v2.Y, v2.X);

                        // Determine the minimum angle and adjust accordingly
                        if (Math.Abs(endAngle - startAngle) > Math.PI)
                        {
                            if (endAngle > startAngle)
                            {
                                endAngle -= 2 * Math.PI; // Adjust to the smaller angle
                            }
                            else
                            {
                                startAngle -= 2 * Math.PI; // Adjust to the smaller angle
                            }
                        }

                        if (Math.Abs(arcSpanAngle) > 1e-3)
                        {

                            Arc tempArc = new Arc(planeXY, points[0], radius, startAngle, endAngle);
                            //Arc tempArc = new Arc(planeXY, points[1], radius, 0, arcSpanAngle);

                            //Arc tempArc = new Arc(DrawingPlane, DrawingPlane.Origin, radius, 0, arcSpanAngle);


                            DrawScreenCurve(tempArc);

                        }

                    }
                }

            }
        }

        

        private void DrawInteractiveMove()
        {
            if (points.Count == 0)
            {
                DrawText(mouseLocation.X, Size.Height - mouseLocation.Y + 10, "Select base point",
                    new System.Drawing.Font("Tahoma", 8.25f), Color.Black, ContentAlignment.BottomLeft);

                return;
            }
            else if (points.Count == 1)
            {

                DrawText(mouseLocation.X, Size.Height - mouseLocation.Y + 10, "Select second point",
                    new System.Drawing.Font("Tahoma", 8.25f), Color.Black, ContentAlignment.BottomLeft);

                // Show temp entity for current movement state
                foreach (Entity ent in this.selEntities)
                {
                    Entity tempEntity = (Entity)ent.Clone();
                    Vector3D tempMovement = new Vector3D(points[0], current);
                    tempEntity.Translate(tempMovement);
                    
                    DrawScreenCurve(tempEntity as ICurve);
             

                }
            }
        }
        private void DrawInteractiveRotate() {
            DrawInteractiveArc();
            if (points.Count == 0)
            {

                DrawText(mouseLocation.X, Size.Height - mouseLocation.Y + 10, "Select center of rotation",
                    new System.Drawing.Font("Tahoma", 8.25f), Color.Black, ContentAlignment.BottomLeft);

            }
            else if (points.Count == 1)
            {

                DrawText(mouseLocation.X, Size.Height - mouseLocation.Y + 10, "Select first reference point",
                    new System.Drawing.Font("Tahoma", 8.25f), Color.Black, ContentAlignment.BottomLeft);

            }
            else if (points.Count == 2)
            {

                DrawText(mouseLocation.X, Size.Height - mouseLocation.Y + 10, "Select second reference point",
                    new System.Drawing.Font("Tahoma", 8.25f), Color.Black, ContentAlignment.BottomLeft);

                // Show temp entities for current rotation state
                foreach (Entity ent in this.selEntities)
                {
                    Entity tempEntity = (Entity)ent.Clone();
                    tempEntity.Rotate(arcSpanAngle, Vector3D.AxisZ, points[0]);

                    DrawScreenCurve(tempEntity as ICurve);
                }
            }
        }
        public void ClearAllPrevious()
        {
           
            points.Clear();
            
            drawingPoints = false;
            drawingLine = false;
            drawingArc = false;
            drawingCircle = false;
            drawingRectangle = false;
            perpendicularSnap = false;
            doingMove = false;
            doingRotate = false;
            isExtrude = false;
            


            activeOperationLabel = "";
            ActionMode = actionType.None;
            Entities.ClearSelection();
            ObjectManipulator.Cancel();
        }

        

        public Entity GetEntityByPosition(System.Drawing.Point location)
        {
            SelectedItem item = GetItemUnderMouseCursor(location);
            if (item != null)
                return item.Item as Entity;
            return null;
        }

    }

    public class SnapPoint : devDept.Geometry.Point3D
    {
        public objectSnapType Type;

        public SnapPoint()
            : base()
        {
            Type = objectSnapType.None;
        }

        public SnapPoint(Point3D point3D, objectSnapType objectSnapType) : base(point3D.X, point3D.Y, point3D.Z)
        {
            this.Type = objectSnapType;
        }

        public override string ToString()
        {
            return base.ToString() + " | " + Type;
        }
    }




}
