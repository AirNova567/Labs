namespace Lab03;

public class DoctorManager
{
    private const int MaxDoctors = 50;
    private readonly Doctor[] _doctors = new Doctor[MaxDoctors];
    private int _count = 0;

    public int Count => _count;
    
    public void Add(Doctor doctor)
    {
        if (_count >= MaxDoctors)
        {
            Console.WriteLine("Помилка: досягнуто ліміту лікарів (50).");
            return;
        }

        _doctors[_count++] = doctor;
        Console.WriteLine($"Лікаря [{doctor.Id}] {doctor.FullName} додано.");
    }
    
    public Doctor? FindById(int id)
    {
        for (int i = 0; i < _count; i++)
        {
            if (_doctors[i].Id == id)
            {
                return _doctors[i];
            }
        }

        return null;
    }
    
    public Doctor[] FindBySpeciality(string speciality)
    {
        string lowerQuery = speciality.ToLower();
        int matchesCount = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_doctors[i].Speciality.ToLower().Contains(lowerQuery))
            {
                matchesCount++;
            }
        }

        Doctor[] result = new Doctor[matchesCount];
        int index = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_doctors[i].Speciality.ToLower().Contains(lowerQuery))
            {
                result[index++] = _doctors[i];
            }
        }

        return result;
    }
    
    public Doctor[] GetAll()
    {
        Doctor[] copy = new Doctor[_count];
        for (int i = 0; i < _count; i++)
        {
            copy[i] = _doctors[i];
        }
        return copy;
    }

  
    public bool Remove(int id)
    {
        int targetIndex = -1;
        for (int i = 0; i < _count; i++)
        {
            if (_doctors[i].Id == id)
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
            _doctors[i] = _doctors[i + 1];
        }

        _doctors[_count - 1] = null!;
        _count--;
        return true;
    }

   
    public void DisplayAll()
    {
        if (_count == 0)
        {
            Console.WriteLine("Список лікарів порожній.");
            return;
        }

        Console.WriteLine($"=== Лікарі ({_count} / {MaxDoctors}) ===");
        for (int i = 0; i < _count; i++)
        {
            Console.WriteLine(_doctors[i]);
        }
        Console.WriteLine(new string('-', 70));
    }

   
    public void DisplayStats()
    {
        if (_count == 0)
        {
            Console.WriteLine("Статистика недоступна: список порожній.");
            return;
        }

        int availableCount = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_doctors[i].IsAvailableNow)
            {
                availableCount++;
            }
        }

        Console.WriteLine("=== Статистика лікарів ===");
        Console.WriteLine($"Всього:        {_count}");
        Console.WriteLine($"Доступні зараз: {availableCount}");
        Console.WriteLine("По спеціальностях:");

       
        for (int i = 0; i < _count; i++)
        {
            string currentSpec = _doctors[i].Speciality;
            bool isAlreadyProcessed = false;

           
            for (int j = 0; j < i; j++)
            {
                if (_doctors[j].Speciality.Equals(currentSpec, StringComparison.OrdinalIgnoreCase))
                {
                    isAlreadyProcessed = true;
                    break;
                }
            }

            if (isAlreadyProcessed)
            {
                continue;
            }
            
            int specCount = 0;
            for (int k = 0; k < _count; k++)
            {
                if (_doctors[k].Speciality.Equals(currentSpec, StringComparison.OrdinalIgnoreCase))
                {
                    specCount++;
                }
            }

            Console.WriteLine($"  {currentSpec}: {specCount}");
        }

        Console.WriteLine("============================");
    }
}