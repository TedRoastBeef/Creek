using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Creek.UI.EFML.Base
{
    public class ProcessorCollection : IDictionary<string, ElementProcessor>
    {
        private readonly Dictionary<string, ElementProcessor> processors = new Dictionary<string, ElementProcessor>();

        #region Implementation of IEnumerable

        /// <summary>
        /// Gibt einen Enumerator zurück, der die Auflistung durchläuft.
        /// </summary>
        /// <returns>
        /// Ein <see cref="T:System.Collections.Generic.IEnumerator`1"/>, der zum Durchlaufen der Auflistung verwendet werden kann.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<KeyValuePair<string, ElementProcessor>> GetEnumerator()
        {
            return processors.GetEnumerator();
        }

        /// <summary>
        /// Gibt einen Enumerator zurück, der eine Auflistung durchläuft.
        /// </summary>
        /// <returns>
        /// Ein <see cref="T:System.Collections.IEnumerator"/>-Objekt, das zum Durchlaufen der Auflistung verwendet werden kann.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Implementation of ICollection<KeyContentPair<string,ElementProcessor>>

        /// <summary>
        /// Fügt der <see cref="T:System.Collections.Generic.ICollection`1"/> ein Element hinzu.
        /// </summary>
        /// <param name="item">Das Objekt, das <see cref="T:System.Collections.Generic.ICollection`1"/> hinzugefügt werden soll.</param><exception cref="T:System.NotSupportedException"><see cref="T:System.Collections.Generic.ICollection`1"/> ist schreibgeschützt.</exception>
        public void Add(KeyValuePair<string, ElementProcessor> item)
        {
            processors.Add(item.Key, item.Value);
        }

        /// <summary>
        /// Entfernt alle Elemente aus <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException"><see cref="T:System.Collections.Generic.ICollection`1"/> ist schreibgeschützt. </exception>
        public void Clear()
        {
            processors.Clear();
        }

        /// <summary>
        /// Bestimmt, ob <see cref="T:System.Collections.Generic.ICollection`1"/> einen bestimmten Wert enthält.
        /// </summary>
        /// <returns>
        /// true, wenn sich <paramref name="item"/> in <see cref="T:System.Collections.Generic.ICollection`1"/> befindet, andernfalls false.
        /// </returns>
        /// <param name="item">Das im <see cref="T:System.Collections.Generic.ICollection`1"/> zu suchende Objekt.</param>
        public bool Contains(KeyValuePair<string, ElementProcessor> item)
        {
            return processors.Contains(item);
        }

        /// <summary>
        /// Kopiert die Elemente von <see cref="T:System.Collections.Generic.ICollection`1"/> in ein <see cref="T:System.Array"/>, beginnend bei einem bestimmten <see cref="T:System.Array"/>-Index.
        /// </summary>
        /// <param name="array">Das eindimensionale <see cref="T:System.Array"/>, das das Ziel der aus <see cref="T:System.Collections.Generic.ICollection`1"/> kopierten Elemente ist.Für <see cref="T:System.Array"/> muss eine nullbasierte Indizierung verwendet werden.</param><param name="arrayIndex">Der nullbasierte Index in <paramref name="array"/>, an dem das Kopieren beginnt.</param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> hat den Wert null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> ist kleiner als 0.</exception><exception cref="T:System.ArgumentException"><paramref name="array"/> ist mehrdimensional.- oder -Die Anzahl der Elemente in der Quelle <see cref="T:System.Collections.Generic.ICollection`1"/> ist größer als der verfügbare Speicherplatz ab <paramref name="arrayIndex"/> bis zum Ende des <paramref name="array"/>, das als Ziel festgelegt wurde.- oder -Typ <paramref name="T"/> kann nicht automatisch in den Typ des Ziel-<paramref name="array"/> umgewandelt werden.</exception>
        public void CopyTo(KeyValuePair<string, ElementProcessor>[] array, int arrayIndex)
        {
        }

        /// <summary>
        /// Entfernt das erste Vorkommen eines bestimmten Objekts aus <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// true, wenn <paramref name="item"/> erfolgreich aus <see cref="T:System.Collections.Generic.ICollection`1"/> gelöscht wurde, andernfalls false.Diese Methode gibt auch dann false zurück, wenn <paramref name="item"/> nicht in der ursprünglichen <see cref="T:System.Collections.Generic.ICollection`1"/> gefunden wurde.
        /// </returns>
        /// <param name="item">Das aus dem <see cref="T:System.Collections.Generic.ICollection`1"/> zu entfernende Objekt.</param><exception cref="T:System.NotSupportedException"><see cref="T:System.Collections.Generic.ICollection`1"/> ist schreibgeschützt.</exception>
        public bool Remove(KeyValuePair<string, ElementProcessor> item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Ruft die Anzahl der Elemente ab, die in <see cref="T:System.Collections.Generic.ICollection`1"/> enthalten sind.
        /// </summary>
        /// <returns>
        /// Die Anzahl der Elemente, die in <see cref="T:System.Collections.Generic.ICollection`1"/> enthalten sind.
        /// </returns>
        public int Count { get; private set; }

        /// <summary>
        /// Ruft einen Wert ab, der angibt, ob <see cref="T:System.Collections.Generic.ICollection`1"/> schreibgeschützt ist.
        /// </summary>
        /// <returns>
        /// true, wenn <see cref="T:System.Collections.Generic.ICollection`1"/> schreibgeschützt ist, andernfalls false.
        /// </returns>
        public bool IsReadOnly { get; private set; }

        #endregion

        #region Implementation of IDictionary<string,ElementProcessor>

        /// <summary>
        /// Ermittelt, ob <see cref="T:System.Collections.Generic.IDictionary`2"/> ein Element mit dem angegebenen Schlüssel enthält.
        /// </summary>
        /// <returns>
        /// true, wenn das <see cref="T:System.Collections.Generic.IDictionary`2"/> ein Element mit dem Schlüssel enthält, andernfalls false.
        /// </returns>
        /// <param name="key">Der im <see cref="T:System.Collections.Generic.IDictionary`2"/> zu suchende Schlüssel.</param><exception cref="T:System.ArgumentNullException"><paramref name="key"/> ist null.</exception>
        public bool ContainsKey(string key)
        {
            return processors.ContainsKey(key);
        }

        /// <summary>
        /// Fügt der <see cref="T:System.Collections.Generic.IDictionary`2"/>-Schnittstelle ein Element mit dem angegebenen Schlüssel und Wert hinzu.
        /// </summary>
        /// <param name="key">Das Objekt, das als Schlüssel für das hinzuzufügende Element verwendet werden soll.</param><param name="Content">Das Objekt, das als Wert für das hinzuzufügende Element verwendet werden soll.</param><exception cref="T:System.ArgumentNullException"><paramref name="key"/> ist null.</exception><exception cref="T:System.ArgumentException">Ein Element mit demselben Schlüssel ist bereits in <see cref="T:System.Collections.Generic.IDictionary`2"/> vorhanden.</exception><exception cref="T:System.NotSupportedException"><see cref="T:System.Collections.Generic.IDictionary`2"/> ist schreibgeschützt.</exception>
        void IDictionary<string, ElementProcessor>.Add(string key, ElementProcessor Content)
        {
            Add(new KeyValuePair<string, ElementProcessor>(key, Content));
        }

        /// <summary>
        /// Entfernt das Element mit dem angegebenen Schlüssel aus dem <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </summary>
        /// <returns>
        /// true, wenn das Element erfolgreich entfernt wurde, andernfalls false.Diese Methode gibt auch dann false zurück, wenn <paramref name="key"/> nicht im ursprünglichen <see cref="T:System.Collections.Generic.IDictionary`2"/> gefunden wurde.
        /// </returns>
        /// <param name="key">Der Schlüssel des zu entfernenden Elements.</param><exception cref="T:System.ArgumentNullException"><paramref name="key"/> ist null.</exception><exception cref="T:System.NotSupportedException"><see cref="T:System.Collections.Generic.IDictionary`2"/> ist schreibgeschützt.</exception>
        public bool Remove(string key)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Ruft den dem angegebenen Schlüssel zugeordneten Wert ab.
        /// </summary>
        /// <returns>
        /// true, wenn das Objekt, das <see cref="T:System.Collections.Generic.IDictionary`2"/> implementiert, ein Element mit dem angegebenen Schlüssel enthält, andernfalls false.
        /// </returns>
        /// <param name="key">Der Schlüssel, dessen Wert abgerufen werden soll.</param><param name="Content">Wenn diese Methode zurückgegeben wird, enthält sie den dem angegebenen Schlüssel zugeordneten Wert, wenn der Schlüssel gefunden wird, andernfalls enthält sie den Standardwert für den Typ des <paramref name="Content"/>-Parameters.Dieser Parameter wird nicht initialisiert übergeben.</param><exception cref="T:System.ArgumentNullException"><paramref name="key"/> ist null.</exception>
        public bool TryGetValue(string key, out ElementProcessor Content)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Ruft das Element mit dem angegebenen Schlüssel ab oder legt dieses fest.
        /// </summary>
        /// <returns>
        /// Das Element mit dem angegebenen Schlüssel.
        /// </returns>
        /// <param name="key">Der Schlüssel des abzurufenden oder zu festzulegenden Elements.</param><exception cref="T:System.ArgumentNullException"><paramref name="key"/> ist null.</exception><exception cref="T:System.Collections.Generic.KeyNotFoundException">Die Eigenschaft wird abgerufen, und <paramref name="key"/> wird nicht gefunden.</exception><exception cref="T:System.NotSupportedException">Die Eigenschaft wird festgelegt, und <see cref="T:System.Collections.Generic.IDictionary`2"/> ist schreibgeschützt.</exception>
        public ElementProcessor this[string key]
        {
            get
            {
                if (processors.ContainsKey(key)) return processors[key];
                return null;
            }
            set
            {
                if (ContainsKey(key))
                    processors[key] = value;
                else
                    Add(new KeyValuePair<string, ElementProcessor>(key, value));
            }
        }

        /// <summary>
        /// Ruft eine <see cref="T:System.Collections.Generic.ICollection`1"/>-Schnittstelle ab, die die Schlüssel von <see cref="T:System.Collections.Generic.IDictionary`2"/> enthält.
        /// </summary>
        /// <returns>
        /// Eine <see cref="T:System.Collections.Generic.ICollection`1"/>, die die Schlüssel des Objekts enthält, das <see cref="T:System.Collections.Generic.IDictionary`2"/> implementiert.
        /// </returns>
        public ICollection<string> Keys { get; private set; }

        /// <summary>
        /// Ruft eine <see cref="T:System.Collections.Generic.ICollection`1"/> ab, die die Werte in <see cref="T:System.Collections.Generic.IDictionary`2"/> enthält.
        /// </summary>
        /// <returns>
        /// Eine <see cref="T:System.Collections.Generic.ICollection`1"/>, die die Werte des Objekts enthält, das <see cref="T:System.Collections.Generic.IDictionary`2"/> implementiert.
        /// </returns>
        public ICollection<ElementProcessor> Values { get; private set; }

        #endregion
    }
}