using System.Reflection;

namespace DevExpressXAFTagBoxHelper.Blazor.Server.Editors.TagBoxEditorHelper
{
    public class ISTagBoxEditorDataItem<T> where T : class
    {
        public ISTagBoxEditorDataItem()
        {
        }

        public ISTagBoxEditorDataItem(T key, string value, string displayText)
        {
            Value = value;
            DisplayText = displayText;
            Key = key;
        }

        private PropertyInfo GetKeyProperty(Type type)
        {
            var locProperty = type.GetProperties()
                .Where(locProperty => Attribute.IsDefined(locProperty, typeof(DevExpress.Xpo.KeyAttribute)));

            if(locProperty != null && locProperty?.Count() == 1)
            {
                return locProperty.Single();
            } else
            {
                throw new Exception("Key attribute is missing");
            }
        }

        /// <summary>
        /// Trying to convert some given object to TagBoxEditorDataItem The XPO.Key Attribute must be implemented in the
        /// given object The XPO.Key data is taken for setting the specific value
        /// </summary>
        /// <param name="data">A class, which contains XPO.KeyAttribute to define value</param>
        /// <param name="displayText">The text, which gets displayed in the TagBox</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public ISTagBoxEditorDataItem<T> TryConvertObjetToDataItem(object data, string displayText)
        {
            try
            {
                var locKeyProperty = GetKeyProperty(data.GetType());
                var locGetType = data.GetType();
                var locIdValue = locGetType.GetProperty(locKeyProperty.Name).GetValue(data, null);
                var locId = (T)Convert.ChangeType(locIdValue, typeof(T));
                return new ISTagBoxEditorDataItem<T>(locId, $"{locGetType.FullName}({locIdValue})", $"{displayText}");
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        /// <summary>
        /// Trying to convert some given object to TagBoxEditorDataItem the parameter propertyName is used as Key
        /// </summary>
        /// <param name="data">Should be a class</param>
        /// <param name="propertyName">The property which is used as key</param>
        /// <param name="displayText">The text, which gets displayed in the TagBox</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">raised, when given propertyName does not exist</exception>
        /// <exception cref="Exception"></exception>
        public ISTagBoxEditorDataItem<T> TryConvertObjetToDataItem(object data, string propertyName, string displayText)
        {
            try
            {
                var locGetType = data.GetType();
                if(locGetType.GetProperty(propertyName) == null)
                {
                    throw new ArgumentNullException(propertyName);
                }
                var locIdValue = locGetType.GetProperty(propertyName).GetValue(data, null);
                var locId = (T)Convert.ChangeType(locIdValue, typeof(T));
                return new ISTagBoxEditorDataItem<T>(locId, $"{locGetType.FullName}({locIdValue})", $"{displayText}");
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string DisplayText { get; }

        public T Key { get; }

        public string Value { get; }
    }
}
