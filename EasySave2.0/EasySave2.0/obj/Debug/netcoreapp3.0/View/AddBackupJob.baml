   M S B A M L   `   `   `  ����F  BEasySave2.0, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:EasySaveEasySave  5   	       EasySave.AddBackupJob   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:EasySave     5   	     � Title$  AddBackupJob��5   	   $	��450��6   $	��800��6+   .��5
      ��  �� Padding$ 20q�6   ._�5   
   �� .+�5      �� -   5         ��Name$
 back��5      $	��←��6#   $��30��5      $��30��5      $	��Top=�5      $
��Left=�5      $��0,0,0,10q�5      
�������
���   �
������ 5   (   �� 5      -   5         ��Name$ Title��5      $&�� Ajouter un travail de sauvegarde��6'   $��24(�6O   $
��Bold"�6]   $��0,0,0,10q�6o   �� 5      -   5        $	 Nme��5      $	��Nom��6%   $��0,0,0,5q�60   �� 5      -   5         ��Name$ namejob��5      $��0,0,0,10q�6'   �� 5      -   5        $	 Src��5      $��Source��6%   $��0,0,0,5q�63   �� 5      -   5        $ source��5      $��0,0,0,10q�6&   �� 5      -   5        $
 Dest��5      $��Destination��6&   $��0,0,0,5q�69   �� 5      -   5        $ target��5      $��0,0,0,10q�6&   �� 5       -	   5        $	 Typ��5       $��Type de Sauvegarde��6%   $��0,0,0,5q�6?   �� 5!      -
   5         ��Name$ 
saveOption��5!       �� SelectedValuePath$ Content��6+   $��0,0,0,10q�6G   .@�5"      �� -   5         ��Name$ typeOption1��5"      $��Complet��64   �� 5#      -   5        $ typeOption2��5#      $��Differentielle��64   5$      � 5&      $��0,10,0,5q�6    � ColumnDefinitions 5'      �� 5(      $
��Auto �6'   �� 5)      $��* �6'   .+�5+      �� -   5        	 ��Name$	 	iscrypted��5+      
���   �$
��Left=�6B   $��Center=�6]   �� 5,      $&�� Cocher pour crypter les fichiers��6   $��1��6D   $��Center=�6T   $��5,0,0,0q�6o   5-      �� 5/      -   5        $ Ajouter��5/      $��Ajouter��50      $	��100��52      $��Center=�53      $��0,10,0,0q�54      
���   �
�������
������ $��28��58      59      5:      5;      