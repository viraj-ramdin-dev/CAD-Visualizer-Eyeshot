   M S B A M L   `   `   `  ����L  HWpfDraftingSample, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:WpfDraftingSampleWpfDraftingSample  5   	   a ]devDept.Eyeshot.v2023, Version=2023.3.630.0, Culture=neutral, PublicKeyToken=9440825e8b4733bcP<clr-namespace:devDept.Eyeshot;assembly=devDept.Eyeshot.v2023devDept.Eyeshot 5   I   "    WpfDraftingSample.MainWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:WpfDraftingSample     5   	   m idevDept.Eyeshot.Control.Wpf.v2023, Version=2023.3.630.0, Culture=neutral, PublicKeyToken=f3cd437f0d8061b5=ddes2http://schemas.devdept.com/winfx/2008/xaml/control  5   	   JEyeshot<clr-namespace:devDept.Eyeshot;assembly=devDept.Eyeshot.v2023  6I     � Title$  
MainWindow��5	   	   $	��450��6   $	��800��6)   .��5      � .+�5   
   +  %System.Windows.Controls.Ribbon.Ribbon  ���#f3f3f3   ApplicationMenu 6&   Z� .@�5      .  (System.Windows.Controls.Ribbon.RibbonTab  $��Drafting��6   ���#ffffff.@�5      0  *System.Windows.Controls.Ribbon.RibbonGroup  $��Shapes��6   ���#e3e3e3.@�5      �� -   5         ��Name$ AddPointButton��5      $��
Add Points��65   ���#4681f4���#ffffff$��10q�5      $��20,10q�5      $��20(�5      $
��Bold"�5      $	��153��6   5      �� 5      -   5        $ AddLineButton��5      $��Add Line��64   ���#4681f4���#ffffff$��10q�5   
   $��20,10q�5   
   $��20(�5    
   $
��Bold"�5!   
   $	��153��6   6(   �� 5$      -   5        $ AddArcButton��5$      $��Add Arc��63   ���#4681f4���#ffffff$��10q�5'   
   $��20,10q�5(   
   $��20(�5)      $
��Bold"�5*   
   5+      �� 5,      -   5        $ AddCircleButton��5,      $��
Add Circle��66   ���#4681f4���#ffffff$��10q�5/   
   $��20,10q�50   
   $��20(�51      $
��Bold"�52   
   53      �� 54      -   5        $ showGridButton��54      $
��Grid��68   ���#4681f4���#ffffff$��10q�57   
   $��20,10q�58   
   $
��Bold"�59   
   $��20(�5:   
   5;   
   �� 5=      -   5        $ AddRectangleButton��5=      $��Add Rectangle��69   ���#4681f4���#ffffff$��10q�5@      $��20,10q�5A      $��20(�5B      $
��Bold"�5C      5D      �� 5F      -   5        $ PerpendcularSnapButton��5F      $��Perpendicular Snap��6=   ���#4681f4���#ffffff$��10q�5I      $��20,10q�5J      $��20(�5K      $
��Bold"�5L      5M      �� 5N      -   5        $ ExtrudeButton��5N      $��Extrude��64   ���#4681f4���#ffffff$��10q�5Q      $��20,10q�5R      $��20(�5S      $
��Bold"�5T      5U      5X      5Y        5\      $��Snapping��6   ���#1a73e8.@�5]        $��Options��6   ���#e3e3e3.@�5_      �� -	   5        $ SnapGridButton��5_      $��	Grid Snap��65   ���#4681f4���#ffffff$��10q�5b   
   $��20,10q�5c      $��19(�5d      $
��Bold"�5e   
   5f      �� 5h      -
   5        $ SnapObjectButton��5h      $��Object Snap��67   ���#4681f4���#ffffff$��10q�5k   
   $��20,10q�5l      $��19(�5m      $
��Bold"�5n   
   5o      �� 5q      -   5         ��Name$ 	snapPanel��5q   !   ${�False��64   $
��Left=�6F    �� Orientation$ 
Horizontal=�6a   .+�5t      	� -   5         	�Name$ startRadioButton��5t   &   $��Start��6@   ���#4681f4$��10q�5u   
   $��19(�5w      $
��Bold"�5x   
   	� 5y      -   5        $ midRadioButton��5y   &   $	��Mid��6>   ���#4681f4$��10q�5z   
   $��19(�5|      $
��Bold"�5}   
   	� 5~      -   5        $ endRadioButton��5~   &   $	��End��6>   ���#4681f4$��10q�5   
   $��19(�5�      $
��Bold"�5�   
   	� 5�      -   5        $ centerRadioButton��5�   &   $��Center��6A   ���#4681f4$��10q�5�   
   $��19(�5�      $
��Bold"�5�   
   	� 5�      -   5        $ pointRadioButton��5�   &   $��Point��6@   ���#4681f4$��10q�5�   
   $��19(�5�      $
��Bold"�5�   
   5�      5�      5�        5�      $��Manipulating��6   ���#1a73e8.@�5�        $��Options��6&   ���#e3e3e3.@�5�      �� -   5        $ SelectButton��5�   %   $��select��6;   ���#4681f4���#ffffff$��10q�5�   
   $��20,10q�5�      $��19(�5�      $
��Bold"�5�   
   5�      �� 5�      -   5        $ selectPanel��5�   !   ${�False��66   $
��Left=�6H   $ 
Horizontal=�6c   .+�5�      	� -   5        $ selectByPick��5�   &   $��By Pick��6<   ���#4681f4$��10q�5�   
   $��19(�5�      $
��Bold"�5�   
   	� 5�      -   5        $ selectByBox��5�   &   $��By Box��6;   ���#4681f4$��10q�5�   
   $��19(�6   $
��Bold"�6$   5�      �� 5�      -   5        $ DeleteButton��5�      $��Delete��63   ���#4681f4���#ffffff$��10q�5�   
   $��20,10q�5�      $��19(�5�      $
��Bold"�5�   
   5�      �� 5�      -   5        $ 
MoveButton��5�      $
��Move��61   ���#4681f4���#ffffff$��10q�5�   
   $��20,10q�5�      $��19(�5�      $
��Bold"�5�   
   5�      �� 5�      -   5        $ RotateButton��5�      $��Rotate��63   ���#4681f4���#ffffff$��10q�5�   
   $��20,10q�5�      $��19(�5�      $
��Bold"�5�   
   5�      �� 5�      -   5        $ CheckParallel��5�      $��Check 2 lines��64   ���#4681f4���#ffffff$��10q�5�   
   $��20,10q�5�      $��19(�5�      $
��Bold"�5�   
   5�      5�      5�        5�      $��Saving File��6   ���#1a73e8.@�5�        $��Options��6   ���#e3e3e3.@�5�      �� -   5        $ OpenFile��5�      $��	Open File��6/   ���#4681f4���#ffffff$��10q�5�   
   $��20,10q�5�      $��19(�5�      $
��Bold"�5�   
   5�      �� 5�      -   5        $ SaveFile��5�      $��	Save File��6/   ���#4681f4���#ffffff$��10q�5�   
   $��20,10q�5�      $��19(�5�      $
��Bold"�5�   
   5�      5�      5�      5�      � 5�   
   -   5        $��MyPopup��5�      $	��450��6   $	��100��6+   $��Mouse=�5�       � 	StaysOpen$ False��6#   .��5�      �� 
����������#4681f4$��1q�6D   ._�5�      �� $��10q�6   .+�5�      �� -   5        $��PopupTextBlock��5�       $�� ��66   $��5q�6>   $��17(�6J   $
��Bold"�5�   
   �� 5�      -   5        $��Close��5�      $��60��6-   $��Center=�5�      $��Bottom=�5�      5�      5�      5      5     ]� 5  
   $��False��6   $��	0,140,0,0q�6*   .+�5         WpfDraftingSample.MyDesign  -   5          Name$ design1��5      J� RenderTransformOrigin$ 	0.5,0.848&�6.   '  !devDept.Eyeshot.Control.Workspace	   ProgressBar	 5     )  #devDept.Eyeshot.Control.ProgressBar  5     
   Lighting$
 False��6'   $  devDept.Eyeshot.Control.Design   	Viewports 5     &   devDept.Eyeshot.Control.Viewport     InitialView$	 Top=�6$      DisplayMode$ 	Wireframe=�66      Camera 5
     	  devDept.Eyeshot.Camera	  5      	  
ZoomFactor$	 1.5Q�6-    	  ProjectionMode$ Orthographic=�6>      OriginSymbols 5     *
  $devDept.Eyeshot.Control.OriginSymbol
  2  ,devDept.Eyeshot.Control.CoordinateSystemBase   Lighting$ False��60      CoordinateSystemIcon 5     2  ,devDept.Eyeshot.Control.CoordinateSystemIcon  5     $ False��68      
Background 5     0  *devDept.Eyeshot.Control.BackgroundSettings  5        	StyleMode$ Solid=�66      TopColor
 �����6I      Grids 5     "  devDept.Eyeshot.Control.Grid     Step$ 10Q�6(      Lighting$ False��62      ToolBars 5!     %  devDept.Eyeshot.Control.ToolBar     Position$ HorizontalTopCenter=�6+      Buttons 5#  &   /  )devDept.Eyeshot.Control.HomeToolBarButton  :  4devDept.Eyeshot.Control.MagnifyingGlassToolBarButton  5$  &   5  /devDept.Eyeshot.Control.ZoomWindowToolBarButton  5%  &   /  )devDept.Eyeshot.Control.ZoomToolBarButton  5&  &   .  (devDept.Eyeshot.Control.PanToolBarButton  5'  &   1  +devDept.Eyeshot.Control.RotateToolBarButton  5(  &   2  ,devDept.Eyeshot.Control.ZoomFitToolBarButton  5)  &   5+        ViewCubeIcon 5.     *  $devDept.Eyeshot.Control.ViewCubeIcon  5/        Lighting$ False��60   51     53     56     5=     5A     