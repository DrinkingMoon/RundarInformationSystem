using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace GlobalObject
{
    /// <summary>
    /// 对对象提供克隆
    /// </summary>
    public class CloneObject
    {
        /// <summary>
        /// 深度克隆(采用序列化方式效率低, 需要被克隆对象标记为[Serializable])
        /// </summary>
        /// <typeparam name="T">要克隆对象的数据类型</typeparam>
        /// <param name="surObject">源数据</param>
        /// <returns>克隆后的数据</returns>
        static public T DeepClone<T>(object surObject)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();

            bf.Serialize(ms, surObject);
            ms.Seek(0, SeekOrigin.Begin);

            return (T)bf.Deserialize(ms);
        }

        /// <summary>
        /// Clone the object, and returning a reference to a cloned object.
        /// </summary>
        /// <param name="surObject">Clone the object</param>
        /// <returns>Reference to the new cloned object.</returns>
        static public T LowClone<T>(object surObject)
        {
            //First we create an instance of this specific type.
            object newObject = Activator.CreateInstance(surObject.GetType());

            //We get the array of fields for the new type instance.
            System.Reflection.FieldInfo[] fields = newObject.GetType().GetFields();

            int i = 0;

            foreach (System.Reflection.FieldInfo fi in surObject.GetType().GetFields())
            {
                //We query if the fiels support the ICloneable interface.
                Type ICloneType = fi.FieldType.GetInterface("ICloneable", true);

                if (ICloneType != null)
                {
                    //Getting the ICloneable interface from the object.
                    ICloneable IClone = (ICloneable)fi.GetValue(surObject);

                    //We use the clone method to set the new value to the field.
                    fields[i].SetValue(newObject, IClone.Clone());
                }
                else
                {
                    // If the field doesn't support the ICloneable 
                    // interface then just set it.
                    fields[i].SetValue(newObject, fi.GetValue(surObject));
                }

                //Now we check if the object support the 
                //IEnumerable interface, so if it does
                //we need to enumerate all its items and check if 
                //they support the ICloneable interface.
                Type IEnumerableType = fi.FieldType.GetInterface("IEnumerable", true);

                if (IEnumerableType != null)
                {
                    //Get the IEnumerable interface from the field.
                    System.Collections.IEnumerable IEnum = (System.Collections.IEnumerable)fi.GetValue(surObject);

                    //This version support the IList and the 
                    //IDictionary interfaces to iterate on collections.
                    Type IListType = fields[i].FieldType.GetInterface("IList", true);
                    Type IDicType = fields[i].FieldType.GetInterface("IDictionary", true);

                    int j = 0;

                    if (IListType != null)
                    {
                        //Getting the IList interface.
                        System.Collections.IList list = (System.Collections.IList)fields[i].GetValue(newObject);

                        foreach (object obj in IEnum)
                        {
                            //Checking to see if the current item 
                            //support the ICloneable interface.
                            ICloneType = obj.GetType().GetInterface("ICloneable", true);

                            if (ICloneType != null)
                            {
                                //If it does support the ICloneable interface, 
                                //we use it to set the clone of
                                //the object in the list.
                                ICloneable clone = (ICloneable)obj;

                                list[j] = clone.Clone();
                            }

                            //NOTE: If the item in the list is not 
                            //support the ICloneable interface then in the 
                            //cloned list this item will be the same 
                            //item as in the original list
                            //(as long as this type is a reference type).

                            j++;
                        }
                    }
                    else if (IDicType != null)
                    {
                        //Getting the dictionary interface.
                        System.Collections.IDictionary dic = (System.Collections.IDictionary)fields[i].GetValue(newObject);
                        j = 0;

                        foreach (System.Collections.DictionaryEntry de in IEnum)
                        {
                            //Checking to see if the item 
                            //support the ICloneable interface.
                            ICloneType = de.Value.GetType().GetInterface("ICloneable", true);

                            if (ICloneType != null)
                            {
                                ICloneable clone = (ICloneable)de.Value;

                                dic[de.Key] = clone.Clone();
                            }
                            j++;
                        }
                    }
                }
                i++;
            }

            return (T)newObject;
        }

        /// <summary>
        /// Clone the object, and returning a reference to a cloned object.
        /// </summary>
        /// <param name="surObject">Clone the object</param>
        /// <returns>Reference to the new cloned object.</returns>
        static public T CloneProperties<T>(object surObject)
        {
            //First we create an instance of this specific type.
            object newObject = Activator.CreateInstance(surObject.GetType());

            //We get the array of fields for the new type instance.
            System.Reflection.PropertyInfo[] fields = newObject.GetType().GetProperties();

            int i = 0;

            foreach (System.Reflection.PropertyInfo fi in surObject.GetType().GetProperties())
            {
                //We query if the fiels support the ICloneable interface.
                Type ICloneType = fi.PropertyType.GetInterface("ICloneable", true);

                if (ICloneType != null)
                {
                    //Getting the ICloneable interface from the object.
                    ICloneable IClone = (ICloneable)fi.GetValue(surObject, null);

                    //We use the clone method to set the new value to the field.
                    if (IClone != null)
                        fields[i].SetValue(newObject, IClone.Clone(), null);
                }
                else
                {
                    // If the field doesn't support the ICloneable 
                    // interface then just set it.
                    fields[i].SetValue(newObject, fi.GetValue(surObject, null), null);
                }

                //Now we check if the object support the 
                //IEnumerable interface, so if it does
                //we need to enumerate all its items and check if 
                //they support the ICloneable interface.
                Type IEnumerableType = fi.PropertyType.GetInterface("IEnumerable", true);

                if (IEnumerableType != null)
                {
                    //Get the IEnumerable interface from the field.
                    System.Collections.IEnumerable IEnum = (System.Collections.IEnumerable)fi.GetValue(surObject, null);

                    //This version support the IList and the 
                    //IDictionary interfaces to iterate on collections.
                    Type IListType = fields[i].PropertyType.GetInterface("IList", true);
                    Type IDicType = fields[i].PropertyType.GetInterface("IDictionary", true);

                    int j = 0;

                    if (IListType != null)
                    {
                        //Getting the IList interface.
                        System.Collections.IList list = (System.Collections.IList)fields[i].GetValue(newObject, null);

                        foreach (object obj in IEnum)
                        {
                            //Checking to see if the current item 
                            //support the ICloneable interface.
                            ICloneType = obj.GetType().GetInterface("ICloneable", true);

                            if (ICloneType != null)
                            {
                                //If it does support the ICloneable interface, 
                                //we use it to set the clone of
                                //the object in the list.
                                ICloneable clone = (ICloneable)obj;

                                if (clone != null)
                                    list[j] = clone.Clone();
                            }

                            //NOTE: If the item in the list is not 
                            //support the ICloneable interface then in the 
                            //cloned list this item will be the same 
                            //item as in the original list
                            //(as long as this type is a reference type).

                            j++;
                        }
                    }
                    else if (IDicType != null)
                    {
                        //Getting the dictionary interface.
                        System.Collections.IDictionary dic = (System.Collections.IDictionary)fields[i].GetValue(newObject, null);
                        j = 0;

                        foreach (System.Collections.DictionaryEntry de in IEnum)
                        {
                            //Checking to see if the item 
                            //support the ICloneable interface.
                            ICloneType = de.Value.GetType().GetInterface("ICloneable", true);

                            if (ICloneType != null)
                            {
                                ICloneable clone = (ICloneable)de.Value;

                                if (clone != null)
                                    dic[de.Key] = clone.Clone();
                            }
                            j++;
                        }
                    }
                }
                i++;
            }

            return (T)newObject;
        }
    }
}
