using System.Reflection;

namespace Schwer.Reflection {
    public static class ReflectionUtility {
        // Reference: https://stackoverflow.com/questions/12993962/set-value-of-private-field
        public static void SetPrivateField(object instance, string fieldName, object value) {
            var field = instance.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            field.SetValue(instance, value);
        }

        public static void SetPrivateProperty(object instance, string propertyName, object value) {
            var prop = instance.GetType().GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Instance);
            prop.SetValue(instance, value);
        }
    }
}
