   M S B A M L   `   `   `  ����B  >Project, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:Project.ViewModelsProject.ViewModels  5   	   $    Project.Views.CreateExamWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   /vm clr-namespace:Project.ViewModels     5   	     � Title$  Tạo đề thi mới��5   	   $	��700��6    $	��900��6-    � WindowStartupLocation$ CenterScreen=�69   .��5      � $��10q�6   .+�5   
   �� .+�5      �� $��Tạo đề thi mới��6   $��26(�6.   $
��Bold"�6<   $��Center=�6N   $��0,0,0,20q�6k   �� 5
       �� Orientation$ 
Horizontal=�6   $��0,0,0,10q�62   .+�5      �� $��
Ngày thi:��6   $	��100��6,   $��Center=�68   (  "System.Windows.Controls.DatePicker  5      $	��200��6d      SelectedDate 6   �� *
ExamDate+ �� UpdateSourceTrigger$ PropertyChanged=�6d   �� 5      $��Phòng thi:��6   $	��100��6-   $��Center=�69   $��20,0,0,0q�6T   �� 5      $	��200��6U   ��6   �� *Room+$ PropertyChanged=�6U   5      �� 5      $ 
Horizontal=�6   $��0,0,0,10q�62   .+�5      �� $��Khóa học:��6   $	��100��6,   $��Center=�68   �� 5       �� DisplayMemberPath$ 
CourseName��5      $	��200��5      ��5      �� *	Courses+ �� SelectedItem 5      �� *SelectedCourse+5      5      �� 5      $��Câu hỏi:��6   $��20(�6(   $
��Bold"�66   $��	0,20,0,10q�6H   �� 5      $
��Auto=�6   $	��350��6>   .��5      �� ��6   �� *	Questions+��5      �� 5      .R�5      �� 
�������$��1q�68    �� CornerRadius$ 5��6L   $��0,0,0,10q�6]    �� Padding$ 10q�6o   ._�5   "   �� .+�5    &   �� $��Nội dung câu hỏi:��60   $
��Bold"�6I   �� 5!   &   $��0,5,0,5q�6l   $��60��6}   	 �� TextWrapping$
	 Wrap=�6�   ��6.   �� *	Content+$ PropertyChanged=�6�   �� 5"   &   $ 
Horizontal=�61   $��0,5,0,5q�6J   .+�5#   *   �� $��Hình ảnh:��60   $	��100��6D   $��Center=�6P   �� 5$   *   $	��100��6M   $	��150��6Z   $��Uniform=�6f   $��5q�6x   ��60   �� *	ImagePath+6x   �� 5%   *   $��Chọn ảnh��61   $��10,0,0,0q�5'   N   $	��100��6`   ��5&   1   �� * DataContext.SelectImageCommand+
 �� RelativeSource
 ��  �� AncestorType
 Window��5'   1   �� 6`   5(   '   �� 5)   &   $��
Đáp án:��60   $
��Bold"�6?   $��0,10,0,5q�6Q   �� 5*   &   ��63   �� *	Options+��5+   *   �� 5,   .   .R�5-   2   �� $ 
Horizontal=�6=   $��0,0,0,5q�6V   .+�5.   6   �� $	��300��6   $��0,0,10,0q�6�   ��6>   �� *
OptionText+$ PropertyChanged=�6�   �� 5/   6   $��Đáp án đúng��6�   $��Center=�6�    d� 	IsChecked 6?   �� *	IsCorrect+$ PropertyChanged=�6�   50   3   51   /   53   '   �� 54   &   $��Thêm đáp án��6-   $	��120��56   J   $��0,5,0,0q�6V   ��55   -   �� *DataContext.AddOptionCommand+
 �� 
 Window��56   -   �� 6V   57   #   58      59      5;      5<      �� 5=      $ 
Horizontal=�6   $��Center=�62   $��0,10,0,0q�6O   .+�5>      �� $��Thêm câu hỏi��6   $	��120��6W   $��0,0,10,0q�6c   ��60   �� *AddQuestionCommand+6c   �� 5?      $��Lưu đề thi��6   $	��120��6R   ��6.   �� *SaveExamCommand+6R   5@      5A      5B      5C      