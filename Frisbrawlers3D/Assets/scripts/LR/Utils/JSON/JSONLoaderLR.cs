using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class JSONData
{
    public string Id { get; protected set; }

    public JSONData() {}

    public JSONData(JSONObject _json)
    {
        BuildJSONData(_json);
    }

    public virtual void BuildJSONData(JSONObject _json)
    {
        var idObj = _json.GetField("id");
        if( idObj == null)
        {
            UnityEngine.Debug.Log("An id column is missing in your table");
        }

        var props = GetType().GetProperties();
        var dic = _json.ToDictionary();
        var dbFields = dic.Select(x => x.Key).ToList(); //list of fields of the table

        foreach ( var dbItem in dic)
        {
            //check value first
            if( dbItem.Value == null ||  string.IsNullOrEmpty( dbItem.Value.ToString()) )
            {
                continue;
            }

            //try getting the property corresponding to the db field
            var prop = props.FirstOrDefault(x=> x.Name.ToLower() == dbItem.Key.ToLower());
            if( prop != null)
            {
                //We'll convert a la mano (.NET 3.5)
                var valueStr = dbItem.Value.ToString();
                if (prop.PropertyType == typeof(string))
                {
                    prop.SetValue(this, valueStr , null);
                }
                else if(prop.PropertyType == typeof(int))
                {
                    int i = -1;
                    int.TryParse(valueStr, out i);
                    prop.SetValue(this, i, null);
                }else if(prop.PropertyType == typeof(float))
                {
                    float i = -1;
                    float.TryParse(valueStr, out i);
                    prop.SetValue(this, i, null);
                } else if (prop.PropertyType == typeof(double))
                {
                    double i = -1;
                    double.TryParse(valueStr, out i);
                    prop.SetValue(this, i, null);
                }
            }
        }                
    }
}

public interface IJSONDataCollection : IEnumerable
{
    void AddElement(JSONObject element);
}

/// <summary>
/// Implementation of a IJSONDataCollection, using a Dictionary indexed by a string ( Id ) as its collection 
/// </summary>
public class IJSONDataDicoCollection<T> : IJSONDataCollection where T : JSONData, new()
{
    protected Dictionary<string, T> items = new Dictionary<string, T>();

    public void AddElement(JSONObject _element) {         
        T t = new T();
        t.BuildJSONData(_element);
        items[t.Id] = t;
    }

    public IEnumerator GetEnumerator()
    {
        return items.GetEnumerator();
    }

    public T this[string i]
    {
        get
        {
            if (items.ContainsKey(i))
                return items[i];
            return default(T);
        }
    }

    public List<T> ToList()
    {
        return items.Values.ToList();
    }
}

/// <summary>
/// Implementation of a IJSONDataCollection, using a List as its collection 
/// </summary>
public class IJSONDataListCollection<T> : IJSONDataCollection where T : JSONData, new()
{
    protected List<T> items = new List<T>();

    public void AddElement(JSONObject element)
    {
        T t = new T();
        t.BuildJSONData(element);
        items.Add(t);
    }

    public IEnumerator GetEnumerator()
    {
        return items.GetEnumerator();
    }
    
    public T this[int i]
    {
        get
        {
            if (i < items.Count)
                return items[i];
            return default(T);
        }
    }

    public List<T> ToList()
    {
        return items.ToList();
    }
}

public class JSONLoaderLR {

    public static T LoadTable<T>(JSONObject _jsonObject) where T : IJSONDataCollection, new()
    {
        T t = new T();
        for (int i = 0; i < _jsonObject.Count; ++i)
        {
            var element = _jsonObject[i];
            t.AddElement(element);
        }
        return t;
    }
}
