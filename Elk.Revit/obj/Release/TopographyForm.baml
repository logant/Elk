   M S B A M L   `   `   `  ����   ElkTopography    Elk.Revit.TopographyForm   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008       � Title$  Elk Topography��
������  � WindowStyle$
 None=� � AllowsTransparency .  � SizeToContent$ WidthAndHeight=� � WindowStartupLocation$ CenterScreen=� � 
ResizeMode$ NoResize=�$	��260��$	��410�� � 	Resources    typicalButtonStyle �� 
TargetType �� Property	 �� Value
 �� Triggers P� Property P� Value �� 
StartPoint �� EndPoint � Color � Offset��%�  &	        �� " ��.���  � ��	 �	#FF404040��  � ��$	 16(���  � ��$	 Center=���  � ��$	 Center=���  � ��	 �	#FF606060
 P�  �@J�IsMouseOver . . ���  � ��	 �� $ 0.5,0&�$ 0.5,1&�.��� $ 	#FF606060��$	 1.0Q�� $ 	#FF8B8B8B��$	 0.0Q���  � ��	 �� $ 0.5,0&�$ 0.5,1&�.��� $ 	#FF606060��$	 1.0Q�� $ 	#FF8B8B8B��$	 0.0Q�.���� -   
������ ._�� .+��� ���	#FF404040$	��400��$	��250��$��Center=�$��Center=�$��5q� J� Effect -  'System.Windows.Media.Effects.BlurEffect     Radius$ 10Q��� 
�������$��6q�$��Stretch=�$��Stretch=��� ���	#FF404040$��1��$
��Left=�$��Stretch=�$��5,5,0,5q��� ���	#FF404040$��1��$��Right=�$��Stretch=�$��0,5,5,5q��� ���	#FF404040$��1��$��Stretch=�$	��Top=�$��5,5,5,0q��� ���	#FF404040$��1��$��Stretch=�$��Bottom=�$��5,0,5,5q��� -    ��Name$ closeButton��#��[  $��Right=�$��80��$��30��$��Bottom=�$��0,0,6,6q���� $��80��$��30��.+��� -    ��Name$ closeButtonRect��
������ �� $��Cancel�� �� TextAlignment$ Center=�����  �� 
TargetType" ��.R��� #��z��#��z��#��z�� J� SnapsToDevicePixels# z #��z����. #��z���� -   $ okButton��#��[  $��Right=�$��80��$��30��$��Bottom=�$��0,0,86,6q���� $��80��$��30��.+��� -   $ okButtonRect��
������ �� $��OK��$ Center=����� " ��.R��� #��z��#��z��#��z��# z #��z����. #��z���� $��Elk Topography��$
��Left=�$��10,5,0,0q�$	��Top=�$��18(����	#FF404040$��Arial)�$
��Bold"��� $��Load a File:��$
��Left=�$��	15,40,0,0q�$	��Top=����	#FF404040$��Arial)��� -   $��fileTextBox��$
��Left=�$��23��$��	15,65,0,0q� �� TextWrapping$
 Wrap=�$�� ��$	��Top=�$	��345���� -   $ browseButton��#��[  $��Right=�$��30��$��30��$	��Top=�$��	0,60,15,0q���� $��30��$��30��.+��� -   $ browseButtonRect��
������ �� $	��...��$ Center=�$��Bottom=����� " ��.R��� #��z��#��z��#��z��# z #��z����. #��z���� $��Coordinate Range��$
��Left=�$��
15,100,0,0q�$	��Top=�$	��105��$	��380��.��� .+��� $��Min Latitude:��$
��Left=�$	��Top=�$��5,15,0,0q��� $��Max Latitude:��$
��Left=�$	��Top=�$��5,45,0,0q��� -	   $��minLatTextBox��$
��Left=�$	��Top=�$��	90,18,0,0q�$��75��$��23���� -
   $��maxLatTextBox��$
��Left=�$	��Top=�$��	90,48,0,0q�$��75��$��23���� $��Min Longitude:��$
��Left=�$	��Top=�$��
180,15,0,0q��� $��Max Longitude:��$
��Left=�$	��Top=�$��
180,45,0,0q��� -   $��minLonTextBox��$
��Left=�$	��Top=�$��
275,18,0,0q�$��75��$��23���� -   $��maxLonTextBox��$
��Left=�$	��Top=�$��
275,48,0,0q�$��75��$��23��