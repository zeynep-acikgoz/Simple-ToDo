using CetTodoApp.Data;
using CetTodoApp.Models; 
using System.Globalization;

namespace CetTodoApp;

public partial class MainPage : ContentPage
{
   
    TodoItemDatabase _database;
    
    public MainPage(TodoItemDatabase database)
    {
        InitializeComponent();
        _database = database;
    }

    
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await RefreshListView();
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
        
       
        var newItem = new TodoItem
        {
            Title = Title.Text,
            DueDate = DueDate.Date,
            CreatedDate = DateTime.Now,
            IsComplete = false
        };

        await _database.SaveItemAsync(newItem);
        
        Title.Text = string.Empty;
        DueDate.Date = DateTime.Now;
      
        await RefreshListView();
    }

    
    private async Task RefreshListView()
    {
        var allItems = await _database.GetItemsAsync();
    
        var sortedItems = allItems
            
            .Where(x => !x.IsComplete || (x.IsComplete && x.DueDate > DateTime.Now.AddDays(-1)))
            
            .OrderBy(x => x.DueDate)
        
            .ToList();

       
        TasksListView.ItemsSource = null;
        TasksListView.ItemsSource = sortedItems;
    }

    private async void TasksListView_OnItemSelected(object? sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem == null) return;
        
        var item = e.SelectedItem as TodoItem;
        
        // YENİ KISIM: Durumu tersine çevir ve veritabanına kaydet
        if (item != null)
        {
            item.IsComplete = !item.IsComplete; // True ise False, False ise True yap
            await _database.SaveItemAsync(item); // Güncellemeyi kaydet
        }
        
        TasksListView.SelectedItem = null; 
        await RefreshListView();
    }
}

// Senin Converter sınıfın aynen kalıyor
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