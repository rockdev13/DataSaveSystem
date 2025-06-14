using Newtonsoft.Json;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace SaveLoadSystem
{
    public class UnityJsonConverters
    {
        // Vector2 Converter
        public class Vector2Converter : JsonConverter<Vector2>
        {
            public override void WriteJson(JsonWriter writer, Vector2 value, JsonSerializer serializer)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("x");
                writer.WriteValue(value.x);
                writer.WritePropertyName("y");
                writer.WriteValue(value.y);
                writer.WriteEndObject();
            }

            public override Vector2 ReadJson(JsonReader reader, Type objectType, Vector2 existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                float x = 0, y = 0;

                while (reader.Read())
                {
                    if (reader.TokenType == JsonToken.PropertyName)
                    {
                        string propName = (string)reader.Value;
                        reader.Read();
                        switch (propName)
                        {
                            case "x": x = (float)(double)reader.Value; break;
                            case "y": y = (float)(double)reader.Value; break;
                        }
                    }
                    else if (reader.TokenType == JsonToken.EndObject)
                        break;
                }

                return new Vector2(x, y);
            }
        }

        // Vector3 Converter
        public class Vector3Converter : JsonConverter<Vector3>
        {
            public override void WriteJson(JsonWriter writer, Vector3 value, JsonSerializer serializer)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("x");
                writer.WriteValue(value.x);
                writer.WritePropertyName("y");
                writer.WriteValue(value.y);
                writer.WritePropertyName("z");
                writer.WriteValue(value.z);
                writer.WriteEndObject();
            }

            public override Vector3 ReadJson(JsonReader reader, Type objectType, Vector3 existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                float x = 0, y = 0, z = 0;

                while (reader.Read())
                {
                    if (reader.TokenType == JsonToken.PropertyName)
                    {
                        string propName = (string)reader.Value;
                        reader.Read();
                        switch (propName)
                        {
                            case "x": x = (float)(double)reader.Value; break;
                            case "y": y = (float)(double)reader.Value; break;
                            case "z": z = (float)(double)reader.Value; break;
                        }
                    }
                    else if (reader.TokenType == JsonToken.EndObject)
                        break;
                }

                return new Vector3(x, y, z);
            }
        }

        // Vector4 Converter
        public class Vector4Converter : JsonConverter<Vector4>
        {
            public override void WriteJson(JsonWriter writer, Vector4 value, JsonSerializer serializer)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("x");
                writer.WriteValue(value.x);
                writer.WritePropertyName("y");
                writer.WriteValue(value.y);
                writer.WritePropertyName("z");
                writer.WriteValue(value.z);
                writer.WritePropertyName("w");
                writer.WriteValue(value.w);
                writer.WriteEndObject();
            }

            public override Vector4 ReadJson(JsonReader reader, Type objectType, Vector4 existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                float x = 0, y = 0, z = 0, w = 0;

                while (reader.Read())
                {
                    if (reader.TokenType == JsonToken.PropertyName)
                    {
                        string propName = (string)reader.Value;
                        reader.Read();
                        switch (propName)
                        {
                            case "x": x = (float)(double)reader.Value; break;
                            case "y": y = (float)(double)reader.Value; break;
                            case "z": z = (float)(double)reader.Value; break;
                            case "w": w = (float)(double)reader.Value; break;
                        }
                    }
                    else if (reader.TokenType == JsonToken.EndObject)
                        break;
                }

                return new Vector4(x, y, z, w);
            }
        }

        // Vector2Int Converter
        public class Vector2IntConverter : JsonConverter<Vector2Int>
        {
            public override void WriteJson(JsonWriter writer, Vector2Int value, JsonSerializer serializer)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("x");
                writer.WriteValue(value.x);
                writer.WritePropertyName("y");
                writer.WriteValue(value.y);
                writer.WriteEndObject();
            }

            public override Vector2Int ReadJson(JsonReader reader, Type objectType, Vector2Int existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                int x = 0, y = 0;

                while (reader.Read())
                {
                    if (reader.TokenType == JsonToken.PropertyName)
                    {
                        string propName = (string)reader.Value;
                        reader.Read();
                        switch (propName)
                        {
                            case "x": x = Convert.ToInt32(reader.Value); break;
                            case "y": y = Convert.ToInt32(reader.Value); break;
                        }
                    }
                    else if (reader.TokenType == JsonToken.EndObject)
                        break;
                }

                return new Vector2Int(x, y);
            }
        }

        // Vector3Int Converter
        public class Vector3IntConverter : JsonConverter<Vector3Int>
        {
            public override void WriteJson(JsonWriter writer, Vector3Int value, JsonSerializer serializer)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("x");
                writer.WriteValue(value.x);
                writer.WritePropertyName("y");
                writer.WriteValue(value.y);
                writer.WritePropertyName("z");
                writer.WriteValue(value.z);
                writer.WriteEndObject();
            }

            public override Vector3Int ReadJson(JsonReader reader, Type objectType, Vector3Int existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                int x = 0, y = 0, z = 0;

                while (reader.Read())
                {
                    if (reader.TokenType == JsonToken.PropertyName)
                    {
                        string propName = (string)reader.Value;
                        reader.Read();
                        switch (propName)
                        {
                            case "x": x = Convert.ToInt32(reader.Value); break;
                            case "y": y = Convert.ToInt32(reader.Value); break;
                            case "z": z = Convert.ToInt32(reader.Value); break;
                        }
                    }
                    else if (reader.TokenType == JsonToken.EndObject)
                        break;
                }

                return new Vector3Int(x, y, z);
            }
        }

        // Quaternion Converter
        public class QuaternionConverter : JsonConverter<Quaternion>
        {
            public override void WriteJson(JsonWriter writer, Quaternion value, JsonSerializer serializer)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("x");
                writer.WriteValue(value.x);
                writer.WritePropertyName("y");
                writer.WriteValue(value.y);
                writer.WritePropertyName("z");
                writer.WriteValue(value.z);
                writer.WritePropertyName("w");
                writer.WriteValue(value.w);
                writer.WriteEndObject();
            }

            public override Quaternion ReadJson(JsonReader reader, Type objectType, Quaternion existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                float x = 0, y = 0, z = 0, w = 0;

                while (reader.Read())
                {
                    if (reader.TokenType == JsonToken.PropertyName)
                    {
                        string propName = (string)reader.Value;
                        reader.Read();
                        switch (propName)
                        {
                            case "x": x = (float)(double)reader.Value; break;
                            case "y": y = (float)(double)reader.Value; break;
                            case "z": z = (float)(double)reader.Value; break;
                            case "w": w = (float)(double)reader.Value; break;
                        }
                    }
                    else if (reader.TokenType == JsonToken.EndObject)
                        break;
                }

                return new Quaternion(x, y, z, w);
            }
        }

        // Color Converter
        public class ColorConverter : JsonConverter<Color>
        {
            public override void WriteJson(JsonWriter writer, Color value, JsonSerializer serializer)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("r");
                writer.WriteValue(value.r);
                writer.WritePropertyName("g");
                writer.WriteValue(value.g);
                writer.WritePropertyName("b");
                writer.WriteValue(value.b);
                writer.WritePropertyName("a");
                writer.WriteValue(value.a);
                writer.WriteEndObject();
            }

            public override Color ReadJson(JsonReader reader, Type objectType, Color existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                float r = 0, g = 0, b = 0, a = 1;

                while (reader.Read())
                {
                    if (reader.TokenType == JsonToken.PropertyName)
                    {
                        string propName = (string)reader.Value;
                        reader.Read();
                        switch (propName)
                        {
                            case "r": r = (float)(double)reader.Value; break;
                            case "g": g = (float)(double)reader.Value; break;
                            case "b": b = (float)(double)reader.Value; break;
                            case "a": a = (float)(double)reader.Value; break;
                        }
                    }
                    else if (reader.TokenType == JsonToken.EndObject)
                        break;
                }

                return new Color(r, g, b, a);
            }
        }

        // Color32 Converter
        public class Color32Converter : JsonConverter<Color32>
        {
            public override void WriteJson(JsonWriter writer, Color32 value, JsonSerializer serializer)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("r");
                writer.WriteValue(value.r);
                writer.WritePropertyName("g");
                writer.WriteValue(value.g);
                writer.WritePropertyName("b");
                writer.WriteValue(value.b);
                writer.WritePropertyName("a");
                writer.WriteValue(value.a);
                writer.WriteEndObject();
            }

            public override Color32 ReadJson(JsonReader reader, Type objectType, Color32 existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                byte r = 0, g = 0, b = 0, a = 255;

                while (reader.Read())
                {
                    if (reader.TokenType == JsonToken.PropertyName)
                    {
                        string propName = (string)reader.Value;
                        reader.Read();
                        switch (propName)
                        {
                            case "r": r = Convert.ToByte(reader.Value); break;
                            case "g": g = Convert.ToByte(reader.Value); break;
                            case "b": b = Convert.ToByte(reader.Value); break;
                            case "a": a = Convert.ToByte(reader.Value); break;
                        }
                    }
                    else if (reader.TokenType == JsonToken.EndObject)
                        break;
                }

                return new Color32(r, g, b, a);
            }
        }

        // Rect Converter
        public class RectConverter : JsonConverter<Rect>
        {
            public override void WriteJson(JsonWriter writer, Rect value, JsonSerializer serializer)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("x");
                writer.WriteValue(value.x);
                writer.WritePropertyName("y");
                writer.WriteValue(value.y);
                writer.WritePropertyName("width");
                writer.WriteValue(value.width);
                writer.WritePropertyName("height");
                writer.WriteValue(value.height);
                writer.WriteEndObject();
            }

            public override Rect ReadJson(JsonReader reader, Type objectType, Rect existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                float x = 0, y = 0, width = 0, height = 0;

                while (reader.Read())
                {
                    if (reader.TokenType == JsonToken.PropertyName)
                    {
                        string propName = (string)reader.Value;
                        reader.Read();
                        switch (propName)
                        {
                            case "x": x = (float)(double)reader.Value; break;
                            case "y": y = (float)(double)reader.Value; break;
                            case "width": width = (float)(double)reader.Value; break;
                            case "height": height = (float)(double)reader.Value; break;
                        }
                    }
                    else if (reader.TokenType == JsonToken.EndObject)
                        break;
                }

                return new Rect(x, y, width, height);
            }
        }

        // RectInt Converter
        public class RectIntConverter : JsonConverter<RectInt>
        {
            public override void WriteJson(JsonWriter writer, RectInt value, JsonSerializer serializer)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("x");
                writer.WriteValue(value.x);
                writer.WritePropertyName("y");
                writer.WriteValue(value.y);
                writer.WritePropertyName("width");
                writer.WriteValue(value.width);
                writer.WritePropertyName("height");
                writer.WriteValue(value.height);
                writer.WriteEndObject();
            }

            public override RectInt ReadJson(JsonReader reader, Type objectType, RectInt existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                int x = 0, y = 0, width = 0, height = 0;

                while (reader.Read())
                {
                    if (reader.TokenType == JsonToken.PropertyName)
                    {
                        string propName = (string)reader.Value;
                        reader.Read();
                        switch (propName)
                        {
                            case "x": x = Convert.ToInt32(reader.Value); break;
                            case "y": y = Convert.ToInt32(reader.Value); break;
                            case "width": width = Convert.ToInt32(reader.Value); break;
                            case "height": height = Convert.ToInt32(reader.Value); break;
                        }
                    }
                    else if (reader.TokenType == JsonToken.EndObject)
                        break;
                }

                return new RectInt(x, y, width, height);
            }
        }

        // Bounds Converter
        public class BoundsConverter : JsonConverter<Bounds>
        {
            public override void WriteJson(JsonWriter writer, Bounds value, JsonSerializer serializer)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("center");
                serializer.Serialize(writer, value.center);
                writer.WritePropertyName("size");
                serializer.Serialize(writer, value.size);
                writer.WriteEndObject();
            }

            public override Bounds ReadJson(JsonReader reader, Type objectType, Bounds existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                Vector3 center = Vector3.zero;
                Vector3 size = Vector3.zero;

                while (reader.Read())
                {
                    if (reader.TokenType == JsonToken.PropertyName)
                    {
                        string propName = (string)reader.Value;
                        reader.Read();
                        switch (propName)
                        {
                            case "center": center = serializer.Deserialize<Vector3>(reader); break;
                            case "size": size = serializer.Deserialize<Vector3>(reader); break;
                        }
                    }
                    else if (reader.TokenType == JsonToken.EndObject)
                        break;
                }

                return new Bounds(center, size);
            }
        }

        // BoundsInt Converter
        public class BoundsIntConverter : JsonConverter<BoundsInt>
        {
            public override void WriteJson(JsonWriter writer, BoundsInt value, JsonSerializer serializer)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("position");
                serializer.Serialize(writer, value.position);
                writer.WritePropertyName("size");
                serializer.Serialize(writer, value.size);
                writer.WriteEndObject();
            }

            public override BoundsInt ReadJson(JsonReader reader, Type objectType, BoundsInt existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                Vector3Int position = Vector3Int.zero;
                Vector3Int size = Vector3Int.zero;

                while (reader.Read())
                {
                    if (reader.TokenType == JsonToken.PropertyName)
                    {
                        string propName = (string)reader.Value;
                        reader.Read();
                        switch (propName)
                        {
                            case "position": position = serializer.Deserialize<Vector3Int>(reader); break;
                            case "size": size = serializer.Deserialize<Vector3Int>(reader); break;
                        }
                    }
                    else if (reader.TokenType == JsonToken.EndObject)
                        break;
                }

                return new BoundsInt(position, size);
            }
        }

        // Matrix4x4 Converter
        public class Matrix4x4Converter : JsonConverter<Matrix4x4>
        {
            public override void WriteJson(JsonWriter writer, Matrix4x4 value, JsonSerializer serializer)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("m00"); writer.WriteValue(value.m00);
                writer.WritePropertyName("m01"); writer.WriteValue(value.m01);
                writer.WritePropertyName("m02"); writer.WriteValue(value.m02);
                writer.WritePropertyName("m03"); writer.WriteValue(value.m03);
                writer.WritePropertyName("m10"); writer.WriteValue(value.m10);
                writer.WritePropertyName("m11"); writer.WriteValue(value.m11);
                writer.WritePropertyName("m12"); writer.WriteValue(value.m12);
                writer.WritePropertyName("m13"); writer.WriteValue(value.m13);
                writer.WritePropertyName("m20"); writer.WriteValue(value.m20);
                writer.WritePropertyName("m21"); writer.WriteValue(value.m21);
                writer.WritePropertyName("m22"); writer.WriteValue(value.m22);
                writer.WritePropertyName("m23"); writer.WriteValue(value.m23);
                writer.WritePropertyName("m30"); writer.WriteValue(value.m30);
                writer.WritePropertyName("m31"); writer.WriteValue(value.m31);
                writer.WritePropertyName("m32"); writer.WriteValue(value.m32);
                writer.WritePropertyName("m33"); writer.WriteValue(value.m33);
                writer.WriteEndObject();
            }

            public override Matrix4x4 ReadJson(JsonReader reader, Type objectType, Matrix4x4 existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                Matrix4x4 matrix = new Matrix4x4();

                while (reader.Read())
                {
                    if (reader.TokenType == JsonToken.PropertyName)
                    {
                        string propName = (string)reader.Value;
                        reader.Read();
                        float value = (float)(double)reader.Value;

                        switch (propName)
                        {
                            case "m00": matrix.m00 = value; break;
                            case "m01": matrix.m01 = value; break;
                            case "m02": matrix.m02 = value; break;
                            case "m03": matrix.m03 = value; break;
                            case "m10": matrix.m10 = value; break;
                            case "m11": matrix.m11 = value; break;
                            case "m12": matrix.m12 = value; break;
                            case "m13": matrix.m13 = value; break;
                            case "m20": matrix.m20 = value; break;
                            case "m21": matrix.m21 = value; break;
                            case "m22": matrix.m22 = value; break;
                            case "m23": matrix.m23 = value; break;
                            case "m30": matrix.m30 = value; break;
                            case "m31": matrix.m31 = value; break;
                            case "m32": matrix.m32 = value; break;
                            case "m33": matrix.m33 = value; break;
                        }
                    }
                    else if (reader.TokenType == JsonToken.EndObject)
                        break;
                }

                return matrix;
            }
        }

        // Ray Converter
        public class RayConverter : JsonConverter<Ray>
        {
            public override void WriteJson(JsonWriter writer, Ray value, JsonSerializer serializer)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("origin");
                serializer.Serialize(writer, value.origin);
                writer.WritePropertyName("direction");
                serializer.Serialize(writer, value.direction);
                writer.WriteEndObject();
            }

            public override Ray ReadJson(JsonReader reader, Type objectType, Ray existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                Vector3 origin = Vector3.zero;
                Vector3 direction = Vector3.forward;

                while (reader.Read())
                {
                    if (reader.TokenType == JsonToken.PropertyName)
                    {
                        string propName = (string)reader.Value;
                        reader.Read();
                        switch (propName)
                        {
                            case "origin": origin = serializer.Deserialize<Vector3>(reader); break;
                            case "direction": direction = serializer.Deserialize<Vector3>(reader); break;
                        }
                    }
                    else if (reader.TokenType == JsonToken.EndObject)
                        break;
                }

                return new Ray(origin, direction);
            }
        }

        // Ray2D Converter
        public class Ray2DConverter : JsonConverter<Ray2D>
        {
            public override void WriteJson(JsonWriter writer, Ray2D value, JsonSerializer serializer)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("origin");
                serializer.Serialize(writer, value.origin);
                writer.WritePropertyName("direction");
                serializer.Serialize(writer, value.direction);
                writer.WriteEndObject();
            }

            public override Ray2D ReadJson(JsonReader reader, Type objectType, Ray2D existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                Vector2 origin = Vector2.zero;
                Vector2 direction = Vector2.right;

                while (reader.Read())
                {
                    if (reader.TokenType == JsonToken.PropertyName)
                    {
                        string propName = (string)reader.Value;
                        reader.Read();
                        switch (propName)
                        {
                            case "origin": origin = serializer.Deserialize<Vector2>(reader); break;
                            case "direction": direction = serializer.Deserialize<Vector2>(reader); break;
                        }
                    }
                    else if (reader.TokenType == JsonToken.EndObject)
                        break;
                }

                return new Ray2D(origin, direction);
            }
        }

        // Plane Converter
        public class PlaneConverter : JsonConverter<Plane>
        {
            public override void WriteJson(JsonWriter writer, Plane value, JsonSerializer serializer)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("normal");
                serializer.Serialize(writer, value.normal);
                writer.WritePropertyName("distance");
                writer.WriteValue(value.distance);
                writer.WriteEndObject();
            }

            public override Plane ReadJson(JsonReader reader, Type objectType, Plane existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                Vector3 normal = Vector3.up;
                float distance = 0;

                while (reader.Read())
                {
                    if (reader.TokenType == JsonToken.PropertyName)
                    {
                        string propName = (string)reader.Value;
                        reader.Read();
                        switch (propName)
                        {
                            case "normal": normal = serializer.Deserialize<Vector3>(reader); break;
                            case "distance": distance = (float)(double)reader.Value; break;
                        }
                    }
                    else if (reader.TokenType == JsonToken.EndObject)
                        break;
                }

                return new Plane(normal, distance);
            }
        }

        // AnimationCurve Converter
        public class AnimationCurveConverter : JsonConverter<AnimationCurve>
        {
            public override void WriteJson(JsonWriter writer, AnimationCurve value, JsonSerializer serializer)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("keys");
                writer.WriteStartArray();

                foreach (var key in value.keys)
                {
                    writer.WriteStartObject();
                    writer.WritePropertyName("time");
                    writer.WriteValue(key.time);
                    writer.WritePropertyName("value");
                    writer.WriteValue(key.value);
                    writer.WritePropertyName("inTangent");
                    writer.WriteValue(key.inTangent);
                    writer.WritePropertyName("outTangent");
                    writer.WriteValue(key.outTangent);
                    writer.WritePropertyName("inWeight");
                    writer.WriteValue(key.inWeight);
                    writer.WritePropertyName("outWeight");
                    writer.WriteValue(key.outWeight);
                    writer.WritePropertyName("weightedMode");
                    writer.WriteValue((int)key.weightedMode);
                    writer.WriteEndObject();
                }

                writer.WriteEndArray();
                writer.WritePropertyName("preWrapMode");
                writer.WriteValue((int)value.preWrapMode);
                writer.WritePropertyName("postWrapMode");
                writer.WriteValue((int)value.postWrapMode);
                writer.WriteEndObject();
            }

            public override AnimationCurve ReadJson(JsonReader reader, Type objectType, AnimationCurve existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                var keys = new List<Keyframe>();
                WrapMode preWrapMode = WrapMode.Once;
                WrapMode postWrapMode = WrapMode.Once;

                while (reader.Read())
                {
                    if (reader.TokenType == JsonToken.PropertyName)
                    {
                        string propName = (string)reader.Value;
                        reader.Read();

                        switch (propName)
                        {
                            case "keys":
                                while (reader.Read() && reader.TokenType != JsonToken.EndArray)
                                {
                                    if (reader.TokenType == JsonToken.StartObject)
                                    {
                                        float time = 0, value = 0, inTangent = 0, outTangent = 0, inWeight = 0, outWeight = 0;
                                        WeightedMode weightedMode = WeightedMode.None;

                                        while (reader.Read() && reader.TokenType != JsonToken.EndObject)
                                        {
                                            if (reader.TokenType == JsonToken.PropertyName)
                                            {
                                                string keyProp = (string)reader.Value;
                                                reader.Read();
                                                switch (keyProp)
                                                {
                                                    case "time": time = (float)(double)reader.Value; break;
                                                    case "value": value = (float)(double)reader.Value; break;
                                                    case "inTangent": inTangent = (float)(double)reader.Value; break;
                                                    case "outTangent": outTangent = (float)(double)reader.Value; break;
                                                    case "inWeight": inWeight = (float)(double)reader.Value; break;
                                                    case "outWeight": outWeight = (float)(double)reader.Value; break;
                                                    case "weightedMode": weightedMode = (WeightedMode)Convert.ToInt32(reader.Value); break;
                                                }
                                            }
                                        }

                                        var keyframe = new Keyframe(time, value, inTangent, outTangent, inWeight, outWeight);
                                        keyframe.weightedMode = weightedMode;
                                        keys.Add(keyframe);
                                    }
                                }
                                break;
                            case "preWrapMode":
                                preWrapMode = (WrapMode)Convert.ToInt32(reader.Value);
                                break;
                            case "postWrapMode":
                                postWrapMode = (WrapMode)Convert.ToInt32(reader.Value);
                                break;
                        }
                    }
                    else if (reader.TokenType == JsonToken.EndObject)
                        break;
                }

                var curve = new AnimationCurve(keys.ToArray());
                curve.preWrapMode = preWrapMode;
                curve.postWrapMode = postWrapMode;
                return curve;
            }
        }

        // Gradient Converter
        public class GradientConverter : JsonConverter<Gradient>
        {
            public override void WriteJson(JsonWriter writer, Gradient value, JsonSerializer serializer)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("mode");
                writer.WriteValue((int)value.mode);

                writer.WritePropertyName("colorKeys");
                writer.WriteStartArray();
                foreach (var key in value.colorKeys)
                {
                    writer.WriteStartObject();
                    writer.WritePropertyName("color");
                    serializer.Serialize(writer, key.color);
                    writer.WritePropertyName("time");
                    writer.WriteValue(key.time);
                    writer.WriteEndObject();
                }
                writer.WriteEndArray();

                writer.WritePropertyName("alphaKeys");
                writer.WriteStartArray();
                foreach (var key in value.alphaKeys)
                {
                    writer.WriteStartObject();
                    writer.WritePropertyName("alpha");
                    writer.WriteValue(key.alpha);
                    writer.WritePropertyName("time");
                    writer.WriteValue(key.time);
                    writer.WriteEndObject();
                }
                writer.WriteEndArray();
                writer.WriteEndObject();
            }

            public override Gradient ReadJson(JsonReader reader, Type objectType, Gradient existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                var gradient = new Gradient();
                var colorKeys = new List<GradientColorKey>();
                var alphaKeys = new List<GradientAlphaKey>();

                while (reader.Read())
                {
                    if (reader.TokenType == JsonToken.PropertyName)
                    {
                        string propName = reader.Value.ToString();
                        reader.Read();

                        switch (propName)
                        {
                            case "mode":
                                gradient.mode = (GradientMode)reader.Value;
                                break;

                            case "colorKeys":
                                while (reader.Read() && reader.TokenType != JsonToken.EndArray)
                                {
                                    if (reader.TokenType == JsonToken.StartObject)
                                    {
                                        var colorKey = ReadColorKey(reader, serializer);
                                        colorKeys.Add(colorKey);
                                    }
                                }
                                break;

                            case "alphaKeys":
                                while (reader.Read() && reader.TokenType != JsonToken.EndArray)
                                {
                                    if (reader.TokenType == JsonToken.StartObject)
                                    {
                                        var alphaKey = ReadAlphaKey(reader);
                                        alphaKeys.Add(alphaKey);
                                    }
                                }
                                break;
                        }
                    }
                    else if (reader.TokenType == JsonToken.EndObject)
                    {
                        break;
                    }
                }

                gradient.SetKeys(colorKeys.ToArray(), alphaKeys.ToArray());
                return gradient;
            }

            private GradientColorKey ReadColorKey(JsonReader reader, JsonSerializer serializer)
            {
                var colorKey = new GradientColorKey();

                while (reader.Read() && reader.TokenType != JsonToken.EndObject)
                {
                    if (reader.TokenType == JsonToken.PropertyName)
                    {
                        string propName = reader.Value.ToString();
                        reader.Read();

                        switch (propName)
                        {
                            case "color":
                                colorKey.color = serializer.Deserialize<Color>(reader);
                                break;
                            case "time":
                                colorKey.time = Convert.ToSingle(reader.Value);
                                break;
                        }
                    }
                }

                return colorKey;
            }

            private GradientAlphaKey ReadAlphaKey(JsonReader reader)
            {
                var alphaKey = new GradientAlphaKey();

                while (reader.Read() && reader.TokenType != JsonToken.EndObject)
                {
                    if (reader.TokenType == JsonToken.PropertyName)
                    {
                        string propName = reader.Value.ToString();
                        reader.Read();

                        switch (propName)
                        {
                            case "alpha":
                                alphaKey.alpha = Convert.ToSingle(reader.Value);
                                break;
                            case "time":
                                alphaKey.time = Convert.ToSingle(reader.Value);
                                break;
                        }
                    }
                }

                return alphaKey;
            }
        }
    }
}