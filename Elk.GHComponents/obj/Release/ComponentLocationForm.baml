   M S B A M L   `   `   `  ����   Elk2,    &Elk.GHComponents.ComponentLocationForm   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008       � Title$  Elk Location Settings��
������  � WindowStyle$
 None=� � AllowsTransparency .  � WindowStartupLocation$ Manual=� � 	Resources    typicalButtonStyle �� 
TargetType �� Property �� Value �� Triggers	 P� Property
 P� Value �� 
StartPoint �� EndPoint � Color � Offset��%�  &	        �� " ��.���  � �� �	#FF252525��  � ��$ 16(���  � ��$ Center=���  � ��$ Center=���  � �� �	#FFE8E8E8 P� 	 �@J�IsMouseOver
 . . ���  � �� �� $ 0.5,0&�$ 0.5,1&�.��� $ 	#FF606060��$	 1.0Q�� $ 	#FF8B8B8B��$	 0.0Q���  � �� �� $ 0.5,0&�$ 0.5,1&�.��� $ 	#FF606060��$	 1.0Q�� $ 	#FF8B8B8B��$	 0.0Q�.���� -   
������ ._�� $	��250��$	��150��.+��� ���	#FF404040$��Stretch=�$��Stretch=�$��5,5,5,5q� J� Effect -  'System.Windows.Media.Effects.BlurEffect     Radius$ 10Q��� 
�������$��5,5,5,5q�$��Stretch=�$��Stretch=��� ���	#FF404040$��1��$
��Left=�$��Stretch=�$��5,5,0,5q��� ���	#FF404040$��1��$��Stretch=�$	��Top=�$��5,5,5,0q��� ���	#FF404040$��1��$��Right=�$��Stretch=�$��0,5,5,5q��� ���	#FF404040$��1��$��Stretch=�$��Bottom=�$��5,0,5,5q��� -    ��Name$ closeButton��#��[  $��Right=�$��80��$��30��$��Bottom=�$��0,0,6,6q���� $��80��$��30��.+��� -    ��Name$ closeButtonRect��
������ �� $��Cancel�� �� TextAlignment$ Center=�����  �� 
TargetType" ��.R��� #��z��#��z��#��z�� J� SnapsToDevicePixels# z #��z����. #��z���� -   $ okButton��#��[  $��Right=�$��80��$��30��$��Bottom=�$��0,0,86,6q���� $��80��$��30��.+��� -   $ okButtonRect��
������ �� $��OK��$ Center=����� " ��.R��� #��z��#��z��#��z��# z #��z����. #��z���� $��Settings��$
��Left=�$��10,5,0,0q�$	��Top=�$��16(����	#FF404040$��Arial)�$
��Bold"��� $��Tab Name��$
��Left=�$	��Top=�$��	16,40,0,0q�$��Arial)����	#FF404040�� $��
Panel Name��$
��Left=�$	��Top=�$��	16,70,0,0q�$��Arial)����	#FF404040�� -   $��
tabTextBox��$��100,40,15,87q� �� TextWrapping$
 Wrap=�$��TextBox���� -   $��panelTextBox��$��100,70,15,57q�$
 Wrap=�$��TextBox��