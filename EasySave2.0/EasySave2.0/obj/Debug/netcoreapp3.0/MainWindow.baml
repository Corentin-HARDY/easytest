   M S B A M L   `   `   `  ����F  BEasySave2.0, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:EasySaveEasySave  5   	       EasySave.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:EasySave     5   	     � Title$  
MainWindow��5   	   $	��450��6   $	��800��6)    � 	Resources 5	         ButtonStyle �� 
TargetType �� Property �� Value=  7System.Windows.Controls.Primitives.DataGridColumnHeader*  $System.Windows.Controls.DataGridCell �� Triggers P� Property P� Value  AddButtonStyle �� BasedOn  SubtractButtonStyle��%�  &	        '      '  �    &	 �    7   &	 6    7   �� 5   
   " ��6$   .�5      ��  � ��
 �����6+   �� 5       � ��
 �   �6+   �� 5       � ��
 ���� 6,   �� 5       � ��$
 10,5q�6(   �� 5       � ��$ 5q�6'   �� 5       � ��$
 Bold"�6+   5      �� 5   
   "  6   .�5      ��  � ��
 �   �6+   �� 5       � ��
 �����6+   �� 5       � ��$ Center=�6;   �� 5       � ��$
 Bold"�6+   �� 5       � ��$ 32��6'   5      �� 5   
   "  6   .�5      ��  � ��$ 5q�6(   �� 5       � ��$ 0q�60    5       P� 5!       �@ 
IsSelected$
 True��60   . �5"      ��  � ��
 �����63   �� 5#       � ��
 �   �63   5$      5&      �� 5)   
   " ��6'   8   .�5*      ��  � ��
 � � �6+   5+      �� 5-   
   " ��6,   8   .�5.      ��  � ��
 �  ��6+   5/      50      .��51      � 	 � RowDefinitions	 52   
   �� 53      $
��Auto �6   �� 54      $��* �6   .+�58   
   ]� $��0��6   .+�5:      �� 
 �� Orientation$
 
Horizontal=�6   $
��Left=�62   .+�5;      �� -   5         ��Name$	 Add��5;      $��+��6&   #��[ 6D   $��30��6l   $��30��6w   $��5q�6�   �� 5<      -   5        $ Remove��5<      $��-��6)   #��[ 6J   $��30��6w   $��30��6�   $��10q�6�   �� 5>      -   5        $5��/pack://application:,,,/Images/settings_gear.png��5>      $��35��6Q   $��35��6\   $��5q�6h    � ToolTip$ Paramètres��6s   6�   �� 5?      -   5        $ Select��5?      $��Tout sélectionner��6)   #��[  6Z   $	��120��6   $��35��6�   �� 5@      -   5        $ Execute��5@      $��	Exécuter��6*   #��[  6S   $��90��6x   $��35��6�   5A      �� 5D      $��Right=�6   $
 
Horizontal=�60   $��Right=�6I   $��5q�6e   .+�5E      �� -   5        $3��-pack://application:,,,/Images/flag_france.png��5E      $��30��6O   $��35��6Z   $��10q�6f   $ 	Français��6r   6�   �� 5F      -   5        $;��5pack://application:,,,/Images/flag_united_kingdom.png��5F      $��30��6W   $��35��6b   $ Anglais��6n   6�   5G      5H      &   System.Windows.Controls.DataGrid  5M   
   -   5          Name$ datagrid��5M      
�������   SelectionUnit$ FullRow=�68      SelectionMode$ Extended=�6P   $��1��6i      AutoGenerateColumns$ False��6�      CanUserAddRows$ False��6�   $��Stretch=�6�   $��Stretch=�6�   ��6v   �� *
BackupJobs+   Columns 5N      0  *System.Windows.Controls.DataGridTextColumn  5O      -	   5        Name��,  &System.Windows.Controls.DataGridColumn   Header$	 Nom��5O   3      Width5  /System.Windows.Controls.DataGridLengthConverter$ * 6Y   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6@   �� *Name+6Y     5P      -
   5        
Source��$ Source��5P   5   $ * 6`    6E   �� *Source+6`     5Q      -   5        
Target��$ Destination��5Q   5   $ * 6e    6J   �� *Target+6e     5R      -   5        Type��$ Type de Sauvegarde��5R   3   $ * 6n    6O   �� *
BackupType+6n     5S      -   5        Crypted��$ Crypter��5S   6   $ * 6e    6G   �� *	IsCrypted+6e   5U      5V      5X      