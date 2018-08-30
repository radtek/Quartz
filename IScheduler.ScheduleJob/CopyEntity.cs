using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JosonEntity
{


    public static class ConvertionExtensions
    {


        //http://www.cnblogs.com/artech/archive/2011/03/17/NullableType.html

        /// <summary>
        /// ConvertTo
        /// </summary>
        //public static T? ConvertTo<T>(this IConvertible convertibleValue) where T : struct
        //{
        //    if (null == convertibleValue)
        //    {
        //        return null;
        //    }
        //    return (T?)Convert.ChangeType(convertibleValue, typeof(T));
        //}



        /// <summary>
        /// ConvertTo
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="convertibleValue"></param>
        /// <returns></returns>
        public static T ConvertTo<T>(this IConvertible convertibleValue)
        {
            if (null == convertibleValue)
            {
                return default(T);
            }

            if (!typeof(T).IsGenericType)
            {
                return (T)Convert.ChangeType(convertibleValue, typeof(T));
            }
            else
            {
                Type genericTypeDefinition = typeof(T).GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(Nullable<>))
                {
                    return (T)Convert.ChangeType(convertibleValue, Nullable.GetUnderlyingType(typeof(T)));
                }
            }

            throw new InvalidCastException(string.Format("Invalid cast from type \"{0}\" to type \"{1}\".", convertibleValue.GetType().FullName, typeof(T).FullName));
        }

    }


    public class Entity
    {
 
        /// <summary>
        /// 不同类型的实体拷贝 
        /// JosonEntity.Entity EntityJoson = new JosonEntity.Entity();
        /// NotifyRecHis = EntityJoson.CopyFromEntity<BPMProcNotifyRec, BPMProcNotifyRecHis>(NotifyRec);
        /// </summary>
        /// <typeparam name="Joson">被拷贝对象</typeparam>
        /// <typeparam name="T">拷贝对象</typeparam>
        /// <param name="Josons">被拷贝实体</param>
        /// <returns>返回实体</returns>
        public T CopyFromEntity<Joson, T>(Joson Josons) where T : class, new()
        {
            T model = new T();

            Type type = typeof(T);
            PropertyInfo[] ps = type.GetProperties();

            Type Type = typeof(Joson);
            PropertyInfo[] Ps = type.GetProperties();

            foreach (PropertyInfo p in ps)
            {
                Type oPropertyType = p.PropertyType;
                Object oPropValue = null;
                String myKey = p.Name.ToLower();

                foreach (PropertyInfo P in Ps)
                {
                    String sPropName = P.Name.ToLower();

                    if (sPropName == myKey)
                    {
                        PropertyInfo propertyInfo = Type.GetProperty(p.Name);

                        Object convertibleValue = propertyInfo.GetValue(Josons);

                        if (P.PropertyType.IsGenericType)
                        {
                            Type genericTypeDefinition = P.PropertyType.GetGenericTypeDefinition();

                            #region 泛型Nullable<>

                            if (genericTypeDefinition == typeof(Nullable<>))
                            {
                                var propValue = Convert.ToString(propertyInfo.GetValue(Josons, null));

                                oPropValue = string.IsNullOrEmpty(propValue)
                                         ? null
                                         : Convert.ChangeType(propValue, Nullable.GetUnderlyingType(P.PropertyType));

                                oPropertyType = string.IsNullOrEmpty(propValue) ? P.PropertyType : Nullable.GetUnderlyingType(P.PropertyType);
                            }
 
                            #endregion

                            //oPropValue = Convert.ChangeType(convertibleValue, oPropertyType);
                        }
                        else
                        {
                            oPropValue = Convert.ChangeType(convertibleValue, P.PropertyType);
                        }

                        break;
                    }

                }

                type.GetProperty(p.Name).SetValue(model, Convert.ChangeType(oPropValue, oPropertyType), null);

            }

            return model;
        }

    }
}
