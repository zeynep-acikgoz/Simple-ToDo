using CetTodoApp.Data;
using System.Globalization;

namespace CetTodoApp;

public partial class MainPage : ContentPage
{
   

    public MainPage()
    {
        InitializeComponent();
        FakeDb.AddToDo("Test1" ,DateTime.Now.AddDays(-1));
        FakeDb.AddToDo("Test2" ,DateTime.Now.AddDays(1));
        FakeDb.AddToDo("Test3" ,DateTime.Now);
        FakeDb.AddToDo("CET301 Calculator App" ,DateTime.Now.AddDays(-2));
        FakeDb.AddToDo("CET301 To-Do App" ,DateTime.Now.AddDays(2));
        RefreshListView();
        ;


    }


    private async void AddButton_OnClicked(object? sender, EventArgs e)
    {
     
        if (string.IsNullOrWhiteSpace(Title.Text))
        {
            await DisplayAlert("ERROR", "Task title cannot be empty. Please enter a title.", "OK");
            return; 
        }

        
        if (DueDate.Date < DateTime.Today)
        {
            await DisplayAlert("ERROR", "You cannot create a task with a past date. Please choose a future date.", "OK");
            return;
        }
        
        FakeDb.AddToDo(Title.Text, DueDate.Date);
        Title.Text = string.Empty;
        DueDate.Date=DateTime.Now;
        RefreshListView();
    }

    private void RefreshListView()
    {
        TasksListView.ItemsSource = null;
        TasksListView.ItemsSource = FakeDb.Data.Where(x => !x.IsComplete ||
                                                           (x.IsComplete && x.DueDate > DateTime.Now.AddDays(-1)))
            .ToList();
    }

    private void TasksListView_OnItemSelected(object? sender, SelectedItemChangedEventArgs e)
    {
        
        if (e.SelectedItem == null) return;
        
        var item = e.SelectedItem as TodoItem;
        FakeDb.ChageCompletionStatus(item);
        
        TasksListView.SelectedItem = null; 
        
        RefreshListView();
       
    }
    
    
}

public class OverdueColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is DateTime date)
        {
            
            if (date.Date < DateTime.Today)
                return Colors.Red;
        }
        return Colors.White; 
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}