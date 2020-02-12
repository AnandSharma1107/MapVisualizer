using System;
using System.Xml;

public class BaseOsm 
{
    public T GetAttribute<T>(string attrName, XmlAttributeCollection attributes)
    {
        string strValue = attributes[attrName].Value;
        return (T)Convert.ChangeType(strValue, typeof(T));
    }

}
