# BinarySerializer
A very simple and fast binary serializer that uses expressions to access data. The serializer remembers the data scheme of the objects during the first scan. Both full serialization and div compression are possible.
## Tutorial

### Declare Classes
All fields that need to be serialized must be marked with the BinaryItem attribute.
```csharp

public class Location
{
    public ByteBinaryObjectCollection<Character> Characters => _characters;
    public IntBinaryObjectCollection<Structure> Structures => _structures;

    [BinaryItem(0)]
    private readonly ByteBinaryObjectCollection<Character> _characters = new ByteBinaryObjectCollection<Character>();

    [BinaryItem(1)]
    private readonly IntBinaryObjectCollection<Structure> _structures = new IntBinaryObjectCollection<Structure>();
}

public class Character
{
    public string Name
    {
        get => _name;
        set => _name = value;
    }

    public byte Age
    {
        get => _age;
        set => _age = value;
    }

    public int Score
    {
        get => _score;
        set => _score = value;
    }

    [BinaryItem(0)] private string _name;
    [BinaryItem(1)] private byte _age;
    [BinaryItem(2)] private int _score;

    public bool Equals(Character obj)
    {
        return _name == obj._name &&
               _age == obj._age &&
               _score == obj._score;
    }
}


public class Structure
{
    public uint Id => _id;

    public float PosX
    {
        get => _posX;
        set => _posX = value;
    }

    public float PosY
    {
        get => _posY;
        set => _posY = value;
    }

    public Structure()
    {

    }

    public Structure(uint id)
    {
        _id = id;
    }

    [BinaryItem(0)] private uint _id;
    [BinaryItem(1)] private float _posX;
    [BinaryItem(2)] private float _posY;

    public bool Equals(Structure obj)
    {
        return _id == obj._id &&
               Math.Abs(_posX - obj._posX) < 1e-6 &&
               Math.Abs(_posY - obj._posY) < 1e-6;
    }
}
```    
Create world

```csharp
Location source = new Location();

Character mike = new Character {Name = "Mike", Age = 18, Score = 180};
Character alice = new Character {Name = "Alice", Age = 23, Score = 120};
Character john = new Character {Name = "John", Age = 32, Score = 510};

Structure st0 = new Structure(0) {PosX = 34.4f, PosY = 1.0f};
Structure st1 = new Structure(1) {PosX = 25.2f, PosY = 36.0f};
Structure st2 = new Structure(2) {PosX = 45.7f, PosY = 67.3f};

source.Characters.Add(0, mike);
source.Characters.Add(1, alice);
source.Characters.Add(2, john);

source.Structures.Add(0, st0);
source.Structures.Add(1, st1);
source.Structures.Add(2, st2);
``` 
### Full serialization
```csharp
byte[] data = BinarySerializer.Serialize(source);
``` 

Create new world and deserialize
```csharp
Location target = new Location();
BinarySerializer.Deserialize(target, data);
``` 
Worlds match each other
```csharp
Assert.AreEqual(source.Characters.Count, target.Characters.Count);
Assert.IsTrue(target.Characters[0].Equals(mike));
Assert.IsTrue(target.Characters[1].Equals(alice));
Assert.IsTrue(target.Characters[2].Equals(john));

Assert.AreEqual(source.Structures.Count, target.Structures.Count);
Assert.IsTrue(target.Structures[0].Equals(st0));
Assert.IsTrue(target.Structures[1].Equals(st1));
Assert.IsTrue(target.Structures[2].Equals(st2));
``` 
### Partial serialization
Create baseline
```csharp
Baseline<byte> baseline = new Baseline<byte>();
``` 
Serialialize with baseline
```csharp
data = BinarySerializer.Serialize(source, baseline);
```
Change data

```csharp
mike.Score++;
``` 
Serialize relative to the baseline
```csharp
data = BinarySerializer.Serialize(source, baseline);
``` 
