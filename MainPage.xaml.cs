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
        RefreshListView();
        ;


    }


    private async void AddButton_OnClicked(object? sender, EventArgs e)
    {
        // --- VALIDATION ---
        
        // Boş veya sadece boşluktan oluşuyorsa başlık, hata ver.
        if (string.IsNullOrWhiteSpace(Title.Text))
        {
            await DisplayAlert("Hata", "Lütfen bir görev başlığı giriniz.", "Tamam");
            return; // Kodun devam etmesini engelle
        }

        // Seçilen tarih dünden önceyse uyarı versin.
        if (DueDate.Date < DateTime.Today)
        {
            await DisplayAlert("Hata", "Geçmişe dönük görev oluşturulamaz. Lütfen ileriye yönük bir tarih seçiniz.", "Tamam");
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
        // Seçim boşsa (unselect durumunda) işlem yapma
        if (e.SelectedItem == null) return;
        
        var item = e.SelectedItem as TodoItem;
        FakeDb.ChageCompletionStatus(item);
        
        TasksListView.SelectedItem = null; //arkaplan kalmasın diye
        
        RefreshListView();
       
    }
    
    
}// --- EKSTRA SINIF: RENK DÖNÜŞTÜRÜCÜ ---
// Bu sınıf XAML tarafında kullanılır. Tarihe bakar, eğer tarih geçmişse Kırmızı, değilse Siyah renk verir.
public class OverdueColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is DateTime date)
        {
            // Eğer tarih bugünden küçükse (geçmişse) Kırmızı döndür
            // DateTime.Today kullanıyoruz ki saat farkından dolayı bugün girilenler kırmızı olmasın.
            if (date.Date < DateTime.Today)
                return Colors.Red;
        }
        return Colors.Black; // Değilse varsayılan renk
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}