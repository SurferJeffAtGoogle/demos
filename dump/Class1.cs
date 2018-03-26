// Copyright (c) 2018 Google LLC.
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not
// use this file except in compliance with the License. You may obtain a copy of
// the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
// WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
// License for the specific language governing permissions and limitations under
// the License.

using System;
using Google.Protobuf;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

/// <summary>
/// Lets Newtonsoft.Json and Protobuf's json converters play nicely
/// together.  The default Netwtonsoft.Json Deserialize method will
/// not correctly deserialize proto messages.
/// </summary>
class ProtoMessageConverter : JsonConverter
{
    public override bool CanConvert(System.Type objectType)
    {
        return typeof(Google.Protobuf.IMessage)
            .IsAssignableFrom(objectType);
    }

    public override object ReadJson(JsonReader reader,
        System.Type objectType, object existingValue,
        JsonSerializer serializer)
    {
        // Read an entire object from the reader.
        var converter = new ExpandoObjectConverter();
        object o = converter.ReadJson(reader, objectType, existingValue,
            serializer);
        // Convert it back to json text.
        string text = JsonConvert.SerializeObject(o);
        // And let protobuf's parser parse the text.
        IMessage message = (IMessage)Activator
            .CreateInstance(objectType);
        return Google.Protobuf.JsonParser.Default.Parse(text,
            message.Descriptor);
    }

    public override void WriteJson(JsonWriter writer, object value,
        JsonSerializer serializer)
    {
        writer.WriteRawValue(Google.Protobuf.JsonFormatter.Default
            .Format((IMessage)value));
    }
}

public class JsonDumper
{
    public static void Dump(object o)
    {
        string text = JsonConvert.SerializeObject(o, new ProtoMessageConverter());
        object clone = JsonConvert.DeserializeObject(text);
        Console.WriteLine(JsonConvert.SerializeObject(clone, Formatting.Indented));
    }
}
