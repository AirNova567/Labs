namespace Lab03;

public class GrowablePatientManager
{
    private Patient[] _patients = new Patient[4];
    private int _count = 0;

    public int Count => _count;
    
    public int Capacity => _patients.Length;
    
    private void Grow()
    {
        int oldCapacity = _patients.Length;
        int newCapacity = oldCapacity * 2;
        Patient[] newArray = new Patient[newCapacity];
        
        for (int i = 0; i < _count; i++)
        {
            newArray[i] = _patients[i];
        }

        _patients = newArray;
        Console.WriteLine($"Масив заповнений! Розширення: {oldCapacity} -> {newCapacity}");
    }
    
    public void Add(Patient patient)
    {
        if (_count == _patients.Length)
        {
            Grow();
        }

        _patients[_count++] = patient;
        Console.WriteLine($"Додано [{patient.Id}]. Розмір: {_count} / {Capacity}");
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

        Console.WriteLine($"=== Пацієнти ({_count} / {Capacity}) ===");
        for (int i = 0; i < _count; i++)
        {
            Console.WriteLine(_patients[i]);
        }
    }
}