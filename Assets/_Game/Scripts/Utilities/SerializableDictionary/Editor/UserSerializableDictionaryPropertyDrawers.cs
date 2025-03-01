using UnityEditor;

[CustomPropertyDrawer(typeof(EnemyPrefabDictionary))]
[CustomPropertyDrawer(typeof(StatDictionary))]

public class AnySerializableDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer {}