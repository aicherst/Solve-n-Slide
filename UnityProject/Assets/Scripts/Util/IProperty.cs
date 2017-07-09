using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using System.Xml;
using System.Xml.Serialization;

public interface ReadOnlyProperty<T> {
    T data {
        get;
    }

    void AddListener(PropertyChangeEventHandler<T> listener);
    void RemoveListener(PropertyChangeEventHandler<T> listener);
}

public interface IProperty<T> : ReadOnlyProperty<T> {
    void SetData(T value);

    void Bind(ReadOnlyProperty<T> property);
}

public class Property<T> : IProperty<T>, IXmlSerializable {
    private event PropertyChangeEventHandler<T> changeListeners;

    protected T _data;

    private bool updating;

    private ReadOnlyProperty<T> bindTo;

    public Property() : this(default(T)) {

    }

    public void AddListener(PropertyChangeEventHandler<T> listener) {
        changeListeners += listener;

        listener(this, _data, default(T));
    }

    public void RemoveListener(PropertyChangeEventHandler<T> listener) {
        changeListeners -= listener;
    }

    public Property(T data) {
        _data = data;
    }

    public T data {
        get {
            return _data;
        }
    }

    protected virtual void ApplySetData(T newData) {
        T oldData = _data;
        _data = newData;

        UpdateListeners(newData, oldData);
    }

    public void SetData(T newData) {
        if (bindTo != null) {
            Bind(null);
        }

        if (_data == null) {
            if (newData == null) {
                return;
            }
        } else if (_data.Equals(newData)) {
            return;
        }

        ApplySetData(newData);
    }

    private void UpdateListeners(T newData, T oldData) {
        if (changeListeners == null)
            return;
        changeListeners(this, newData, oldData);
    }

    public static implicit operator T(Property<T> property) { return property.data; }


    public System.Xml.Schema.XmlSchema GetSchema() {
        return null;
    }

    public void ReadXml(XmlReader reader) {
        if (reader.IsEmptyElement || reader.Read() == false)
            return;

        XmlSerializer inner = new XmlSerializer(typeof(T));
        _data = (T)inner.Deserialize(reader);

        reader.ReadEndElement();
    }

    public void WriteXml(XmlWriter writer) {
        XmlSerializer inner = new XmlSerializer(typeof(T));
        inner.Serialize(writer, _data);
    }

    public void Bind(ReadOnlyProperty<T> property) {
        if (bindTo != null) {
            bindTo.RemoveListener(bindToChanged);
            bindTo = null;
        }

        if (property != null) {
            SetData(property.data);
            bindTo = property;
            bindTo.AddListener(bindToChanged);
        }
    }

    void bindToChanged(ReadOnlyProperty<T> changedProperty, T newData, T oldData) {
        ApplySetData(newData);
    }
}

public delegate void PropertyChangeEventHandler<T>(ReadOnlyProperty<T> changedProperty, T newData, T oldData);