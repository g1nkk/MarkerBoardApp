   M S B A M L   `   `   `  ����F  BMarkerBoard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:MarkerBoardMarkerBoard  5   	       MarkerBoard.MainWindow   5   	   W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:MarkerBoard     5   	     � AllowsTransparency$
  True��5   	    � WindowStyle$
 None=�5	   	    � Title$ 
MainWindow��5
   	   $	��800��6   $
��1350��6)    � WindowStartupLocation$ CenterScreen=�66    � 
ResizeMode$ NoResize=�6[   
������ .��5      � $��4q�6   $
}�True��6    � ColumnDefinitions 5   
   �� 5      $	��.2* �6   �� 5      .+�5   
   �� 
������� �� CornerRadius$ 20��6$   $��2��66   �� 5   
   -   5         ��Name$ 	inkCanvas��5      $��2��6'   $��6q�6;    � Cursor$	 Pen��6F   	 �� 
Background
	 �����
 �� EditingModeInverted$
 EraseByPoint=�6f   �� 5   
   -   5         ��Name$ 
showButton��5       J� Opacity$ 0Q�6%   $
��Left=�61      controlDefault#��[  6L   ��6t   �� *ShowButtonClick+.��5      �� $��50��6   $��/icons/show.png��6!   5      � 5   
   -   5         �Name$ InstrumentPanel��5      $��-168,5,1060,5q�6-   $��2��6D    J� RenderTransformOrigin$ 0.25,0.5&�6X   z�5      V� 5      .~�5      ��  �� ScaleX$ 1Q�6%   �� 5      �� 5       T� 5!      5"      .+�5%      �� #��[  6   $��20��6=   $��20��6H   $��	180,3,0,0q�6T   ��6g   �� *MinimizeButtonClick+.��5&      �� $��/icons/minimize.png��6   5'      �� 5(      #��[  6   $��20��6=   $��20��6H   $��5,3,0,0q�6U   ��6f   �� *CloseButtonClick+.��5)      �� $��/icons/close.png��6   5*      �� 5,      #��[  6   $��21��6=   $��21��6H   $��5,3,0,0q�6U   ��6f   �� *HideButtonClick+.��5-      �� $��/icons/hide.png��6   5.      �� 50      -   5        $ Eraser��50   S   
 � Tag$ 4��6     markers#��[ 6   $��
0, 5, 5, 5q�6?   .��51      �� $��/icons/eraser.png��6   $��40, 20, 0, 0q�64   $	��100��6J   $	��200��6W   52      �� 54      -   5        $ defaultMarker��54   >   $ 3��6   #��[ 6   $��
0, 5, 5, 5q�6V   $��55��6j   $	��250��6v   .��55      �� $��/icons/marker.png��6   $��70, 0, 0, 0q�63    J� Effect 56      3  -System.Windows.Media.Effects.DropShadowEffect  57         ShadowDepth$ 0Q�6+      Color$ White��6;   59      5:      �� 5<      $��50q�6   �� 5>      -   5        $ 0��5>      #��[ 6   $��56��6>   $	��240��6J   $��
0, 5, 5, 5q�6V   .��5?      �� $��/icons/blueMarker.png��6   $��70, 0, 0, 0q�67    5@        5A      $ 0Q�6+   $ White��6;   5C      5E      �� 5F      -   5        $ 1��5F      #��[ 6   $��56��6>   $	��240��6J   $��
0, 5, 5, 5q�6V   .��5G      �� $��/icons/redMarker.png��6   $��70, 0, 0, 0q�66    5H        5I      $ 0Q�6+   $ White��6;   5K      5L      �� 5M      -   5        $ 2��5M      #��[ 6   $��56��6>   $	��240��6J   $��
0, 5, 5, 5q�6V   .��5N      �� $��/icons/greenMarker.png��6   $��70, 0, 0, 0q�68    5O        5P      $ 0Q�6+   $ White��6;   5R      5S      �� 5T      -	   5         �� Orientation$ Vertical=�5T       � Maximum$ 40Q�6,    � Minimum$ 3Q�69   $	��150��6h   $��190,50,5,50q�6u    � Value$ 11Q�6�   	� 5U      -
   5         	�Name$ brushScaleImage��5U      $ 0.5,0.5&�63   $��15��6S   ${�False��6_   $��12q�6q   z�5V      V� 5W      .~�5X      �� $ 1Q�6)    �� ScaleY$ 1Q�64   �� 5Y      �� 5Z      T� 5[      5\      5^      �� 5`      #��[  6   $��
180,10,0,0q�6=   $��38��6Q   $��38��6]   ��6h   �� *CopyToClipboardClick+.��5a      �� $��/icons/copy.png��6   5b      �� 5d      #��[  6   $��	10,10,0,0q�6=   $��41��6P   $��41��6\   ��6g   �� *ClearCanvasButtonClick+.��5e      �� $��/icons/clear.png��6   5f      5g      �� 5i   
   -   5         ��Name$ NotificationsPanel��5i      $ 0Q�6   $��450,680,450,70q�69   $��2��6Q   $ 25��6e   ���	#FFECF5FF._�5j      �� -   5         ��Name$ NotificationsText��5j      $��MESSAGE��63   $��Center=�6B   $��Center=�6_   $��20(�6z   $��Bahnschrift)�6�   ���	#FF2929296�   5k      5l      5m      