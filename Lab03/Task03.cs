namespace Lab03;

public class PatientManager
{
    private const int MaxPatients = 100;
    private readonly Patient[] _patients = new Patient[MaxPatients];
    private int _count = 0;

    public int Count => _count;

    public void Add(Patient patient)
    {
        if (_count >= MaxPatients)
        {
            Console.WriteLine("Помилка: досягнуто ліміту пацієнтів (100).");
            return;
        }

        _patients[_count++] = patient;
        Console.WriteLine($"Пацієнта [{patient.Id}] {patient.FullName} додано.");
    }
    
    

    public Patient? FindById(int id)
    {
        for (int i = 0; i < _count; i++)
        {
            if (_patients[i].Id == id)
            {
                return _patients[i];
            }
        }

        return null;
    }
    
    

    public Patient[] FindByName(string query)
    {
        string lowerQuery = query.ToLower();
        int matchesCount = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_patients[i].FirstName.ToLower().Contains(lowerQuery) ||
                _patients[i].LastName.ToLower().Contains(lowerQuery))
            {
                matchesCount++;
            }
        }
        
        Patient[] result = new Patient[matchesCount];
        int index = 0;
        
        
        for (int i = 0; i < _count; i++)
        {
            if (_patients[i].FirstName.ToLower().Contains(lowerQuery) ||
                _patients[i].LastName.ToLower().Contains(lowerQuery))
            {
                result[index++] = _patients[i];
            }
        }

        return result;
    }
    
    public bool Remove(int id)
    {
        int targetIndex = -1;
        for (int i = 0; i < _count; i++)
        {
            if (_patients[i].Id == id)
            {
                targetIndex = i;
                break;
            }
        }

        if (targetIndex == -1)
        {
            return false;
        }
        
        for (int i = targetIndex; i < _count - 1; i++)
        {
            _patients[i] = _patients[i + 1];
        }

        _patients[_count - 1] = null!; 
        _count--;
        return true;
    }
    
    public void DisplayAll()
    {
        if (_count == 0)
        {
            Console.WriteLine("Список пацієнтів порожній.");
            return;
        }

        Console.WriteLine($"=== Пацієнти ({_count} / {MaxPatients}) ===");
        for (int i = 0; i < _count; i++)
        {
            Console.WriteLine(_patients[i]);
        }
        Console.WriteLine(new string('-', 50));
    }

   
    public void DisplayStats()
    {
        if (_count == 0)
        {
            Console.WriteLine("Статистика недоступна: список порожній.");
            return;
        }

        int sumAges = 0;
        int youngestIndex = 0;
        int oldestIndex = 0;
        int adultCount = 0;

        for (int i = 0; i < _count; i++)
        {
            int age = _patients[i].Age;
            sumAges += age;

            if (age < _patients[youngestIndex].Age)
            {
                youngestIndex = i;
            }

            if (age > _patients[oldestIndex].Age)
            {
                oldestIndex = i;
            }

            if (_patients[i].IsAdult)
            {
                adultCount++;
            }
        }

       
        double averageAge = (double)sumAges / _count;

        Console.WriteLine("=== Статистика пацієнтів ===");
        Console.WriteLine($"Всього:        {_count}");
        Console.WriteLine($"Середній вік:  {averageAge:F1} р.");
        Console.WriteLine($"Наймолодший:   {_patients[youngestIndex].FullName} ({_patients[youngestIndex].Age} р.)");
        Console.WriteLine($"Найстарший:    {_patients[oldestIndex].FullName} ({_patients[oldestIndex].Age} р.)");
        Console.WriteLine($"Дорослих:      {adultCount} з {_count}");
        Console.WriteLine("============================");
    }
    
}
