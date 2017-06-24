using UnityEngine;

namespace Arsonide
{
    public static class Extensions
    {
        public static T GetComponentInChildrenByName<T>(this GameObject parent, string componentName) where T : MonoBehaviour
        {
            foreach (T component in parent.GetComponentsInChildren<T>())
            {
                if (component.gameObject.name == componentName)
                    return component;
            }

            return null;
        }

        public static T GetComponentInChildrenByName<T>(this MonoBehaviour parent, string componentName) where T : MonoBehaviour
        {
            foreach (T component in parent.GetComponentsInChildren<T>())
            {
                if (component.gameObject.name == componentName)
                    return component;
            }

            return null;
        }

        public static T GetComponentUpwards<T>(this GameObject child)
        {
            Transform parent = child.transform.parent;

            while (parent != null)
            {
                T result = parent.GetComponent<T>();

                if (result != null)
                    return result;

                parent = parent.parent;
            }

            return default(T);
        }

        public static T GetComponentUpwards<T>(this MonoBehaviour child)
        {
            Transform parent = child.transform.parent;

            while (parent != null)
            {
                T result = parent.GetComponent<T>();

                if (result != null)
                    return result;

                parent = parent.parent;
            }

            return default(T);
        }

        public static float ToNearest(this float number, float interval)
        {
            return Mathf.Round(number / interval) * interval;
        }
    }
}
