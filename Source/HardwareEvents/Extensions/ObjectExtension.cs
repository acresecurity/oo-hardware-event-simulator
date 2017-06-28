
namespace System
{
    public static class ObjectExtension
    {
        /// <summary>
        /// 	Cast an object to the given type. Usefull especially for anonymous types.
        /// </summary>
        /// <typeparam name = "T">The type to cast to</typeparam>
        /// <param name = "value">The object to case</param>
        /// <returns>
        /// 	the casted type or null if casting is not possible.
        /// </returns>
        /// <remarks>
        /// 	Contributed by blaumeister, http://www.codeplex.com/site/users/view/blaumeiser
        /// </remarks>
        public static T CastTo<T>(this object value)
        {
            if (!(value is T))
                return default(T);

            return (T)value;
        }

        /// <summary>
        /// 	Determines whether the object is excactly of the passed type
        /// </summary>
        /// <param name = "obj">The object to check.</param>
        /// <param name = "type">The target type.</param>
        /// <returns>
        /// 	<c>true</c> if the object is of the specified type; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsOfType(this object obj, Type type)
        {
            return (obj.GetType() == type);
        }

        /// <summary>
        /// 	Determines whether the object is of the passed generic type or inherits from it.
        /// </summary>
        /// <typeparam name = "T">The target type.</typeparam>
        /// <param name = "obj">The object to check.</param>
        /// <returns>
        /// 	<c>true</c> if the object is of the specified type; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsOfTypeOrInherits<T>(this object obj)
        {
            return obj.IsOfTypeOrInherits(typeof(T));
        }

        /// <summary>
        /// 	Determines whether the object is of the passed type or inherits from it.
        /// </summary>
        /// <param name = "obj">The object to check.</param>
        /// <param name = "type">The target type.</param>
        /// <returns>
        /// 	<c>true</c> if the object is of the specified type; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsOfTypeOrInherits(this object obj, Type type)
        {
            var objectType = obj.GetType();

            do
            {
                if (objectType == type)
                    return true;
                if ((objectType == objectType.BaseType) || (objectType.BaseType == null))
                    return false;
                objectType = objectType.BaseType;
            } while (true);
        }

        /// <summary>
        /// 	Determines whether the object is assignable to the passed generic type.
        /// </summary>
        /// <typeparam name = "T">The target type.</typeparam>
        /// <param name = "obj">The object to check.</param>
        /// <returns>
        /// 	<c>true</c> if the object is assignable to the specified type; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAssignableTo<T>(this object obj)
        {
            return obj.IsAssignableTo(typeof(T));
        }

        /// <summary>
        /// 	Determines whether the object is assignable to the passed type.
        /// </summary>
        /// <param name = "obj">The object to check.</param>
        /// <param name = "type">The target type.</param>
        /// <returns>
        /// 	<c>true</c> if the object is assignable to the specified type; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAssignableTo(this object obj, Type type)
        {
            var objectType = obj.GetType();
            return type.IsAssignableFrom(objectType);
        }

        /// <summary>
        /// 	Gets the type default value for the underlying data type, in case of reference types: null
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <param name = "value">The value.</param>
        /// <returns>The default value</returns>
        public static T GetTypeDefaultValue<T>(this T value)
        {
            return default(T);
        }
    }
}
