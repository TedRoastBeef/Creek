﻿#region License
// Copyright (c) 2007 James Newton-King
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Lib.JSON.Utilities;

namespace Lib.JSON.Converters
{
  /// <summary>
  /// Converts an <see cref="System.Enum"/> to and from its name string value.
  /// </summary>
  /// <summary>
  /// Converts an <see cref="System.Enum"/> to and from its name string value.
  /// </summary>
  public class StringEnumConverter : JsonConverter
  {
    private readonly Dictionary<Type, BidirectionalDictionary<string, string>> _EnumMemberNamesPerType = new Dictionary<Type, BidirectionalDictionary<string, string>>();

    /// <summary>
    /// Gets or sets a value indicating whether the written System.Enum text should be camel case.
    /// </summary>
    /// <value><c>true</c> if the written System.Enum text will be camel case; otherwise, <c>false</c>.</value>
    public bool CamelCaseText { get; set; }
    
    /// <summary>
    /// Writes the JSON representation of the object.
    /// </summary>
    /// <param name="writer">The <see cref="JsonWriter"/> to write to.</param>
    /// <param name="value">The value.</param>
    /// <param name="serializer">The calling serializer.</param>
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      if (value == null)
      {
        writer.WriteNull();
        return;
      }

      System.Enum e = (System.Enum)value;

      string EnumName = e.ToString("G");

      if (char.IsNumber(EnumName[0]) || EnumName[0] == '-')
      {
        writer.WriteValue(value);
      }
      else
      {
        BidirectionalDictionary<string, string> map = GetEnumNameMap(e.GetType());

        string resolvedEnumName;
        map.TryGetByFirst(EnumName, out resolvedEnumName);
        resolvedEnumName = resolvedEnumName ?? EnumName;

        if (CamelCaseText)
        {
          string[] names = resolvedEnumName.Split(',').Select(item => StringUtils.ToCamelCase(item.Trim())).ToArray();
          resolvedEnumName = string.Join(", ", names);
        }

        writer.WriteValue(resolvedEnumName);
      }
    }

    /// <summary>
    /// Reads the JSON representation of the object.
    /// </summary>
    /// <param name="reader">The <see cref="JsonReader"/> to read from.</param>
    /// <param name="objectType">Type of the object.</param>
    /// <param name="existingValue">The existing value of object being read.</param>
    /// <param name="serializer">The calling serializer.</param>
    /// <returns>The object value.</returns>
    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
      Type t = (ReflectionUtils.IsNullableType(objectType))
      ? Nullable.GetUnderlyingType(objectType)
      : objectType;

      if (reader.TokenType == JsonToken.Null)
      {
        if (!ReflectionUtils.IsNullableType(objectType))
          throw new Exception("Cannot convert null value to {0}.".FormatWith(CultureInfo.InvariantCulture, objectType));

        return null;
      }

      if (reader.TokenType == JsonToken.String)
      {
        var map = GetEnumNameMap(t);
        string resolvedEnumName;
        map.TryGetBySecond(reader.Value.ToString(), out resolvedEnumName);
        resolvedEnumName = resolvedEnumName ?? reader.Value.ToString();

        return System.Enum.Parse(t, resolvedEnumName, true);
      }

      if (reader.TokenType == JsonToken.Integer)
        return ConvertUtils.ConvertOrCast(reader.Value, CultureInfo.InvariantCulture, t);

      throw new Exception("Unexpected token when parsing System.Enum. Expected String or Integer, got {0}.".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
    }

    /// <summary>
    /// A cached representation of the System.Enum string representation to respect per System.Enum field name.
    /// </summary>
    /// <param name="t">The type of the System.Enum.</param>
    /// <returns>A map of System.Enum field name to either the field name, or the configured System.Enum member name (<see cref="EnumMemberAttribute"/>).</returns>
    private BidirectionalDictionary<string, string> GetEnumNameMap(Type t)
    {
      BidirectionalDictionary<string, string> map;

      if (!_EnumMemberNamesPerType.TryGetValue(t, out map))
      {
        lock (_EnumMemberNamesPerType)
        {
          if (_EnumMemberNamesPerType.TryGetValue(t, out map))
            return map;

          map = new BidirectionalDictionary<string, string>(
            StringComparer.OrdinalIgnoreCase,
            StringComparer.OrdinalIgnoreCase);

          foreach (FieldInfo f in t.GetFields())
          {
            string n1 = f.Name;
            string n2;
            
#if !NET20
            n2 = f.GetCustomAttributes(typeof (EnumMemberAttribute), true)
                          .Cast<EnumMemberAttribute>()
                          .Select(a => a.Value)
                          .SingleOrDefault() ?? f.Name;
#else
            n2 = f.Name;
#endif

            string s;
            if (map.TryGetBySecond(n2, out s))
            {
              throw new Exception("System.Enum name '{0}' already exists on System.Enum '{1}'."
                .FormatWith(CultureInfo.InvariantCulture, n2, t.Name));
            }

            map.Add(n1, n2);
          }

          _EnumMemberNamesPerType[t] = map;
        }
      }

      return map;
    }

    /// <summary>
    /// Determines whether this instance can convert the specified object type.
    /// </summary>
    /// <param name="objectType">Type of the object.</param>
    /// <returns>
    /// <c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
    /// </returns>
    public override bool CanConvert(Type objectType)
    {
      Type t = (ReflectionUtils.IsNullableType(objectType))
      ? Nullable.GetUnderlyingType(objectType)
      : objectType;

      return t.IsEnum;
    }
  }
}