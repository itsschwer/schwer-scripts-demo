using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Schwer.Database {
    /// <summary>
    /// An abstract base class to inherit from to create a <c>ScriptableObject</c> that serves as a database for a specified type of <c>ScriptableObject</c>.
    /// </summary>
    /// <remarks>
    /// The first serializable field should be an array or a list of the specified type of <c>ScriptableObject</c>.
    /// </remarks>
    public abstract class ScriptableDatabase<T> : ScriptableObject where T : ScriptableObject {
        // [SerializeField] private T[] arbitraryName;

        /// <summary>
        /// Initialises the database. Intended to be used in-editor through <c cref="ScriptableDatabaseUtility.GenerateDatabase">ScriptableDatabaseUtility.GenerateDatabase</c>.
        /// </summary>
        /// <param name="elements">
        /// The elements that make up the database. Elements may be filtered before being applied.
        /// </param>
        public abstract void Initialise(T[] elements);

        /// <summary>
        /// Filters out elements with duplicate <c>id</c>s and sorts elements by <c>id</c>.
        /// </summary>
        /// <param name="elements">
        /// The elements to filter through and sort.
        /// </param>
        /// <returns>
        /// A filtered array of elements sorted by <c>id</c>.
        /// </returns>
        protected I[] FilterByID<I>(I[] elements) where I : ScriptableObject, IID {
            var filteredElements = new List<I>();
            var filteredIDs = new List<int>();

            for (int i = 0; i < elements.Length; i++) {
                if (filteredIDs.Contains(elements[i].id)) {
                    var sharedID = filteredElements[filteredIDs.IndexOf(elements[i].id)].name;
                    Debug.LogWarning($"'{elements[i].name}' was excluded from {this.name} because it shares its ID ({elements[i].id}) with '{sharedID}'.");
                }
                else {
                    filteredElements.Add(elements[i]);
                    filteredIDs.Add(elements[i].id);
                }
            }
            return filteredElements.OrderBy(i => i.id).ToArray();
        }

        /// <summary>
        /// Loops through the specified elements and tries to find one with a matching id.
        /// </summary>
        /// <remarks>
        /// Will log any null entries as a warning in the console.
        /// </remarks>
        protected I GetFromID<I>(int id, I[] elements) where I : ScriptableObject, IID {
            I result = null;
            foreach (var element in elements) {
                if (element == null) {
                    Debug.LogWarning($"{this.name} contains a null entry. Please regenerate the database to remove.");
                }
                else if (element.id == id) {
                    result = element;
                }
            }
            if (result == null) Debug.LogWarning($"{typeof(I).Name} with ID '{id}' was not found in {this.name}.");
            return result;
        }
    }

    /// <summary>
    /// An interface that should be inherited from to ensure that a class has an int <c>id</c>.
    /// </summary>
    public interface IID {
        int id { get; }
    }
}
