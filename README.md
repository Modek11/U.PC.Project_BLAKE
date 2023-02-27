# Repo work organization

Mamy dwa główne branche 

main → stablina wersja

develop → wersja rozwojowa

Jeżeli zaczynacie prace, tworzycie swojego brancha używając nazwy taska z trello.

Po skończonej pracy robicie merge request do developa, ja wam robie code review/merge.

Prefixy!

feature/ ← dodajecie coś do projektu

bugfix/ ← naprawianie buga

hotfix/ ← naprawienie buga, merge do maina

Ważne!

Jak pracujecie jakiś czas i poszły nowe merge do developa, warto jest sobie tego developa domergować do swojego brancha.

Program do zarządania gitem, warto pobrać sobie jakiś problem a terminala używać jak się coś sypie. Ja używam SourceTree, używamy też w firmie, potężne i proste w użyciu narzędzie.

Polecam używać kluczy ssh do autoryzacji, bardzo przydatna rzecz, jak chcecie tutorial z tego dajcie znać.

# Konwencje programowania

# Stosujemy się do SOLID czyli czego unikać ?

- Jeżeli coś kopiujesz i wklejasz → robisz coś źle, albo trzeba zrobić metode/dziedziczenie
- Nie robimy klasy typu murarz, tynkarz, akrobata
- Magic number - użycie w kodzie stałych o niewyjaśnionym sensie i pochodzeniu. Przykładem
wystąpienia tego antywzorca jest użycie stałej, której nazwa nic nie mówi o jej przeznaczeniu, a
komentarze nie wyjaśniają sensu jej użycia. W efekcie tego działania posiadamy w kodzie
magiczną liczbę, której przeznaczenia nikt nie zna.
- Nie nadużywamy singeltonów

# C#

Camel Case 

- prywatne zmienne
- prywatne constanty
- parametry
- prywatne pola statyczne

Pascal Case

- Metody
- Właściwości
- Eventy

## Kolejność w klasach
```
class A 
{
    public enum B 
    {
        A, B, C
    }

    private struct C 
    {

        public int Get() 
        {
            return a;
        }

        private int a;
    }
    
    public static A Instance => instance;

    public event Action OnUpdate = null;

    [field: SerializeField]
    public int Width {get; private set;}    

    public int Height => height;

    private static A instance = null;

    [SerializeField]
    private int b;

    private int height;

    public A() 
    {
        instance = this;
    }

    private void Start()
    {
        DoSomething();
    }
    
    public int GetMagic() 
    {
        return 3;
    }

    internal void Why() {}
    protected void Test() {}
    private void Foo() {}
}

```
#Hubert/Papkin - Autor
