   M S B A M L   `   `   `  ����C  ?practica, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:practicapractica  5   	       practica.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:practica     5   	   -   5          � Title$  Logare��5   	   $	��450��6   $	��350��6%    � SizeToContent$ WidthAndHeight=�61   $	��450��6P   $	��350��6`   $	��450��6o   $	��350��6    � Icon$ /favicon.ico��6�   .��5	      � $��Center=�6   $	��330��6(   $	��440��64   $	��Top=�6A    � ShowGridLines . .+�5
   
   �� -   5         ��Name$ 
Logo_image��5
      $��0,10,0,0q�6$   $��/linella-md.png��66   $
��Fill=�6O   $��Center=�6^   $	��330��6{   $	��150��6�   $	��Top=�6�    J� Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5         
BlurRadius$ 3Q�6#      ShadowDepth$ 2Q�62   5      � 5   
   $��0,165,0,-274q�6   $��Center=�6%   $	��354��6B   .+�5      �� -   5         ��Name$ Login_label��5      $��Login:��6)   $
��Left=�6:   $��26��6U   $��	32,87,0,0q�6a   $	��Top=�6t   $��41��6�   �� 5      -   5        $ Hello_label��5      $��Bine ati venit!��6)   $��Center=�6C   $��63��6`   $��0,10,0,0q�6l   $	��Top=�6~   $	��256��6�   $��40(�6�   $��Medium"�6�   	 � FocusVisualStyle	 5      �� 5      5      �� 5      -   5        $ 
Pass_label��5      $��Parola:��6(   $
��Left=�6:   $��26��6U   $��
32,118,0,0q�6a   $	��Top=�6u   $��45��6�   G� 5      -   5        
 G�Name$
 PassWord_Box��5      $
��Left=�60   $��26��6K   $��
82,118,0,0q�6W   $	��Top=�6k   $	��234��6�    G� Password$  ��6�   �� 5      -   5         ��Name$ Login_TextBox��5      $
��Left=�6-   $��26��6H   $��	82,87,0,0q�6T    �� TextWrapping$
 Wrap=�6g   $�� ��6{   $	��Top=�6�   $	��234��6�    �� 
AcceptsTab . ���	#FFABADB36�   �� 5      -   5         ��Name$ Login_Button��5      $��Login��6+   $
��Left=�6;   $��39��6V   $��
36,199,0,0q�6b   $	��Top=�6v   $	��110��6�   
�������$��2,2,2,2q�6�   6�   �� 5      -	   5        $ 	Qr_Button��5      $��QR-CODE��6(   $��Right=�6:   $��39��6V   $��
0,199,36,0q�6b   $	��Top=�6v   $	��110��6�   
�������$��2,2,2,2q�6�   6�   �� 5      -
   5        $ Iesire_button��5      $��Iesire��6,   $��Right=�6=   $��39��6Y   $��
0,199,36,0q�6e   $	��Top=�6y   $	��110��6�   
���  ��$��2,2,2,2q�6�   4  .System.Windows.Automation.AutomationProperties   IsRowHeader . $y�Hidden=�6  
�������6  �� 5      -   5        $ Start��5      $��Login��6$   $
��Left=�64   $��39��6O   $��
36,199,0,0q�6[   $	��Top=�6o   $	��110��6�   ���	#FF1CD42F$��2,2,2,2q�6�   $y�Hidden=�6�   
�������6�   5      �� 5   
   -   5         ��Name$ devicecombobox��5      $
��Left=�6+   $��22��6F   $��	33,17,0,0q�6R   $	��Top=�6e   $	��276��6}      IsOffscreenBehavior$ Onscreen=�6�   $y�Hidden=�6�   �� 5   
   -   5        $ Camera��5      $��0,54,0,0q�6    $��/linella-md.png��62   $��Center=�6K   $	��330��6h   $	��293��6t   $	��Top=�6�   $y�Hidden=�6�    � 
InputScope$ Default��6�    5         5!      $ 3Q�6#   $ 2Q�62   5#      5$      5%      