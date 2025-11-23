using CetTodoApp.Data;

namespace CetTodoApp;

public partial class MainPage : ContentPage
{
   

    public MainPage()
    {
        InitializeComponent();
        FakeDb.AddToDo("Test1" ,DateTime.Now.AddDays(-1));
        FakeDb.AddToDo("Test2" ,DateTime.Now.AddDays(1));
        FakeDb.AddToDo("Test3" ,DateTime.Now);
        RefreshListView();
        ;


    }


    private void AddButton_OnClicked(object? sender, EventArgs e)
    {
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
        var item = e.SelectedItem as TodoItem;
       FakeDb.ChageCompletionStatus(item);
       RefreshListView();
       
    }
}